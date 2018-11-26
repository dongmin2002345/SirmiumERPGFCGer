﻿using Configurator;
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
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName," +
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

                        if (reader["BusinessPartnerId"] != null)
                        {
                            inputInvoice.BusinessPartner = new BusinessPartner();
                            inputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            inputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            inputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerCode"].ToString());
                            inputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            inputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != null)
                            inputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != null)
                            inputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != null)
                            inputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != null)
                            inputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != null)
                            inputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PDVPercent"] != null)
                            inputInvoice.PDVPercent = Int32.Parse(reader["PDVPercent"].ToString());
                        if (reader["PDV"] != null)
                            inputInvoice.PDV = decimal.Parse(reader["PDV"].ToString());
                        if (reader["AmountGross"] != null)
                            inputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != null)
                            inputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != null)
                            inputInvoice.DateOfPaymet = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != null)
                            inputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != null)
                            inputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != null)
                            inputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != null)
                            inputInvoice.Description = reader["Path"].ToString();

                        inputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        inputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if(reader["CreatedById"] != null)
                        {
                            inputInvoice.CreatedBy = new User();
                            inputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            inputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            inputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            inputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if(reader["CompanyId"] != null)
                        {
                            inputInvoice.Company = new Company();
                            inputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            inputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
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
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName," +
                "FROM vInputInvoices " +
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
                    InputInvoice inputInvoice;
                    while (reader.Read())
                    {
                        inputInvoice = new InputInvoice();
                        inputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
                        inputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
                        inputInvoice.Code = reader["InputInvoiceCode"]?.ToString();

                        if (reader["BusinessPartnerId"] != null)
                        {
                            inputInvoice.BusinessPartner = new BusinessPartner();
                            inputInvoice.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            inputInvoice.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            inputInvoice.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerCode"].ToString());
                            inputInvoice.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            inputInvoice.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Supplier"] != null)
                            inputInvoice.Supplier = reader["Supplier"].ToString();
                        if (reader["Address"] != null)
                            inputInvoice.Address = reader["Address"].ToString();
                        if (reader["InvoiceNumber"] != null)
                            inputInvoice.InvoiceNumber = reader["InvoiceNumber"].ToString();
                        if (reader["InvoiceDate"] != null)
                            inputInvoice.InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString());
                        if (reader["AmountNet"] != null)
                            inputInvoice.AmountNet = decimal.Parse(reader["AmountNet"].ToString());
                        if (reader["PDVPercent"] != null)
                            inputInvoice.PDVPercent = Int32.Parse(reader["PDVPercent"].ToString());
                        if (reader["PDV"] != null)
                            inputInvoice.PDV = decimal.Parse(reader["PDV"].ToString());
                        if (reader["AmountGross"] != null)
                            inputInvoice.AmountGross = decimal.Parse(reader["AmountGross"].ToString());
                        if (reader["Currency"] != null)
                            inputInvoice.AmountGross = Int32.Parse(reader["Currency"].ToString());
                        if (reader["DateOfPayment"] != null)
                            inputInvoice.DateOfPaymet = DateTime.Parse(reader["DateOfPayment"].ToString());
                        if (reader["Status"] != null)
                            inputInvoice.Status = reader["Status"].ToString();
                        if (reader["StatusDate"] != null)
                            inputInvoice.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Description"] != null)
                            inputInvoice.Description = reader["Description"].ToString();
                        if (reader["Path"] != null)
                            inputInvoice.Description = reader["Path"].ToString();

                        inputInvoice.Active = bool.Parse(reader["Active"].ToString());
                        inputInvoice.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            inputInvoice.CreatedBy = new User();
                            inputInvoice.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            inputInvoice.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            inputInvoice.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            inputInvoice.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            inputInvoice.Company = new Company();
                            inputInvoice.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            inputInvoice.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
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
                    return "";
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