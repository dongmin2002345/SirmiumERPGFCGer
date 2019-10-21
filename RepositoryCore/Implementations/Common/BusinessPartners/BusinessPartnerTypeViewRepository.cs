using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
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
    public class BusinessPartnerTypeViewRepository : IBusinessPartnerTypeRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerTypeViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerType> GetBusinessPartnerTypes(int companyId)
        {
            List<BusinessPartnerType> BusinessPartnerTypes = new List<BusinessPartnerType>();

            string queryString =
                "SELECT BusinessPartnerTypeId, BusinessPartnerTypeIdentifier, BusinessPartnerTypeCode, BusinessPartnerTypeName, " +
                "IsBuyer, IsSupplier, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerTypes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerType businessPartnerType;
                    while (reader.Read())
                    {
                        businessPartnerType = new BusinessPartnerType();
                        businessPartnerType.Id = Int32.Parse(reader["BusinessPartnerTypeId"].ToString());
                        businessPartnerType.Identifier = Guid.Parse(reader["BusinessPartnerTypeIdentifier"].ToString());
                        businessPartnerType.Code = reader["BusinessPartnerTypeCode"].ToString();
                        businessPartnerType.Name = reader["BusinessPartnerTypeName"].ToString();
                        businessPartnerType.IsBuyer = bool.Parse(reader["IsBuyer"]?.ToString());
                        businessPartnerType.IsBuyer = bool.Parse(reader["IsSupplier"]?.ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerType.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerType.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerType.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerType.CreatedBy = new User();
                            businessPartnerType.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerType.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerType.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerType.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerType.Company = new Company();
                            businessPartnerType.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerType.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerType.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerTypes.Add(businessPartnerType);
                    }
                }
            }
            return BusinessPartnerTypes;

            //List<BusinessPartnerType> BusinessPartnerTypes = context.BusinessPartnerTypes
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerTypes;
        }

        public List<BusinessPartnerType> GetBusinessPartnerTypesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerType> BusinessPartnerTypes = new List<BusinessPartnerType>();

            string queryString =
                "SELECT BusinessPartnerTypeId, BusinessPartnerTypeIdentifier, BusinessPartnerTypeCode, BusinessPartnerTypeName, " +
                "IsBuyer, IsSupplier, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerTypes " +
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
                    BusinessPartnerType businessPartnerType;
                    while (reader.Read())
                    {
                        businessPartnerType = new BusinessPartnerType();
                        businessPartnerType.Id = Int32.Parse(reader["BusinessPartnerTypeId"].ToString());
                        businessPartnerType.Identifier = Guid.Parse(reader["BusinessPartnerTypeIdentifier"].ToString());
                        businessPartnerType.Code = reader["BusinessPartnerTypeCode"].ToString();
                        businessPartnerType.Name = reader["BusinessPartnerTypeName"].ToString();
                        businessPartnerType.IsBuyer = bool.Parse(reader["IsBuyer"]?.ToString());
                        businessPartnerType.IsSupplier = bool.Parse(reader["IsSupplier"]?.ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerType.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerType.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerType.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerType.CreatedBy = new User();
                            businessPartnerType.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerType.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerType.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerType.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerType.Company = new Company();
                            businessPartnerType.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerType.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerType.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerTypes.Add(businessPartnerType);
                    }
                }
            }
            return BusinessPartnerTypes;

            //List<BusinessPartnerType> BusinessPartnerTypes = context.BusinessPartnerTypes
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerTypes;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartnerTypes
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerType))
                    .Select(x => x.Entity as BusinessPartnerType))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "KOR-TIP-00001";
            else
            {
                string activeCode = context.BusinessPartnerTypes
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerType))
                        .Select(x => x.Entity as BusinessPartnerType))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("KOR-TIP-", ""));
                    return "KOR-TIP-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public BusinessPartnerType Create(BusinessPartnerType businessPartnerType)
        {
            if (context.BusinessPartnerTypes.Where(x => x.Identifier != null && x.Identifier == businessPartnerType.Identifier).Count() == 0)
            {
                businessPartnerType.Id = 0;

                businessPartnerType.Code = GetNewCodeValue(businessPartnerType.CompanyId ?? 0);
                businessPartnerType.Active = true;

                businessPartnerType.UpdatedAt = DateTime.Now;
                businessPartnerType.CreatedAt = DateTime.Now;

                context.BusinessPartnerTypes.Add(businessPartnerType);
                return businessPartnerType;
            }
            else
            {
                // Load businessPartnerType that will be updated
                BusinessPartnerType dbEntry = context.BusinessPartnerTypes
                .FirstOrDefault(x => x.Identifier == businessPartnerType.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = businessPartnerType.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerType.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerType.Code;
                    dbEntry.Name = businessPartnerType.Name;
                    dbEntry.IsBuyer = businessPartnerType.IsBuyer;
                    dbEntry.IsSupplier = businessPartnerType.IsSupplier;
                    dbEntry.ItemStatus = businessPartnerType.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerType Delete(Guid identifier)
        {
            // Load BusinessPartnerType that will be deleted
            BusinessPartnerType dbEntry = context.BusinessPartnerTypes
               .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerType))
                    .Select(x => x.Entity as BusinessPartnerType))
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
