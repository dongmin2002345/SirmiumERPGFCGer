using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
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
    public class BusinessPartnerByConstructionSiteViewRepository : IBusinessPartnerByConstructionSiteRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerByConstructionSiteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSites(int companyId)
        {
            List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSite>();

            string queryString =
                "SELECT BusinessPartnerByConstructionSiteId, BusinessPartnerByConstructionSiteIdentifier, BusinessPartnerByConstructionSiteCode, StartDate, EndDate, RealEndDate, MaxNumOfEmployees, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BusinessPartnerCount, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerByConstructionSites " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerByConstructionSite businessPartnerByConstructionSite;
                    while (reader.Read())
                    {
                        businessPartnerByConstructionSite = new BusinessPartnerByConstructionSite();
                        businessPartnerByConstructionSite.Id = Int32.Parse(reader["BusinessPartnerByConstructionSiteId"].ToString());
                        businessPartnerByConstructionSite.Identifier = Guid.Parse(reader["BusinessPartnerByConstructionSiteIdentifier"].ToString());
                        businessPartnerByConstructionSite.Code = reader["BusinessPartnerByConstructionSiteCode"].ToString();
                        businessPartnerByConstructionSite.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        businessPartnerByConstructionSite.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        businessPartnerByConstructionSite.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());
                        businessPartnerByConstructionSite.MaxNumOfEmployees = Int32.Parse(reader["MaxNumOfEmployees"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerByConstructionSite.BusinessPartner = new BusinessPartner();
                            businessPartnerByConstructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Identifier = Guid.Parse(reader["FoodTypeIdentifier"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Code = reader["FoodTypeCode"].ToString();
                            businessPartnerByConstructionSite.BusinessPartner.Name = reader["FoodTypeName"].ToString();
                        }

                        if (reader["BusinessPartnerCount"] != null)
                            businessPartnerByConstructionSite.BusinessPartnerCount = Int32.Parse(reader["BusinessPartnerCount"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            businessPartnerByConstructionSite.ConstructionSite = new ConstructionSite();
                            businessPartnerByConstructionSite.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            businessPartnerByConstructionSite.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        businessPartnerByConstructionSite.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerByConstructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerByConstructionSite.CreatedBy = new User();
                            businessPartnerByConstructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerByConstructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerByConstructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerByConstructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerByConstructionSite.Company = new Company();
                            businessPartnerByConstructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerByConstructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerByConstructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerByConstructionSites.Add(businessPartnerByConstructionSite);
                    }
                }
            }
            return BusinessPartnerByConstructionSites;

            //List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = context.BusinessPartnerByConstructionSites
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerByConstructionSites;
        }

        public List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSite>();

            string queryString =
                "SELECT BusinessPartnerByConstructionSiteId, BusinessPartnerByConstructionSiteIdentifier, BusinessPartnerByConstructionSiteCode, StartDate, EndDate, RealEndDate, MaxNumOfEmployees, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BusinessPartnerCount, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerByConstructionSites " +
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
                    BusinessPartnerByConstructionSite businessPartnerByConstructionSite;
                    while (reader.Read())
                    {
                        businessPartnerByConstructionSite = new BusinessPartnerByConstructionSite();
                        businessPartnerByConstructionSite.Id = Int32.Parse(reader["BusinessPartnerByConstructionSiteId"].ToString());
                        businessPartnerByConstructionSite.Identifier = Guid.Parse(reader["BusinessPartnerByConstructionSiteIdentifier"].ToString());
                        businessPartnerByConstructionSite.Code = reader["BusinessPartnerByConstructionSiteCode"].ToString();
                        businessPartnerByConstructionSite.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        businessPartnerByConstructionSite.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        businessPartnerByConstructionSite.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());
                        businessPartnerByConstructionSite.MaxNumOfEmployees = Int32.Parse(reader["MaxNumOfEmployees"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerByConstructionSite.BusinessPartner = new BusinessPartner();
                            businessPartnerByConstructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Identifier = Guid.Parse(reader["FoodTypeIdentifier"].ToString());
                            businessPartnerByConstructionSite.BusinessPartner.Code = reader["FoodTypeCode"].ToString();
                            businessPartnerByConstructionSite.BusinessPartner.Name = reader["FoodTypeName"].ToString();
                        }

                        if (reader["BusinessPartnerCount"] != null)
                            businessPartnerByConstructionSite.BusinessPartnerCount = Int32.Parse(reader["BusinessPartnerCount"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            businessPartnerByConstructionSite.ConstructionSite = new ConstructionSite();
                            businessPartnerByConstructionSite.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            businessPartnerByConstructionSite.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            businessPartnerByConstructionSite.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        businessPartnerByConstructionSite.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerByConstructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerByConstructionSite.CreatedBy = new User();
                            businessPartnerByConstructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerByConstructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerByConstructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerByConstructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerByConstructionSite.Company = new Company();
                            businessPartnerByConstructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerByConstructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerByConstructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerByConstructionSites.Add(businessPartnerByConstructionSite);
                    }
                }
            }
            return BusinessPartnerByConstructionSites;

            //List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = context.BusinessPartnerByConstructionSites
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerByConstructionSites;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartnerByConstructionSites
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSite))
                    .Select(x => x.Entity as BusinessPartnerByConstructionSite))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "BP-BY-CS-00001";
            else
            {
                string activeCode = context.BusinessPartnerByConstructionSites
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSite))
                        .Select(x => x.Entity as BusinessPartnerByConstructionSite))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("BP-BY-CS-", ""));
                    return "BP-BY-CS-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public BusinessPartnerByConstructionSite Create(BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            if (context.BusinessPartnerByConstructionSites.Where(x => x.Identifier != null && x.Identifier == businessPartnerByConstructionSite.Identifier).Count() == 0)
            {
                businessPartnerByConstructionSite.Id = 0;

                businessPartnerByConstructionSite.Code = GetNewCodeValue(businessPartnerByConstructionSite.CompanyId ?? 0);
                businessPartnerByConstructionSite.Active = true;

                businessPartnerByConstructionSite.UpdatedAt = DateTime.Now;
                businessPartnerByConstructionSite.CreatedAt = DateTime.Now;

                context.BusinessPartnerByConstructionSites.Add(businessPartnerByConstructionSite);
                return businessPartnerByConstructionSite;
            }
            else
            {
                // Load businessPartnerByConstructionSite that will be updated
                BusinessPartnerByConstructionSite dbEntry = context.BusinessPartnerByConstructionSites
                .FirstOrDefault(x => x.Identifier == businessPartnerByConstructionSite.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerByConstructionSite.BusinessPartnerId ?? null;
                    dbEntry.ConstructionSiteId = businessPartnerByConstructionSite.ConstructionSiteId ?? null;
                    dbEntry.CompanyId = businessPartnerByConstructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerByConstructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerByConstructionSite.Code;
                    dbEntry.StartDate = businessPartnerByConstructionSite.StartDate;
                    dbEntry.EndDate = businessPartnerByConstructionSite.EndDate;
                    dbEntry.RealEndDate = businessPartnerByConstructionSite.RealEndDate;
                    dbEntry.MaxNumOfEmployees = businessPartnerByConstructionSite.MaxNumOfEmployees;
                    dbEntry.BusinessPartnerCount = businessPartnerByConstructionSite.BusinessPartnerCount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerByConstructionSite Delete(BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            // Load BusinessPartnerByConstructionSite that will be deleted
            BusinessPartnerByConstructionSite dbEntry = context.BusinessPartnerByConstructionSites
                .FirstOrDefault(x => x.Identifier == businessPartnerByConstructionSite.Identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                dbEntry.RealEndDate = businessPartnerByConstructionSite.RealEndDate;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
