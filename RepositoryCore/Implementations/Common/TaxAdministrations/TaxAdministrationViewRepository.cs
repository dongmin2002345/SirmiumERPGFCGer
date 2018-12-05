using Configurator;
using DomainCore.Banks;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.TaxAdministrations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.TaxAdministrations;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.TaxAdministrations
{
    public class TaxAdministrationViewRepository : ITaxAdministrationRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public TaxAdministrationViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<TaxAdministration> GetTaxAdministrations(int companyId)
        {
            List<TaxAdministration> TaxAdministrations = new List<TaxAdministration>();

            string queryString =
                "SELECT TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationSecondCode, TaxAdministrationName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address1, Address2, Address3, " +
                "BankId1, BankIdentifier1, BankCode1, BankName1, " +
                "BankId2, BankIdentifier2, BankCode2, BankName2, " +
                "IBAN1, SWIFT, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vTaxAdministrations " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    TaxAdministration taxAdministration;
                    while (reader.Read())
                    {
                        taxAdministration = new TaxAdministration();
                        taxAdministration.Id = Int32.Parse(reader["TaxAdministrationId"].ToString());
                        taxAdministration.Identifier = Guid.Parse(reader["TaxAdministrationIdentifier"].ToString());
                        taxAdministration.Code = reader["TaxAdministrationCode"]?.ToString();
                        taxAdministration.SecondCode = reader["TaxAdministrationSecondCode"]?.ToString();
                        taxAdministration.Name = reader["TaxAdministrationName"].ToString();

                        if (reader["CityId"] != DBNull.Value)
                        {
                            taxAdministration.City = new City();
                            taxAdministration.CityId = Int32.Parse(reader["CityId"].ToString());
                            taxAdministration.City.Id = Int32.Parse(reader["CityId"].ToString());
                            taxAdministration.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            taxAdministration.City.Code = reader["CityCode"].ToString();
                            taxAdministration.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["Address1"] != DBNull.Value)
                            taxAdministration.Address1 = reader["Address1"].ToString();
                        if (reader["Address2"] != DBNull.Value)
                            taxAdministration.Address2 = reader["Address2"].ToString();
                        if (reader["Address3"] != DBNull.Value)
                            taxAdministration.Address3 = reader["Address3"].ToString();

                        if (reader["BankId1"] != DBNull.Value)
                        {
                            taxAdministration.Bank1 = new Bank();
                            taxAdministration.BankId1 = Int32.Parse(reader["BankId1"].ToString());
                            taxAdministration.Bank1.Id = Int32.Parse(reader["BankId1"].ToString());
                            taxAdministration.Bank1.Identifier = Guid.Parse(reader["BankIdentifier1"].ToString());
                            taxAdministration.Bank1.Code = reader["BankCode1"].ToString();
                            taxAdministration.Bank1.Name = reader["BankName1"].ToString();
                        }

                        if (reader["BankId2"] != DBNull.Value)
                        {
                            taxAdministration.Bank2 = new Bank();
                            taxAdministration.BankId2 = Int32.Parse(reader["BankId2"].ToString());
                            taxAdministration.Bank2.Id = Int32.Parse(reader["BankId2"].ToString());
                            taxAdministration.Bank2.Identifier = Guid.Parse(reader["BankIdentifier2"].ToString());
                            taxAdministration.Bank2.Code = reader["BankCode2"].ToString();
                            taxAdministration.Bank2.Name = reader["BankName2"].ToString();
                        }

                        if (reader["IBAN1"] != DBNull.Value)
                            taxAdministration.IBAN1 = reader["IBAN1"].ToString();
                        if (reader["SWIFT"] != DBNull.Value)
                            taxAdministration.SWIFT = reader["SWIFT"].ToString();

                        taxAdministration.Active = bool.Parse(reader["Active"].ToString());
                        taxAdministration.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            taxAdministration.CreatedBy = new User();
                            taxAdministration.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            taxAdministration.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            taxAdministration.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            taxAdministration.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            taxAdministration.Company = new Company();
                            taxAdministration.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            taxAdministration.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            taxAdministration.Company.Name = reader["CompanyName"].ToString();
                        }

                        TaxAdministrations.Add(taxAdministration);
                    }
                }
            }
            return TaxAdministrations;

            //List<TaxAdministration> TaxAdministrations = context.TaxAdministrations
            //    .Include(x => x.City)
            //    .Include(x => x.Bank1)
            //    .Include(x => x.Bank2)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return TaxAdministrations;
        }

        public List<TaxAdministration> GetTaxAdministrationsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<TaxAdministration> TaxAdministrations = new List<TaxAdministration>();

            string queryString =
                "SELECT TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationSecondCode, TaxAdministrationName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address1, Address2, Address3, " +
                "BankId1, BankIdentifier1, BankCode1, BankName1, " +
                "BankId2, BankIdentifier2, BankCode2, BankName2, " +
                "IBAN1, SWIFT, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vTaxAdministrations " +
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
                    TaxAdministration taxAdministration;
                    while (reader.Read())
                    {
                        taxAdministration = new TaxAdministration();
                        taxAdministration.Id = Int32.Parse(reader["TaxAdministrationId"].ToString());
                        taxAdministration.Identifier = Guid.Parse(reader["TaxAdministrationIdentifier"].ToString());
                        taxAdministration.Code = reader["TaxAdministrationCode"]?.ToString();
                        taxAdministration.SecondCode = reader["TaxAdministrationSecondCode"]?.ToString();
                        taxAdministration.Name = reader["TaxAdministrationName"].ToString();

                        if (reader["CityId"] != DBNull.Value)
                        {
                            taxAdministration.City = new City();
                            taxAdministration.CityId = Int32.Parse(reader["CityId"].ToString());
                            taxAdministration.City.Id = Int32.Parse(reader["CityId"].ToString());
                            taxAdministration.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            taxAdministration.City.Code = reader["CityCode"].ToString();
                            taxAdministration.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["Address1"] != DBNull.Value)
                            taxAdministration.Address1 = reader["Address1"].ToString();
                        if (reader["Address2"] != DBNull.Value)
                            taxAdministration.Address2 = reader["Address2"].ToString();
                        if (reader["Address3"] != DBNull.Value)
                            taxAdministration.Address3 = reader["Address3"].ToString();

                        if (reader["BankId1"] != DBNull.Value)
                        {
                            taxAdministration.Bank1 = new Bank();
                            taxAdministration.BankId1 = Int32.Parse(reader["BankId1"].ToString());
                            taxAdministration.Bank1.Id = Int32.Parse(reader["BankId1"].ToString());
                            taxAdministration.Bank1.Identifier = Guid.Parse(reader["BankIdentifier1"].ToString());
                            taxAdministration.Bank1.Code = reader["BankCode1"].ToString();
                            taxAdministration.Bank1.Name = reader["BankName1"].ToString();
                        }

                        if (reader["BankId2"] != DBNull.Value)
                        {
                            taxAdministration.Bank2 = new Bank();
                            taxAdministration.BankId2 = Int32.Parse(reader["BankId2"].ToString());
                            taxAdministration.Bank2.Id = Int32.Parse(reader["BankId2"].ToString());
                            taxAdministration.Bank2.Identifier = Guid.Parse(reader["BankIdentifier2"].ToString());
                            taxAdministration.Bank2.Code = reader["BankCode2"].ToString();
                            taxAdministration.Bank2.Name = reader["BankName2"].ToString();
                        }

                        if (reader["IBAN1"] != DBNull.Value)
                            taxAdministration.IBAN1 = reader["IBAN1"].ToString();
                        if (reader["SWIFT"] != DBNull.Value)
                            taxAdministration.SWIFT = reader["SWIFT"].ToString();

                        taxAdministration.Active = bool.Parse(reader["Active"].ToString());
                        taxAdministration.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            taxAdministration.CreatedBy = new User();
                            taxAdministration.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            taxAdministration.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            taxAdministration.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            taxAdministration.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            taxAdministration.Company = new Company();
                            taxAdministration.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            taxAdministration.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            taxAdministration.Company.Name = reader["CompanyName"].ToString();
                        }

                        TaxAdministrations.Add(taxAdministration);
                    }
                }
            }
            return TaxAdministrations;

            //List<TaxAdministration> TaxAdministrations = context.TaxAdministrations
            //    .Include(x => x.City)
            //    .Include(x => x.Bank1)
            //    .Include(x => x.Bank2)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return TaxAdministrations;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.TaxAdministrations
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(TaxAdministration))
                    .Select(x => x.Entity as TaxAdministration))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.TaxAdministrations
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(TaxAdministration))
                        .Select(x => x.Entity as TaxAdministration))
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

        public TaxAdministration Create(TaxAdministration taxAdministration)
        {
            if (context.OutputInvoices.Where(x => x.Identifier != null && x.Identifier == taxAdministration.Identifier).Count() == 0)
            {
                taxAdministration.Id = 0;

                taxAdministration.Code = GetNewCodeValue(taxAdministration.CompanyId ?? 0);
                taxAdministration.Active = true;

                taxAdministration.UpdatedAt = DateTime.Now;
                taxAdministration.CreatedAt = DateTime.Now;

                context.TaxAdministrations.Add(taxAdministration);
                return taxAdministration;
            }
            else
            {
                // Load taxAdministration that will be updated
                TaxAdministration dbEntry = context.TaxAdministrations
                .FirstOrDefault(x => x.Identifier == taxAdministration.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CityId = taxAdministration.CityId ?? null;
                    dbEntry.BankId1 = taxAdministration.BankId1 ?? null;
                    dbEntry.BankId2 = taxAdministration.BankId2 ?? null;
                    dbEntry.CompanyId = taxAdministration.CompanyId ?? null;
                    dbEntry.CreatedById = taxAdministration.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = taxAdministration.Code;
                    dbEntry.Name = taxAdministration.Name;
                    dbEntry.Address1 = taxAdministration.Address1;
                    dbEntry.Address2 = taxAdministration.Address2;
                    dbEntry.Address3 = taxAdministration.Address3;
                    dbEntry.IBAN1 = taxAdministration.IBAN1;
                    dbEntry.SWIFT = taxAdministration.SWIFT;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public TaxAdministration Delete(Guid identifier)
        {
            // Load TaxAdministration that will be deleted
            TaxAdministration dbEntry = context.TaxAdministrations
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
