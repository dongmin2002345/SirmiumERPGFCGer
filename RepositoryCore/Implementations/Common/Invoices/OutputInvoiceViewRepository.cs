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

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            outputInvoice.BusinessPartner = new BusinessPartner();
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                outputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerIdentifier"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            if (reader["BusinessPartnerCode"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            if (reader["BusinessPartnerName"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != DBNull.Value)
                            outputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            outputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != DBNull.Value)
                            outputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != DBNull.Value)
                            outputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != DBNull.Value)
                            outputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PdvPercent"] != DBNull.Value)
                            outputInvoice.PdvPercent = Int32.Parse(reader["PdvPercent"].ToString());
                        if (reader["Pdv"] != DBNull.Value)
                            outputInvoice.Pdv = decimal.Parse(reader["Pdv"].ToString());
                        if (reader["AmountGross"] != DBNull.Value)
                            outputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != DBNull.Value)
                            outputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != DBNull.Value)
                            outputInvoice.DateOfPayment = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != DBNull.Value)
                            outputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != DBNull.Value)
                            outputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            outputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            outputInvoice.Description = reader["Path"].ToString();

                        outputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            outputInvoice.CreatedBy = new User();
                            if (reader["CreatedById"] != DBNull.Value)
                                outputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedById"] != DBNull.Value)
                                outputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedByFirstName"] != DBNull.Value)
                                outputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            if (reader["CreatedByLastName"] != DBNull.Value)
                                outputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            outputInvoice.Company = new Company();
                            if (reader["CompanyId"] != DBNull.Value)
                                outputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyId"] != DBNull.Value)
                                outputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyName"] != DBNull.Value)
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

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            outputInvoice.BusinessPartner = new BusinessPartner();
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                outputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerId"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            if (reader["BusinessPartnerIdentifier"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            if (reader["BusinessPartnerCode"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            if (reader["BusinessPartnerName"] != DBNull.Value)
                                outputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != DBNull.Value)
                            outputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            outputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != DBNull.Value)
                            outputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != DBNull.Value)
                            outputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != DBNull.Value)
                            outputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PdvPercent"] != DBNull.Value)
                            outputInvoice.PdvPercent = Int32.Parse(reader["PdvPercent"].ToString());
                        if (reader["Pdv"] != DBNull.Value)
                            outputInvoice.Pdv = decimal.Parse(reader["Pdv"].ToString());
                        if (reader["AmountGross"] != DBNull.Value)
                            outputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != DBNull.Value)
                            outputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != DBNull.Value)
                            outputInvoice.DateOfPayment = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != DBNull.Value)
                            outputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != DBNull.Value)
                            outputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            outputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            outputInvoice.Description = reader["Path"].ToString();

                        outputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            outputInvoice.CreatedBy = new User();
                            if (reader["CreatedById"] != DBNull.Value)
                                outputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedById"] != DBNull.Value)
                                outputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            if (reader["CreatedByFirstName"] != DBNull.Value)
                                outputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            if (reader["CreatedByLastName"] != DBNull.Value)
                                outputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            outputInvoice.Company = new Company();
                            if (reader["CompanyId"] != DBNull.Value)
                                outputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyId"] != DBNull.Value)
                                outputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            if (reader["CompanyName"] != DBNull.Value)
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
