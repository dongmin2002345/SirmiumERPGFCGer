using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeByConstructionSiteViewRepository : IEmployeeByConstructionSiteRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public EmployeeByConstructionSiteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSites(int companyId)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = new List<EmployeeByConstructionSite>();

            string queryString =
                "SELECT EmployeeByConstructionSiteId, EmployeeByConstructionSiteIdentifier, EmployeeByConstructionSiteCode, StartDate, EndDate, RealEndDate, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "EmployeeCount, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BusinessPartnerCount, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeByConstructionSites " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeByConstructionSite employeeByConstructionSite;
                    while (reader.Read())
                    {
                        employeeByConstructionSite = new EmployeeByConstructionSite();
                        employeeByConstructionSite.Id = Int32.Parse(reader["EmployeeByConstructionSiteId"].ToString());
                        employeeByConstructionSite.Identifier = Guid.Parse(reader["EmployeeByConstructionSiteIdentifier"].ToString());
                        employeeByConstructionSite.Code = reader["EmployeeByConstructionSiteCode"].ToString();
                        if (reader["StartDate"] != DBNull.Value)
                            employeeByConstructionSite.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        if (reader["EndDate"] != DBNull.Value)
                            employeeByConstructionSite.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        if (reader["RealEndDate"] != DBNull.Value)
                            employeeByConstructionSite.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Employee = new Employee();
                            employeeByConstructionSite.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeByConstructionSite.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeByConstructionSite.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["EmployeeCount"] != DBNull.Value)
                            employeeByConstructionSite.EmployeeCount = Int32.Parse(reader["EmployeeCount"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.BusinessPartner = new BusinessPartner();
                            employeeByConstructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            employeeByConstructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            employeeByConstructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BusinessPartnerCount"] != DBNull.Value)
                            employeeByConstructionSite.BusinessPartnerCount = Int32.Parse(reader["BusinessPartnerCount"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.ConstructionSite = new ConstructionSite();
                            employeeByConstructionSite.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            employeeByConstructionSite.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            employeeByConstructionSite.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        employeeByConstructionSite.Active = bool.Parse(reader["Active"].ToString());
                        employeeByConstructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeByConstructionSite.CreatedBy = new User();
                            employeeByConstructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeByConstructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Company = new Company();
                            employeeByConstructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeByConstructionSites.Add(employeeByConstructionSite);
                    }
                }
            }

            //List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
            //     .Include(x => x.Employee)
            //     .Include(x => x.BusinessPartner)
            //     .Include(x => x.ConstructionSite)
            //     .Include(x => x.Company)
            //     .Include(x => x.CreatedBy)
            //     .Where(x => x.Active == true && x.CompanyId == companyId)
            //     .OrderByDescending(x => x.CreatedAt)
            //     .AsNoTracking()
            //     .ToList();

            return EmployeeByConstructionSites;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSitesAndBusinessPartner(int companyId, int constructionSiteId, int businessPartnerId)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = new List<EmployeeByConstructionSite>();

            string queryString =
                "SELECT EmployeeByConstructionSiteId, EmployeeByConstructionSiteIdentifier, EmployeeByConstructionSiteCode, StartDate, EndDate, RealEndDate, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "EmployeeCount, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BusinessPartnerCount, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeByConstructionSites " +
                "WHERE CompanyId = @CompanyId AND ConstructionSiteId = @ConstructionSiteId AND BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", constructionSiteId));
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeByConstructionSite employeeByConstructionSite;
                    while (reader.Read())
                    {
                        employeeByConstructionSite = new EmployeeByConstructionSite();
                        employeeByConstructionSite.Id = Int32.Parse(reader["EmployeeByConstructionSiteId"].ToString());
                        employeeByConstructionSite.Identifier = Guid.Parse(reader["EmployeeByConstructionSiteIdentifier"].ToString());
                        employeeByConstructionSite.Code = reader["EmployeeByConstructionSiteCode"].ToString();
                        if (reader["StartDate"] != DBNull.Value)
                            employeeByConstructionSite.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        if (reader["EndDate"] != DBNull.Value)
                            employeeByConstructionSite.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        if (reader["RealEndDate"] != DBNull.Value)
                            employeeByConstructionSite.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Employee = new Employee();
                            employeeByConstructionSite.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeByConstructionSite.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeByConstructionSite.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["EmployeeCount"] != DBNull.Value)
                            employeeByConstructionSite.EmployeeCount = Int32.Parse(reader["EmployeeCount"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.BusinessPartner = new BusinessPartner();
                            employeeByConstructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            employeeByConstructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            employeeByConstructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BusinessPartnerCount"] != DBNull.Value)
                            employeeByConstructionSite.BusinessPartnerCount = Int32.Parse(reader["BusinessPartnerCount"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.ConstructionSite = new ConstructionSite();
                            employeeByConstructionSite.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            employeeByConstructionSite.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            employeeByConstructionSite.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        employeeByConstructionSite.Active = bool.Parse(reader["Active"].ToString());
                        employeeByConstructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeByConstructionSite.CreatedBy = new User();
                            employeeByConstructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeByConstructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Company = new Company();
                            employeeByConstructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeByConstructionSites.Add(employeeByConstructionSite);
                    }
                }
            }

            //List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
            //     .Include(x => x.Employee)
            //     .Include(x => x.BusinessPartner)
            //     .Include(x => x.ConstructionSite)
            //     .Include(x => x.Company)
            //     .Include(x => x.CreatedBy)
            //     .Where(x => x.Active == true && x.CompanyId == companyId)
            //     .OrderByDescending(x => x.CreatedAt)
            //     .AsNoTracking()
            //     .ToList();

            return EmployeeByConstructionSites;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = new List<EmployeeByConstructionSite>();

            string queryString =
                "SELECT EmployeeByConstructionSiteId, EmployeeByConstructionSiteIdentifier, EmployeeByConstructionSiteCode, StartDate, EndDate, RealEndDate, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "EmployeeCount, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "BusinessPartnerCount, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeByConstructionSites " +
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
                    EmployeeByConstructionSite employeeByConstructionSite;
                    while (reader.Read())
                    {
                        employeeByConstructionSite = new EmployeeByConstructionSite();
                        employeeByConstructionSite.Id = Int32.Parse(reader["EmployeeByConstructionSiteId"].ToString());
                        employeeByConstructionSite.Identifier = Guid.Parse(reader["EmployeeByConstructionSiteIdentifier"].ToString());
                        employeeByConstructionSite.Code = reader["EmployeeByConstructionSiteCode"].ToString();
                        if (reader["StartDate"] != DBNull.Value)
                            employeeByConstructionSite.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        if (reader["EndDate"] != DBNull.Value)
                            employeeByConstructionSite.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        if (reader["RealEndDate"] != DBNull.Value)
                            employeeByConstructionSite.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Employee = new Employee();
                            employeeByConstructionSite.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByConstructionSite.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeByConstructionSite.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeByConstructionSite.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["EmployeeCount"] != DBNull.Value)
                            employeeByConstructionSite.EmployeeCount = Int32.Parse(reader["EmployeeCount"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.BusinessPartner = new BusinessPartner();
                            employeeByConstructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByConstructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            employeeByConstructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            employeeByConstructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["BusinessPartnerCount"] != DBNull.Value)
                            employeeByConstructionSite.BusinessPartnerCount = Int32.Parse(reader["BusinessPartnerCount"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.ConstructionSite = new ConstructionSite();
                            employeeByConstructionSite.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            employeeByConstructionSite.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            employeeByConstructionSite.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            employeeByConstructionSite.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        employeeByConstructionSite.Active = bool.Parse(reader["Active"].ToString());
                        employeeByConstructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeByConstructionSite.CreatedBy = new User();
                            employeeByConstructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByConstructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeByConstructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeByConstructionSite.Company = new Company();
                            employeeByConstructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByConstructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeByConstructionSites.Add(employeeByConstructionSite);
                    }
                }
            }

            //List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
            //   .Include(x => x.Employee)
            //   .Include(x => x.BusinessPartner)
            //   .Include(x => x.ConstructionSite)
            //   .Include(x => x.Company)
            //   .Include(x => x.CreatedBy)
            //   .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //   .OrderByDescending(x => x.CreatedAt)
            //   .AsNoTracking()
            //   .ToList();

            return EmployeeByConstructionSites;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.EmployeeByConstructionSites
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByConstructionSite))
                    .Select(x => x.Entity as EmployeeByConstructionSite))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-BP-00001";
            else
            {
                string activeCode = context.EmployeeByConstructionSites
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByConstructionSite))
                        .Select(x => x.Entity as EmployeeByConstructionSite))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("EMP-BY-BP-", ""));
                    return "EMP-BY-BP-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public EmployeeByConstructionSite Create(EmployeeByConstructionSite employeeByConstructionSite)
        {
            if (context.EmployeeByConstructionSites.Where(x => x.Identifier != null && x.Identifier == employeeByConstructionSite.Identifier).Count() == 0)
            {
                employeeByConstructionSite.Id = 0;

                employeeByConstructionSite.Code = GetNewCodeValue(employeeByConstructionSite.CompanyId ?? 0);
                employeeByConstructionSite.Active = true;

                employeeByConstructionSite.UpdatedAt = DateTime.Now;
                employeeByConstructionSite.CreatedAt = DateTime.Now;

                context.EmployeeByConstructionSites.Add(employeeByConstructionSite);
                return employeeByConstructionSite;
            }
            else
            {
                // Load employeeByConstructionSite that will be updated
                EmployeeByConstructionSite dbEntry = context.EmployeeByConstructionSites
                .FirstOrDefault(x => x.Identifier == employeeByConstructionSite.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.EmployeeId = employeeByConstructionSite.EmployeeId ?? null;
                    dbEntry.BusinessPartnerId = employeeByConstructionSite.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = employeeByConstructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByConstructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByConstructionSite.Code;
                    dbEntry.StartDate = employeeByConstructionSite.StartDate;
                    dbEntry.EndDate = employeeByConstructionSite.EndDate;
                    dbEntry.RealEndDate = employeeByConstructionSite.RealEndDate;
                    dbEntry.EmployeeCount = employeeByConstructionSite.EmployeeCount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeByConstructionSite Delete(EmployeeByConstructionSite employeeByConstructionSite)
        {
            // Load EmployeeByConstructionSite that will be deleted
            EmployeeByConstructionSite dbEntry = context.EmployeeByConstructionSites
                .Include(x => x.Employee)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Identifier == employeeByConstructionSite.Identifier);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                dbEntry.RealEndDate = employeeByConstructionSite.RealEndDate;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
