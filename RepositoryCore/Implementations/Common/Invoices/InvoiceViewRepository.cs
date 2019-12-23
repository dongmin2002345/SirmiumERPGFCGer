using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Invoices;
using DomainCore.Common.Locations;
using DomainCore.Common.Prices;
using DomainCore.Vats;
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
    public class InvoiceViewRepository : IInvoiceRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        string selectString =
              "SELECT InvoiceId, InvoiceIdentifier, InvoiceCode, InvoiceNumber, " +

              "BuyerId, BuyerIdentifier, BuyerCode, BuyerName, EnteredBuyerName, " +
              "Address, InvoiceDate, DueDate, DateOfPayment, Status, StatusDate, Description, " +
              "CurrencyCode, CurrencyExchangeRate, " +

              "CityId, CityIdentifier, CityZipCode, CityName, " +
              "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
              "VatId, VatIdentifier, VatCode, VatDescription, VatAmount, " +
              "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +

              "PdvType, TotalPrice, TotalPDV, TotalRebate, Active, UpdatedAt," +
              "CreatedById, CreatedByFirstName, CreatedByLastName, " +
              "CompanyId, CompanyName " +
              "FROM vInvoices ";

        public InvoiceViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        private static Invoice Read(SqlDataReader reader)
        {
            Invoice invoice = new Invoice();
            invoice.Id = Int32.Parse(reader["InvoiceId"].ToString());
            invoice.Identifier = Guid.Parse(reader["InvoiceIdentifier"].ToString());
            invoice.Code = reader["InvoiceCode"].ToString();
            invoice.InvoiceNumber = reader["InvoiceNumber"]?.ToString();

            if (reader["BuyerId"] != DBNull.Value)
            {
                invoice.Buyer = new BusinessPartner();
                invoice.BuyerId = Int32.Parse(reader["BuyerId"].ToString());
                invoice.Buyer.Id = Int32.Parse(reader["BuyerId"].ToString());
                invoice.Buyer.Identifier = Guid.Parse(reader["BuyerIdentifier"].ToString());
                invoice.Buyer.Code = reader["BuyerCode"].ToString();
                invoice.Buyer.Name = reader["BuyerName"].ToString();
            }

            invoice.BuyerName = reader["EnteredBuyerName"]?.ToString();
            invoice.Address = reader["Address"]?.ToString();

            if (reader["InvoiceDate"] != DBNull.Value)
                invoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());

            if (reader["DueDate"] != DBNull.Value)
                invoice.DueDate = DateTime.Parse(reader["DueDate"].ToString());

            if (reader["DateOfPayment"] != DBNull.Value)
                invoice.DateOfPayment = DateTime.Parse(reader["DateOfPayment"].ToString());

            if (reader["Status"] != DBNull.Value)
                invoice.Status = Int32.Parse(reader["Status"].ToString());

            if (reader["StatusDate"] != DBNull.Value)
                invoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());

            invoice.Description = reader["Description"]?.ToString();
            invoice.CurrencyCode = reader["CurrencyCode"]?.ToString();

            if (reader["CurrencyExchangeRate"] != DBNull.Value)
                invoice.CurrencyExchangeRate = double.Parse(reader["CurrencyExchangeRate"].ToString());


            if (reader["DiscountId"] != DBNull.Value)
            {
                invoice.Discount = new Discount();
                invoice.DiscountId = Int32.Parse(reader["DiscountId"].ToString());
                invoice.Discount.Id = Int32.Parse(reader["DiscountId"].ToString());
                invoice.Discount.Identifier = Guid.Parse(reader["DiscountIdentifier"].ToString());
                invoice.Discount.Code = reader["DiscountCode"].ToString();
                invoice.Discount.Name = reader["DiscountName"].ToString();
                invoice.Discount.Amount = decimal.Parse(reader["DiscountAmount"].ToString());
            }

            if (reader["VatId"] != DBNull.Value)
            {
                invoice.Vat = new Vat();
                invoice.VatId = Int32.Parse(reader["VatId"].ToString());
                invoice.Vat.Id = Int32.Parse(reader["VatId"].ToString());
                invoice.Vat.Identifier = Guid.Parse(reader["VatIdentifier"].ToString());
                invoice.Vat.Code = reader["VatCode"].ToString();
                invoice.Vat.Description = reader["VatDescription"].ToString();
                invoice.Vat.Amount = decimal.Parse(reader["VatAmount"].ToString());
            }

            if (reader["CityId"] != DBNull.Value)
            {
                invoice.City = new City();
                invoice.CityId = Int32.Parse(reader["CityId"].ToString());
                invoice.City.Id = Int32.Parse(reader["CityId"].ToString());
                invoice.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                invoice.City.ZipCode = reader["CityZipCode"].ToString();
                invoice.City.Name = reader["CityName"].ToString();
            }

            if (reader["MunicipalityId"] != DBNull.Value)
            {
                invoice.Municipality = new Municipality();
                invoice.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                invoice.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                invoice.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                invoice.Municipality.Code = reader["MunicipalityCode"].ToString();
                invoice.Municipality.Name = reader["MunicipalityName"].ToString();
            }


            if (reader["PdvType"] != DBNull.Value)
                invoice.PdvType = Int32.Parse(reader["PdvType"].ToString());

            if (reader["TotalPrice"] != DBNull.Value)
                invoice.TotalPrice = double.Parse(reader["TotalPrice"].ToString());
            if (reader["TotalPDV"] != DBNull.Value)
                invoice.TotalPDV = double.Parse(reader["TotalPDV"].ToString());
            if (reader["TotalRebate"] != DBNull.Value)
                invoice.TotalRebate = double.Parse(reader["TotalRebate"].ToString());

            invoice.Active = bool.Parse(reader["Active"].ToString());
            invoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                invoice.CreatedBy = new User();
                invoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                invoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                invoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                invoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                invoice.Company = new Company();
                invoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                invoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                invoice.Company.Name = reader["CompanyName"].ToString();
            }

            return invoice;
        }
        public List<Invoice> GetInvoices(int companyId)
        {
            List<Invoice> invoices = new List<Invoice>();

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
                    Invoice invoice = null;
                    if (reader.Read())
                    {
                        invoice = Read(reader);
                        invoices.Add(invoice);
                    }
                }
            }

            return invoices;
        }

        public List<Invoice> GetInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<Invoice> invoices = new List<Invoice>();

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
                    Invoice invoice = null;
                    while (reader.Read())
                    {
                        invoice = Read(reader);
                        invoices.Add(invoice);
                    }
                }
            }
            return invoices;
        }
        private string GetNewCodeValue(int companyId)
        {
            int count = context.Invoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Invoice))
                    .Select(x => x.Entity as Invoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Invoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Invoice))
                        .Select(x => x.Entity as Invoice))
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
        private string GetNewInvoiceNumber(int companyId)
        {
            int count = context.Invoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Invoice))
                    .Select(x => x.Entity as Invoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "00001/" + DateTime.Now.Year;
            else
            {
                int activeInvoices = context.Invoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Invoice))
                        .Select(x => x.Entity as Invoice))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).Count();

                if (activeInvoices >= 0)
                {
                    return (activeInvoices + 1).ToString("00000") + "/" + DateTime.Now.Year;
                }
                else
                    return "";
            }
        }
        public Invoice Create(Invoice invoice)
        {
            if (context.Invoices.Where(x => x.Identifier != null && x.Identifier == invoice.Identifier).Count() == 0)
            {
                invoice.Id = 0;

                invoice.Code = GetNewCodeValue(invoice.CompanyId ?? 0);
                invoice.InvoiceNumber = GetNewInvoiceNumber(invoice.CompanyId ?? 0);

                invoice.Active = true;

                invoice.UpdatedAt = DateTime.Now;
                invoice.CreatedAt = DateTime.Now;

                context.Invoices.Add(invoice);
                return invoice;
            }
            else
            {
                // Load item that will be updated
                Invoice dbEntry = context.Invoices
                    .FirstOrDefault(x => x.Identifier == invoice.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BuyerId = invoice.BuyerId ?? null;
                    dbEntry.DiscountId = invoice.DiscountId ?? null;
                    dbEntry.VatId = invoice.VatId ?? null;
                    dbEntry.CityId = invoice.CityId ?? null;
                    dbEntry.MunicipalityId = invoice.MunicipalityId ?? null;
                    dbEntry.CompanyId = invoice.CompanyId ?? null;
                    dbEntry.CreatedById = invoice.CreatedById ?? null;

                    dbEntry.InvoiceNumber = invoice.InvoiceNumber;

                    dbEntry.BuyerName = invoice.BuyerName;
                    dbEntry.Address = invoice.Address;
                    dbEntry.InvoiceDate = invoice.InvoiceDate;
                    dbEntry.DueDate = invoice.DueDate;
                    dbEntry.DateOfPayment = invoice.DateOfPayment;
                    dbEntry.Status = invoice.Status;
                    dbEntry.StatusDate = invoice.StatusDate;

                    dbEntry.Description = invoice.Description;
                    dbEntry.CurrencyCode = invoice.CurrencyCode;
                    dbEntry.CurrencyExchangeRate = invoice.CurrencyExchangeRate;
                    dbEntry.PdvType = invoice.PdvType;

                    dbEntry.TotalPrice = invoice.TotalPrice;
                    dbEntry.TotalPDV = invoice.TotalPDV;
                    dbEntry.TotalRebate = invoice.TotalRebate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Invoice Delete(Guid identifier)
        {
            // Load Invoice that will be deleted
            Invoice dbEntry = context.Invoices
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
