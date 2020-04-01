using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.InputInvoices;
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
    public class InputInvoiceViewRepository : IInputInvoiceRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public InputInvoiceViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<InputInvoice> GetInputInvoices(int companyId)
        {
            List<InputInvoice> InputInvoices = new List<InputInvoice>();

            string queryString =
                "SELECT InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, AmountGross, Currency, DateOfPaymet, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vInputInvoices " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InputInvoice inputInvoice;
                    while(reader.Read())
                    {
                        inputInvoice = new InputInvoice();
                        inputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
                        inputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
                        inputInvoice.Code = reader["InputInvoiceCode"]?.ToString();

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            inputInvoice.BusinessPartner = new BusinessPartner();
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                inputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerIdentifier"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            if (reader["BusinessPartnerCode"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            if (reader["BusinessPartnerName"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                            if (reader["BusinessPartnerInternalCode"] != DBNull.Value)
                                inputInvoice.BusinessPartner.InternalCode = reader["BusinessPartnerInternalCode"].ToString();
                            if (reader["BusinessPartnerNameGer"] != DBNull.Value)
                                inputInvoice.BusinessPartner.NameGer = reader["BusinessPartnerNameGer"].ToString();
                        }

                        if (reader["Supplier"] != DBNull.Value)
                            inputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            inputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != DBNull.Value)
                            inputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != DBNull.Value)
                            inputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != DBNull.Value)
                            inputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PDVPercent"] != DBNull.Value)
                            inputInvoice.PDVPercent = Int32.Parse(reader["PDVPercent"].ToString());
                        if (reader["PDV"] != DBNull.Value)
                            inputInvoice.PDV = decimal.Parse(reader["PDV"].ToString());
                        if (reader["AmountGross"] != DBNull.Value)
                            inputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != DBNull.Value)
                            inputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPaymet"] != DBNull.Value)
                            inputInvoice.DateOfPaymet = DateTime.Parse(reader["DateOfPaymet"].ToString());
                        if (reader["Status"] != DBNull.Value)
                            inputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != DBNull.Value)
                            inputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            inputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            inputInvoice.Path = reader["Path"].ToString();

                        inputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        inputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if(reader["CreatedById"] != DBNull.Value)
                        {
                            inputInvoice.CreatedBy = new User();
                            if (reader["CreatedById"] != DBNull.Value)
                                inputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedById"] != DBNull.Value)
                                inputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedByFirstName"] != DBNull.Value)
                                inputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            if (reader["CreatedByLastName"] != DBNull.Value)
                                inputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if(reader["CompanyId"] != DBNull.Value)
                        {
                            inputInvoice.Company = new Company();
                            if (reader["CompanyId"] != DBNull.Value)
                                inputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyId"] != DBNull.Value)
                                inputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyName"] != DBNull.Value)
                                inputInvoice.Company.Name = reader["CompanyName"].ToString();
                        }

                        InputInvoices.Add(inputInvoice);

                    }
                }
            }

                //List<InputInvoice> InputInvoices = context.InputInvoices
                //    .Include(x => x.BusinessPartner)
                //    .Include(x => x.Company)
                //    .Include(x => x.CreatedBy)
                //    .Where(x => x.Active == true && x.CompanyId == companyId)
                //    .OrderByDescending(x => x.CreatedAt)
                //    .AsNoTracking()
                //    .ToList();

                return InputInvoices;
        }

        public List<InputInvoice> GetInputInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<InputInvoice> InputInvoices = new List<InputInvoice>();

            string queryString =
                "SELECT InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, AmountGross, Currency, DateOfPaymet, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vInputInvoices " +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InputInvoice inputInvoice;
                    while (reader.Read())
                    {
                        inputInvoice = new InputInvoice();
                        inputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
                        inputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
                        inputInvoice.Code = reader["InputInvoiceCode"]?.ToString();

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            inputInvoice.BusinessPartner = new BusinessPartner();
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                inputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerIdentifier"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            if (reader["BusinessPartnerCode"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            if (reader["BusinessPartnerName"] != DBNull.Value)
                                inputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                            if (reader["BusinessPartnerInternalCode"] != DBNull.Value)
                                inputInvoice.BusinessPartner.InternalCode = reader["BusinessPartnerInternalCode"].ToString();
                            if (reader["BusinessPartnerNameGer"] != DBNull.Value)
                                inputInvoice.BusinessPartner.NameGer = reader["BusinessPartnerNameGer"].ToString();
                        }

                        if (reader["Supplier"] != DBNull.Value)
                            inputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            inputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != DBNull.Value)
                            inputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != DBNull.Value)
                            inputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != DBNull.Value)
                            inputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PDVPercent"] != DBNull.Value)
                            inputInvoice.PDVPercent = Int32.Parse(reader["PDVPercent"].ToString());
                        if (reader["PDV"] != DBNull.Value)
                            inputInvoice.PDV = decimal.Parse(reader["PDV"].ToString());
                        if (reader["AmountGross"] != DBNull.Value)
                            inputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != DBNull.Value)
                            inputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPaymet"] != DBNull.Value)
                            inputInvoice.DateOfPaymet = DateTime.Parse(reader["DateOfPaymet"].ToString());
                        if (reader["Status"] != DBNull.Value)
                            inputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != DBNull.Value)
                            inputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            inputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            inputInvoice.Path = reader["Path"].ToString();

                        inputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        inputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            inputInvoice.CreatedBy = new User();
                            if (reader["CreatedById"] != DBNull.Value)
                                inputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedById"] != DBNull.Value)
                                inputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedByFirstName"] != DBNull.Value)
                                inputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            if (reader["CreatedByLastName"] != DBNull.Value)
                                inputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            inputInvoice.Company = new Company();
                            if (reader["CompanyId"] != DBNull.Value)
                                inputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyId"] != DBNull.Value)
                                inputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyName"] != DBNull.Value)
                                inputInvoice.Company.Name = reader["CompanyName"].ToString();
                        }

                        InputInvoices.Add(inputInvoice);

                    }
                }
            }

            //List<InputInvoice> InputInvoices = context.InputInvoices
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return InputInvoices;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.InputInvoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InputInvoice))
                    .Select(x => x.Entity as InputInvoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.InputInvoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InputInvoice))
                        .Select(x => x.Entity as InputInvoice))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return "1";
            }
        }

        public InputInvoice Create(InputInvoice inputInvoice)
        {
            if (context.InputInvoices.Where(x => x.Identifier != null && x.Identifier == inputInvoice.Identifier).Count() == 0)
            {
                inputInvoice.Id = 0;

                inputInvoice.Code = GetNewCodeValue(inputInvoice.CompanyId ?? 0);
                inputInvoice.Active = true;

                inputInvoice.UpdatedAt = DateTime.Now;
                inputInvoice.CreatedAt = DateTime.Now;

                context.InputInvoices.Add(inputInvoice);
                return inputInvoice;
            }
            else
            {
                // Load favor that will be updated
                InputInvoice dbEntry = context.InputInvoices
                .FirstOrDefault(x => x.Identifier == inputInvoice.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = inputInvoice.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = inputInvoice.CompanyId ?? null;
                    dbEntry.CreatedById = inputInvoice.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = inputInvoice.Code;
                    dbEntry.Supplier = inputInvoice.Supplier;
                    dbEntry.Address = inputInvoice.Address;
                    dbEntry.InvoiceNumber = inputInvoice.InvoiceNumber;
                    dbEntry.InvoiceDate = inputInvoice.InvoiceDate;
                    dbEntry.AmountNet = inputInvoice.AmountNet;
                    dbEntry.PDVPercent = inputInvoice.PDVPercent;
                    dbEntry.PDV = inputInvoice.PDV;
                    dbEntry.AmountGross = inputInvoice.AmountGross;
                    dbEntry.Currency = inputInvoice.Currency;
                    dbEntry.DateOfPaymet = inputInvoice.DateOfPaymet;
                    dbEntry.Status = inputInvoice.Status;
                    dbEntry.StatusDate = inputInvoice.StatusDate;
                    dbEntry.Description = inputInvoice.Description;
                    dbEntry.Path = inputInvoice.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public InputInvoice Delete(Guid identifier)
        {
            // Load Favor that will be deleted
            InputInvoice dbEntry = context.InputInvoices
                .FirstOrDefault(x => x.Identifier == identifier);

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
