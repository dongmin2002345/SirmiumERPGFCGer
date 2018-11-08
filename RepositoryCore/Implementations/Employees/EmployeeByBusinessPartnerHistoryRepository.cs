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
    public class EmployeeByBusinessPartnerHistoryRepository : IEmployeeByBusinessPartnerHistoryRepository
    {
        private ApplicationDbContext context;

        public EmployeeByBusinessPartnerHistoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeByBusinessPartnerHistory> GetEmployeeByBusinessPartnerHistories(int companyId)
        {
            List<EmployeeByBusinessPartnerHistory> EmployeeByBusinessPartnerHistories = context.EmployeeByBusinessPartnerHistories
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByBusinessPartnerHistories;
        }

        public List<EmployeeByBusinessPartnerHistory> GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByBusinessPartnerHistory> EmployeeByBusinessPartnerHistories = context.EmployeeByBusinessPartnerHistories
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByBusinessPartnerHistories;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.EmployeeByBusinessPartnerHistories
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartnerHistory))
                    .Select(x => x.Entity as EmployeeByBusinessPartnerHistory))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-CS-HIS-00001";
            else
            {
                string activeCode = context.EmployeeByBusinessPartnerHistories
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartnerHistory))
                        .Select(x => x.Entity as EmployeeByBusinessPartnerHistory))
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

        public EmployeeByBusinessPartnerHistory Create(EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory)
        {
            if (context.EmployeeByBusinessPartnerHistories.Where(x => x.Identifier != null && x.Identifier == employeeByBusinessPartnerHistory.Identifier).Count() == 0)
            {
                employeeByBusinessPartnerHistory.Id = 0;

                employeeByBusinessPartnerHistory.Code = GetNewCodeValue(employeeByBusinessPartnerHistory.CompanyId ?? 0);
                employeeByBusinessPartnerHistory.Active = true;

                employeeByBusinessPartnerHistory.UpdatedAt = DateTime.Now;
                employeeByBusinessPartnerHistory.CreatedAt = DateTime.Now;

                context.EmployeeByBusinessPartnerHistories.Add(employeeByBusinessPartnerHistory);
                return employeeByBusinessPartnerHistory;
            }
            else
            {
                // Load employeeByBusinessPartnerHistory that will be updated
                EmployeeByBusinessPartnerHistory dbEntry = context.EmployeeByBusinessPartnerHistories
                .FirstOrDefault(x => x.Identifier == employeeByBusinessPartnerHistory.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.EmployeeId = employeeByBusinessPartnerHistory.EmployeeId ?? null;
                    dbEntry.BusinessPartnerId = employeeByBusinessPartnerHistory.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = employeeByBusinessPartnerHistory.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByBusinessPartnerHistory.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByBusinessPartnerHistory.Code;
                    dbEntry.StartDate = employeeByBusinessPartnerHistory.StartDate;
                    dbEntry.EndDate = employeeByBusinessPartnerHistory.EndDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeByBusinessPartnerHistory Delete(Guid identifier)
        {
            // Load EmployeeByBusinessPartnerHistory that will be deleted
            EmployeeByBusinessPartnerHistory dbEntry = context.EmployeeByBusinessPartnerHistories
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

