using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeByConstructionSiteRepository : IEmployeeByConstructionSiteRepository
    {
        private ApplicationDbContext context;

        public EmployeeByConstructionSiteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSites(int companyId)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByConstructionSites;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSitesAndBusinessPartner(int companyId, int constructionSiteId, int businessPartnerId)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId && x.ConstructionSiteId == constructionSiteId && x.BusinessPartnerId == businessPartnerId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByConstructionSites;
        }

        public List<EmployeeByConstructionSite> GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByConstructionSite> EmployeeByConstructionSites = context.EmployeeByConstructionSites
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

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
                return "EMP-BY-CS-00001";
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
                    int intValue = Int32.Parse(activeCode.Replace("EMP-BY-CS-", ""));
                    return "EMP-BY-CS-" + (intValue + 1).ToString("00000");
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
                    dbEntry.ConstructionSiteId = employeeByConstructionSite.ConstructionSiteId ?? null;
                    dbEntry.CompanyId = employeeByConstructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByConstructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByConstructionSite.Code;
                    dbEntry.StartDate = employeeByConstructionSite.StartDate;
                    dbEntry.EndDate = employeeByConstructionSite.EndDate;
                    dbEntry.RealEndDate = employeeByConstructionSite.RealEndDate;
                    dbEntry.EmployeeCount = employeeByConstructionSite.EmployeeCount;
                    dbEntry.BusinessPartnerCount = employeeByConstructionSite.BusinessPartnerCount;

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
                .FirstOrDefault(x => x.Identifier == employeeByConstructionSite.Identifier && x.Active == true);

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
