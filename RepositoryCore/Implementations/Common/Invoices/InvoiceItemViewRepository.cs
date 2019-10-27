using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Invoices;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class InvoiceItemViewRepository : IInvoiceItemRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        string selectString =
           "SELECT " +
            "InvoiceItemId, " +
            "InvoiceItemIdentifier, " +
            "InvoiceItemCode, " +
            "InvoiceItemName, " +
            "InvoiceId, " +
            "InvoiceIdentifier, " +
            "InvoiceCode, " +
            "UnitOfMeasure, " +
            "Quantity, " +
            "PriceWithPDV, " +
            "PriceWithoutPDV, " +
            "Discount, " +
            "PDVPercent, " +
            "PDV, " +
            "Amount, " +
            "ItemStatus, " +
            "Active, " +
            "UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vInvoiceItems ";
        public InvoiceItemViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }
        private static InvoiceItem Read(SqlDataReader reader)
        {
            InvoiceItem invoiceItem = new InvoiceItem();
            invoiceItem.Id = Int32.Parse(reader["InvoiceItemId"].ToString());
            invoiceItem.Identifier = Guid.Parse(reader["InvoiceItemIdentifier"].ToString());
            invoiceItem.Code = reader["InvoiceItemCode"].ToString();
            invoiceItem.Name = reader["InvoiceItemName"].ToString();

            if (reader["InvoiceId"] != DBNull.Value)
            {
                invoiceItem.Invoice = new Invoice();
                invoiceItem.InvoiceId = Int32.Parse(reader["InvoiceId"].ToString());
                invoiceItem.Invoice.Id = Int32.Parse(reader["InvoiceId"].ToString());
                invoiceItem.Invoice.Identifier = Guid.Parse(reader["InvoiceIdentifier"].ToString());
                invoiceItem.Invoice.Code = reader["InvoiceCode"].ToString();
            }


            if (reader["UnitOfMeasure"] != DBNull.Value)
                invoiceItem.UnitOfMeasure = reader["UnitOfMeasure"].ToString();

            if (reader["Quantity"] != DBNull.Value)
                invoiceItem.Quantity = Decimal.Parse(reader["Quantity"].ToString());
            if (reader["PriceWithPDV"] != DBNull.Value)
                invoiceItem.PriceWithPDV = Decimal.Parse(reader["PriceWithPDV"].ToString());
            if (reader["PriceWithoutPDV"] != DBNull.Value)
                invoiceItem.PriceWithoutPDV = Decimal.Parse(reader["PriceWithoutPDV"].ToString());
            if (reader["Discount"] != DBNull.Value)
                invoiceItem.Discount = Decimal.Parse(reader["Discount"].ToString());
            if (reader["PDVPercent"] != DBNull.Value)
                invoiceItem.PDVPercent = Decimal.Parse(reader["PDVPercent"].ToString());
            if (reader["PDV"] != DBNull.Value)
                invoiceItem.PDV = Decimal.Parse(reader["PDV"].ToString());
            if (reader["Amount"] != DBNull.Value)
                invoiceItem.Amount = Decimal.Parse(reader["Amount"].ToString());
            if (reader["ItemStatus"] != DBNull.Value)
                invoiceItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

            invoiceItem.Active = bool.Parse(reader["Active"].ToString());
            invoiceItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                invoiceItem.CreatedBy = new User();
                invoiceItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                invoiceItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                invoiceItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                invoiceItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                invoiceItem.Company = new Company();
                invoiceItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                invoiceItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                invoiceItem.Company.Name = reader["CompanyName"].ToString();
            }

            return invoiceItem;
        }
        public List<InvoiceItem> GetInvoiceItems(int companyId)
        {
            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();

            string queryString = selectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InvoiceItem invoiceItem;
                    while (reader.Read())
                    {
                        invoiceItem = Read(reader);
                        invoiceItems.Add(invoiceItem);
                    }
                }
            }

            return invoiceItems;
        }

        public List<InvoiceItem> GetInvoiceItemsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();

            string queryString = selectString +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(NVARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(NVARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));


                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InvoiceItem invoiceItem;
                    while (reader.Read())
                    {
                        invoiceItem = Read(reader);
                        invoiceItems.Add(invoiceItem);
                    }
                }
            }

            return invoiceItems;
        }
        private string GetNewCodeValue(int companyId)
        {
            int count = context.InvoiceItems
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InvoiceItem))
                    .Select(x => x.Entity as InvoiceItem))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.InvoiceItems
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InvoiceItem))
                        .Select(x => x.Entity as InvoiceItem))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    ?.Code ?? "";

                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return "";
            }
        }
        public InvoiceItem Create(InvoiceItem invoiceItem)
        {
            if (context.InvoiceItems.Where(x => x.Identifier != null && x.Identifier == invoiceItem.Identifier).Count() == 0)
            {
                invoiceItem.Id = 0;

                invoiceItem.Active = true;
                invoiceItem.Code = GetNewCodeValue(invoiceItem.CompanyId ?? 0);

                invoiceItem.UpdatedAt = DateTime.Now;
                invoiceItem.CreatedAt = DateTime.Now;

                context.InvoiceItems.Add(invoiceItem);
                return invoiceItem;
            }
            else
            {
                InvoiceItem dbEntry = context.InvoiceItems
                    .FirstOrDefault(x => x.Identifier == invoiceItem.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.InvoiceId = invoiceItem.InvoiceId;
                    dbEntry.CompanyId = invoiceItem.CompanyId;
                    dbEntry.CreatedById = invoiceItem.CreatedById;

                    dbEntry.Code = invoiceItem.Code;
                    dbEntry.UnitOfMeasure = invoiceItem.UnitOfMeasure;
                    dbEntry.Quantity = invoiceItem.Quantity;
                    dbEntry.PriceWithPDV = invoiceItem.PriceWithPDV;
                    dbEntry.PriceWithoutPDV = invoiceItem.PriceWithoutPDV;
                    dbEntry.Discount = invoiceItem.Discount;
                    dbEntry.PDVPercent = invoiceItem.PDVPercent;
                    dbEntry.PDV = invoiceItem.PDV;
                    dbEntry.Amount = invoiceItem.Amount;

                    dbEntry.ItemStatus = invoiceItem.ItemStatus;

                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return invoiceItem;
            }
        }

        public InvoiceItem Delete(Guid identifier)
        {
            InvoiceItem dbEntry = context.InvoiceItems
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InvoiceItem))
                    .Select(x => x.Entity as InvoiceItem))
                .FirstOrDefault(x => x.Identifier == identifier);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }

}
