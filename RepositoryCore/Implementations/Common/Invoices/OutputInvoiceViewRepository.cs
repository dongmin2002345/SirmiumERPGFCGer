using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.OutputInvoices;
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
    public class OutputInvoiceViewRepository : IOutputInvoiceRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public OutputInvoiceViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<OutputInvoice> GetOutputInvoices(int companyId)
        {
            List<OutputInvoice> OutputInvoices = new List<OutputInvoice>();

            string queryString =
                "SELECT OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vOutputInvoices " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    OutputInvoice outputInvoice;
                    while (reader.Read())
                    {
                        outputInvoice = new OutputInvoice();
                        outputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
                        outputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
                        outputInvoice.Code = reader["OutputInvoiceCode"]?.ToString();

                        if (reader["BusinessPartnerId"] != null)
                        {
                            outputInvoice.BusinessPartner = new BusinessPartner();
                            outputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            outputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            outputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            outputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            outputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != null)
                            outputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != null)
                            outputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != null)
                            outputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != null)
                            outputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != null)
                            outputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PdvPercent"] != null)
                            outputInvoice.PdvPercent = Int32.Parse(reader["PdvPercent"].ToString());
                        if (reader["Pdv"] != null)
                            outputInvoice.Pdv = decimal.Parse(reader["Pdv"].ToString());
                        if (reader["AmountGross"] != null)
                            outputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != null)
                            outputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != null)
                            outputInvoice.DateOfPayment = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != null)
                            outputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != null)
                            outputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != null)
                            outputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != null)
                            outputInvoice.Description = reader["Path"].ToString();

                        outputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            outputInvoice.CreatedBy = new User();
                            outputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            outputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            outputInvoice.Company = new Company();
                            outputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoice.Company.Name = reader["CompanyName"].ToString();
                        }

                        OutputInvoices.Add(outputInvoice);

                    }
                }
            }

            //List<OutputInvoice> OutputInvoices = context.OutputInvoices
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return OutputInvoices;
        }

        public List<OutputInvoice> GetOutputInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<OutputInvoice> OutputInvoices = new List<OutputInvoice>();

            string queryString =
                "SELECT OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vOutputInvoices " +
                "WHERE CompanyId = @CompanyId AND UpdatedAt > @LastUpdateTime;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    OutputInvoice outputInvoice;
                    while (reader.Read())
                    {
                        outputInvoice = new OutputInvoice();
                        outputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
                        outputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
                        outputInvoice.Code = reader["OutputInvoiceCode"]?.ToString();

                        if (reader["BusinessPartnerId"] != null)
                        {
                            outputInvoice.BusinessPartner = new BusinessPartner();
                            outputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            outputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            outputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerCode"].ToString());
                            outputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            outputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != null)
                            outputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != null)
                            outputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != null)
                            outputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != null)
                            outputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != null)
                            outputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PdvPercent"] != null)
                            outputInvoice.PdvPercent = Int32.Parse(reader["PdvPercent"].ToString());
                        if (reader["Pdv"] != null)
                            outputInvoice.Pdv = decimal.Parse(reader["Pdv"].ToString());
                        if (reader["AmountGross"] != null)
                            outputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != null)
                            outputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != null)
                            outputInvoice.DateOfPayment = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != null)
                            outputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != null)
                            outputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != null)
                            outputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != null)
                            outputInvoice.Description = reader["Path"].ToString();

                        outputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            outputInvoice.CreatedBy = new User();
                            outputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            outputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            outputInvoice.Company = new Company();
                            outputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoice.Company.Name = reader["CompanyName"].ToString();
                        }

                        OutputInvoices.Add(outputInvoice);

                    }
                }
            }

            //List<OutputInvoice> OutputInvoices = context.OutputInvoices
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return OutputInvoices;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.OutputInvoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                    .Select(x => x.Entity as OutputInvoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.OutputInvoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                        .Select(x => x.Entity as OutputInvoice))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return "";
            }
        }

        public OutputInvoice Create(OutputInvoice outputInvoice)
        {
            if (context.OutputInvoices.Where(x => x.Identifier != null && x.Identifier == outputInvoice.Identifier).Count() == 0)
            {
                outputInvoice.Id = 0;

                outputInvoice.Code = GetNewCodeValue(outputInvoice.CompanyId ?? 0);
                outputInvoice.Active = true;

                outputInvoice.UpdatedAt = DateTime.Now;
                outputInvoice.CreatedAt = DateTime.Now;

                context.OutputInvoices.Add(outputInvoice);
                return outputInvoice;
            }
            else
            {
                // Load favor that will be updated
                OutputInvoice dbEntry = context.OutputInvoices
                .FirstOrDefault(x => x.Identifier == outputInvoice.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = outputInvoice.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = outputInvoice.CompanyId ?? null;
                    dbEntry.CreatedById = outputInvoice.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = outputInvoice.Code;
                    dbEntry.Supplier = outputInvoice.Supplier;
                    dbEntry.Address = outputInvoice.Address;
                    dbEntry.InvoiceNumber = outputInvoice.InvoiceNumber;
                    dbEntry.InvoiceDate = outputInvoice.InvoiceDate;
                    dbEntry.AmountNet = outputInvoice.AmountNet;
                    dbEntry.PdvPercent = outputInvoice.PdvPercent;
                    dbEntry.Pdv = outputInvoice.Pdv;
                    dbEntry.AmountGross = outputInvoice.AmountGross;
                    dbEntry.Currency = outputInvoice.Currency;
                    dbEntry.DateOfPayment = outputInvoice.DateOfPayment;
                    dbEntry.Status = outputInvoice.Status;
                    dbEntry.StatusDate = outputInvoice.StatusDate;
                    dbEntry.Description = outputInvoice.Description;
                    dbEntry.Path = outputInvoice.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public OutputInvoice Delete(Guid identifier)
        {
            // Load Favor that will be deleted
            OutputInvoice dbEntry = context.OutputInvoices
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
