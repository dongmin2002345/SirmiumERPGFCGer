using Configurator;
using DomainCore.Banks;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerBankViewRepository : IBusinessPartnerBankRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerBankViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanks(int companyId)
        {
            List<BusinessPartnerBank> businessPartnerBanks = new List<BusinessPartnerBank>();

            string queryString =
                "SELECT BusinessPartnerBankId, BusinessPartnerBankIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BankId, BankIdentifier, BankCode, BankName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "AccountNumber, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerBanks " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerBank businessPartnerBank;
                    while (reader.Read())
                    {
                        businessPartnerBank = new BusinessPartnerBank();
                        businessPartnerBank.Id = Int32.Parse(reader["BusinessPartnerBankId"].ToString());
                        businessPartnerBank.Identifier = Guid.Parse(reader["BusinessPartnerBankIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerBank.BusinessPartner = new BusinessPartner();
                            businessPartnerBank.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerBank.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerBank.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BankId"] != DBNull.Value)
                        {
                            businessPartnerBank.Bank = new Bank();
                            businessPartnerBank.BankId = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Id = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Identifier = Guid.Parse(reader["BankIdentifier"].ToString());
                            businessPartnerBank.Bank.Code = reader["BankCode"].ToString();
                            businessPartnerBank.Bank.Name = reader["BankName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerBank.Country = new Country();
                            businessPartnerBank.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerBank.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerBank.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["AccountNumber"] != DBNull.Value)
                            businessPartnerBank.AccountNumber = reader["AccountNumber"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerBank.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerBank.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerBank.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerBank.CreatedBy = new User();
                            businessPartnerBank.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerBank.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerBank.Company = new Company();
                            businessPartnerBank.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Name = reader["CompanyName"].ToString();
                        }

                        businessPartnerBanks.Add(businessPartnerBank);
                    }
                }
            }

            //List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Bank)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            return businessPartnerBanks;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanksByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerBank> businessPartnerBanks = new List<BusinessPartnerBank>();

            string queryString =
                "SELECT BusinessPartnerBankId, BusinessPartnerBankIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BankId, BankIdentifier, BankCode, BankName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "AccountNumber, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerBanks " +
                "WHERE BusinessPartnerId = @BusinessPartnerId;"; // AND Active = 1

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerBank businessPartnerBank;
                    while (reader.Read())
                    {
                        businessPartnerBank = new BusinessPartnerBank();
                        businessPartnerBank.Id = Int32.Parse(reader["BusinessPartnerBankId"].ToString());
                        businessPartnerBank.Identifier = Guid.Parse(reader["BusinessPartnerBankIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerBank.BusinessPartner = new BusinessPartner();
                            businessPartnerBank.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerBank.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerBank.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BankId"] != DBNull.Value)
                        {
                            businessPartnerBank.Bank = new Bank();
                            businessPartnerBank.BankId = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Id = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Identifier = Guid.Parse(reader["BankIdentifier"].ToString());
                            businessPartnerBank.Bank.Code = reader["BankCode"].ToString();
                            businessPartnerBank.Bank.Name = reader["BankName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerBank.Country = new Country();
                            businessPartnerBank.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerBank.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerBank.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["AccountNumber"] != DBNull.Value)
                            businessPartnerBank.AccountNumber = reader["AccountNumber"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerBank.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerBank.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerBank.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerBank.CreatedBy = new User();
                            businessPartnerBank.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerBank.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerBank.Company = new Company();
                            businessPartnerBank.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Name = reader["CompanyName"].ToString();
                        }

                        businessPartnerBanks.Add(businessPartnerBank);
                    }
                }
            }

            //List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Bank)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            return businessPartnerBanks;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanksNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerBank> businessPartnerBanks = new List<BusinessPartnerBank>();

            string queryString =
                "SELECT BusinessPartnerBankId, BusinessPartnerBankIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BankId, BankIdentifier, BankCode, BankName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "AccountNumber, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerBanks " +
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
                    BusinessPartnerBank businessPartnerBank;
                    while (reader.Read())
                    {
                        businessPartnerBank = new BusinessPartnerBank();
                        businessPartnerBank.Id = Int32.Parse(reader["BusinessPartnerBankId"].ToString());
                        businessPartnerBank.Identifier = Guid.Parse(reader["BusinessPartnerBankIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerBank.BusinessPartner = new BusinessPartner();
                            businessPartnerBank.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerBank.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerBank.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerBank.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BankId"] != DBNull.Value)
                        {
                            businessPartnerBank.Bank = new Bank();
                            businessPartnerBank.BankId = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Id = Int32.Parse(reader["BankId"].ToString());
                            businessPartnerBank.Bank.Identifier = Guid.Parse(reader["BankIdentifier"].ToString());
                            businessPartnerBank.Bank.Code = reader["BankCode"].ToString();
                            businessPartnerBank.Bank.Name = reader["BankName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerBank.Country = new Country();
                            businessPartnerBank.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerBank.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerBank.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerBank.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["AccountNumber"] != DBNull.Value)
                            businessPartnerBank.AccountNumber = reader["AccountNumber"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerBank.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerBank.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerBank.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerBank.CreatedBy = new User();
                            businessPartnerBank.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerBank.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerBank.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerBank.Company = new Company();
                            businessPartnerBank.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerBank.Company.Name = reader["CompanyName"].ToString();
                        }

                        businessPartnerBanks.Add(businessPartnerBank);
                    }
                }
            }

            //List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Bank)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            return businessPartnerBanks;
        }
                
        public BusinessPartnerBank Create(BusinessPartnerBank businessPartnerBank)
        {
            if (context.BusinessPartnerBanks.Where(x => x.Identifier != null && x.Identifier == businessPartnerBank.Identifier).Count() == 0)
            {
                businessPartnerBank.Id = 0;

                businessPartnerBank.Active = true;
                businessPartnerBank.UpdatedAt = DateTime.Now;
                businessPartnerBank.CreatedAt = DateTime.Now;
                context.BusinessPartnerBanks.Add(businessPartnerBank);
                return businessPartnerBank;
            }
            else
            {
                // Load businessPartnerBank that will be updated
                BusinessPartnerBank dbEntry = context.BusinessPartnerBanks
                    .FirstOrDefault(x => x.Identifier == businessPartnerBank.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerBank.BusinessPartnerId ?? null;
                    dbEntry.BankId = businessPartnerBank.BankId ?? null;
                    dbEntry.CountryId = businessPartnerBank.CountryId ?? null;
                    dbEntry.CompanyId = businessPartnerBank.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerBank.CreatedById ?? null;

                    // Set properties
                    dbEntry.AccountNumber = businessPartnerBank.AccountNumber;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerBank Delete(Guid identifier)
        {
            BusinessPartnerBank dbEntry = context.BusinessPartnerBanks
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerBank))
                    .Select(x => x.Entity as BusinessPartnerBank))
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
