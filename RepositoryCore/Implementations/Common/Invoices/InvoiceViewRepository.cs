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
              "SELECT InvoiceId, InvoiceIdentifier, InvoiceCode, " +
              "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
              "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +
              "VatId, VatIdentifier, VatCode, VatDescription, VatAmount, " +
              "CityId, CityIdentifier, CityZipCode, CityName, " +
              "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
              "InvoiceNumber, InvoiceDate, DateOfSupplyOfGoods," +
              "Customer, PIB, BPName, Address," +
              "Currency, IsInPDV, Active, UpdatedAt," +
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

            if (reader["BusinessPartnerId"] != DBNull.Value)
            {
                invoice.BusinessPartner = new BusinessPartner();
                invoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                invoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                invoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                invoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                invoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
            }

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

            if (reader["InvoiceNumber"] != DBNull.Value)
                invoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
            if (reader["InvoiceDate"] != DBNull.Value)
                invoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
            if (reader["DateOfSupplyOfGoods"] != DBNull.Value)
                invoice.DateOfSupplyOfGoods = DateTime.Parse(reader["DateOfSupplyOfGoods"].ToString());

            if (reader["Customer"] != DBNull.Value)
                invoice.Customer = reader["Customer"].ToString();
            if (reader["PIB"] != DBNull.Value)
                invoice.PIB = reader["PIB"].ToString();
            if (reader["BusinessPartnerName"] != DBNull.Value)
                invoice.BPName = reader["BPName"].ToString();
            if (reader["Address"] != DBNull.Value)
                invoice.Address = reader["Address"].ToString();
            if (reader["Currency"] != DBNull.Value)
                invoice.Currency = DateTime.Parse(reader["Currency"].ToString());
            if (reader["IsInPDV"] != DBNull.Value)
                invoice.IsInPDV = bool.Parse(reader["IsInPDV"].ToString());
            
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
        public Invoice Create(Invoice invoice)
        {
            if (context.Invoices.Where(x => x.Identifier != null && x.Identifier == invoice.Identifier).Count() == 0)
            {
                invoice.Id = 0;

                invoice.Code = GetNewCodeValue(invoice.CompanyId ?? 0);

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
                    dbEntry.BusinessPartnerId = invoice.BusinessPartnerId ?? null;
                    dbEntry.DiscountId = invoice.DiscountId ?? null;
                    dbEntry.VatId = invoice.VatId ?? null;
                    dbEntry.CityId = invoice.CityId ?? null;
                    dbEntry.MunicipalityId = invoice.MunicipalityId ?? null;
                    dbEntry.CompanyId = invoice.CompanyId ?? null;
                    dbEntry.CreatedById = invoice.CreatedById ?? null;

                    dbEntry.InvoiceNumber = invoice.InvoiceNumber;

                    dbEntry.InvoiceDate = invoice.InvoiceDate;
                    dbEntry.DateOfSupplyOfGoods = invoice.DateOfSupplyOfGoods;
                    dbEntry.Customer = invoice.Customer;
                    dbEntry.PIB = invoice.PIB;
                    dbEntry.BPName = invoice.BPName;
                    dbEntry.Address = invoice.Address;
                    dbEntry.Currency = invoice.Currency;
                    dbEntry.IsInPDV = invoice.IsInPDV;

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
