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
    public class EmployeeByBusinessPartnerRepository : IEmployeeByBusinessPartnerRepository
    {
        private ApplicationDbContext context;

        public EmployeeByBusinessPartnerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartners(int companyId)
        {
            List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = context.EmployeeByBusinessPartners
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByBusinessPartners;
        }

        public List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = context.EmployeeByBusinessPartners
                .Include(x => x.Employee)
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return EmployeeByBusinessPartners;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.EmployeeByBusinessPartners
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartner))
                    .Select(x => x.Entity as EmployeeByBusinessPartner))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-BP-00001";
            else
            {
                string activeCode = context.EmployeeByBusinessPartners
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartner))
                        .Select(x => x.Entity as EmployeeByBusinessPartner))
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

        public EmployeeByBusinessPartner Create(EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            if (context.EmployeeByBusinessPartners.Where(x => x.Identifier != null && x.Identifier == employeeByBusinessPartner.Identifier).Count() == 0)
            {
                employeeByBusinessPartner.Id = 0;

                employeeByBusinessPartner.Code = GetNewCodeValue(employeeByBusinessPartner.CompanyId ?? 0);
                employeeByBusinessPartner.Active = true;

                employeeByBusinessPartner.UpdatedAt = DateTime.Now;
                employeeByBusinessPartner.CreatedAt = DateTime.Now;

                context.EmployeeByBusinessPartners.Add(employeeByBusinessPartner);
                return employeeByBusinessPartner;
            }
            else
            {
                // Load employeeByBusinessPartner that will be updated
                EmployeeByBusinessPartner dbEntry = context.EmployeeByBusinessPartners
                .FirstOrDefault(x => x.Identifier == employeeByBusinessPartner.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.EmployeeId = employeeByBusinessPartner.EmployeeId ?? null;
                    dbEntry.BusinessPartnerId = employeeByBusinessPartner.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = employeeByBusinessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByBusinessPartner.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByBusinessPartner.Code;
                    dbEntry.StartDate = employeeByBusinessPartner.StartDate;
                    dbEntry.EndDate = employeeByBusinessPartner.EndDate;
                    dbEntry.RealEndDate = employeeByBusinessPartner.RealEndDate;
                    dbEntry.EmployeeCount = employeeByBusinessPartner.EmployeeCount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeByBusinessPartner Delete(EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            // Load EmployeeByBusinessPartner that will be deleted
            EmployeeByBusinessPartner dbEntry = context.EmployeeByBusinessPartners
                .FirstOrDefault(x => x.Identifier == employeeByBusinessPartner.Identifier);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                dbEntry.RealEndDate = employeeByBusinessPartner.RealEndDate;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}

