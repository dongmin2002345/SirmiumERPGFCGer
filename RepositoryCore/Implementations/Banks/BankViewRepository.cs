using Configurator;
using DomainCore.Banks;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Banks
{
    public class BankViewRepository : IBankRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BankViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Bank> GetBanks(int companyId)
        {
            List<Bank> banks = new List<Bank>();

            string queryString =
                "SELECT BankId, BankIdentifier, BankCode, BankName, Swift, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBanks " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Bank bank;
                    while (reader.Read())
                    {
                        bank = new Bank();
                        bank.Id = Int32.Parse(reader["BankId"].ToString());
                        bank.Identifier = Guid.Parse(reader["BankIdentifier"].ToString());
                        bank.Code = reader["BankCode"]?.ToString();
                        bank.Name = reader["BankName"].ToString();

                        if (reader["Swift"] != null)
                            bank.Swift = reader["Swift"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            bank.Country = new Country();
                            bank.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            bank.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            bank.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            bank.Country.Code = reader["CountryCode"].ToString();
                            bank.Country.Name = reader["CountryName"].ToString();
                        }

                        bank.Active = bool.Parse(reader["Active"].ToString());
                        bank.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            bank.CreatedBy = new User();
                            bank.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            bank.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            bank.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            bank.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            bank.Company = new Company();
                            bank.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            bank.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            bank.Company.Name = reader["CompanyName"].ToString();
                        }

                        banks.Add(bank);
                    }
                }
            }

            //List<Bank> banks = context.Banks
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return banks;
        }

        public List<Bank> GetBanksNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Bank> banks = new List<Bank>();

            string queryString =
                "SELECT BankId, BankIdentifier, BankCode, BankName, Swift, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBanks " +
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
                    Bank bank;
                    while (reader.Read())
                    {
                        bank = new Bank();
                        bank.Id = Int32.Parse(reader["BankId"].ToString());
                        bank.Identifier = Guid.Parse(reader["BankIdentifier"].ToString());
                        bank.Code = reader["BankCode"]?.ToString();
                        bank.Name = reader["BankName"].ToString();

                        if (reader["Swift"] != null)
                            bank.Swift = reader["Swift"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            bank.Country = new Country();
                            bank.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            bank.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            bank.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            bank.Country.Code = reader["CountryCode"].ToString();
                            bank.Country.Name = reader["CountryName"].ToString();
                        }

                        bank.Active = bool.Parse(reader["Active"].ToString());
                        bank.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            bank.CreatedBy = new User();
                            bank.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            bank.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            bank.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            bank.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            bank.Company = new Company();
                            bank.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            bank.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            bank.Company.Name = reader["CompanyName"].ToString();
                        }

                        banks.Add(bank);
                    }
                }
            }

            //List<Bank> banks = context.Banks
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return banks;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Banks
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Bank))
                    .Select(x => x.Entity as Bank))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "BANK-00001";
            else
            {
                string activeCode = context.Banks
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Bank))
                        .Select(x => x.Entity as Bank))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("BANK-", ""));
                    return "BANK-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Bank Create(Bank bank)
        {
            if (context.Banks.Where(x => x.Identifier != null && x.Identifier == bank.Identifier).Count() == 0)
            {
                bank.Id = 0;

                bank.Code = GetNewCodeValue(bank.CompanyId ?? 0);
                bank.Active = true;

                bank.UpdatedAt = DateTime.Now;
                bank.CreatedAt = DateTime.Now;

                context.Banks.Add(bank);
                return bank;
            }
            else
            {
                // Load Sector that will be updated
                Bank dbEntry = context.Banks
                .FirstOrDefault(x => x.Identifier == bank.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = bank.CountryId ?? null;
                    dbEntry.CompanyId = bank.CompanyId ?? null;
                    dbEntry.CreatedById = bank.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = bank.Code;
                    dbEntry.Name = bank.Name;
                    dbEntry.Swift = bank.Swift;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Bank Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            Bank dbEntry = context.Banks
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
