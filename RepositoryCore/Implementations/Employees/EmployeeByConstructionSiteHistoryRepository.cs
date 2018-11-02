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
    public class EmployeeByConstructionSiteHistoryRepository : IEmployeeByConstructionSiteHistoryRepository
    {
        private ApplicationDbContext context;

        public EmployeeByConstructionSiteHistoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeByConstructionSiteHistory> GetEmployeeByConstructionSiteHistories(int companyId)
        {
            List<EmployeeByConstructionSiteHistory> EmployeeByConstructionSiteHistories = context.EmployeeByConstructionSiteHistories
                .Include(x => x.Employee)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByConstructionSiteHistories;
        }

        public List<EmployeeByConstructionSiteHistory> GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByConstructionSiteHistory> EmployeeByConstructionSiteHistories = context.EmployeeByConstructionSiteHistories
                .Include(x => x.Employee)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByConstructionSiteHistories;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.EmployeeByConstructionSiteHistories
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByConstructionSiteHistory))
                    .Select(x => x.Entity as EmployeeByConstructionSiteHistory))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-CS-HIS-00001";
            else
            {
                string activeCode = context.EmployeeByConstructionSiteHistories
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByConstructionSiteHistory))
                        .Select(x => x.Entity as EmployeeByConstructionSiteHistory))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("EMP-BY-CS-HIS-", ""));
                    return "EMP-BY-CS-HIS-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public EmployeeByConstructionSiteHistory Create(EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory)
        {
            if (context.EmployeeByConstructionSiteHistories.Where(x => x.Identifier != null && x.Identifier == employeeByConstructionSiteHistory.Identifier).Count() == 0)
            {
                employeeByConstructionSiteHistory.Id = 0;

                employeeByConstructionSiteHistory.Code = GetNewCodeValue(employeeByConstructionSiteHistory.CompanyId ?? 0);
                employeeByConstructionSiteHistory.Active = true;

                employeeByConstructionSiteHistory.UpdatedAt = DateTime.Now;
                employeeByConstructionSiteHistory.CreatedAt = DateTime.Now;

                context.EmployeeByConstructionSiteHistories.Add(employeeByConstructionSiteHistory);
                return employeeByConstructionSiteHistory;
            }
            else
            {
                // Load employeeByConstructionSiteHistory that will be updated
                EmployeeByConstructionSiteHistory dbEntry = context.EmployeeByConstructionSiteHistories
                .FirstOrDefault(x => x.Identifier == employeeByConstructionSiteHistory.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.EmployeeId = employeeByConstructionSiteHistory.EmployeeId ?? null;
                    dbEntry.ConstructionSiteId = employeeByConstructionSiteHistory.ConstructionSiteId ?? null;
                    dbEntry.CompanyId = employeeByConstructionSiteHistory.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByConstructionSiteHistory.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByConstructionSiteHistory.Code;
                    dbEntry.StartDate = employeeByConstructionSiteHistory.StartDate;
                    dbEntry.EndDate = employeeByConstructionSiteHistory.EndDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeByConstructionSiteHistory Delete(Guid identifier)
        {
            // Load EmployeeByConstructionSiteHistory that will be deleted
            EmployeeByConstructionSiteHistory dbEntry = context.EmployeeByConstructionSiteHistories
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
