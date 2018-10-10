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
    public class EmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        #region GET methods

        public List<Employee> GetEmployees(int companyId)
        {
            return context.Employees
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.Active == true)
                .AsNoTracking()
                .ToList();
        }

        public List<Employee> GetEmployeesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            return context.Employees
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .AsNoTracking()
                .ToList();
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Employees
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Employee))
                    .Select(x => x.Entity as Employee))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "RAD-00001";
            else
            {
                string activeCode = context.Employees
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Employee))
                        .Select(x => x.Entity as Employee))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("RAD-", ""));
                    return "RAD-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        #endregion

        #region CREATE methods

        public Employee Create(Employee Employee)
        {
            if (context.Employees.Where(x => x.Identifier != null && x.Identifier == Employee.Identifier).Count() == 0)
            {
                Employee.Id = 0;

                Employee.Code = GetNewCodeValue(Employee.CompanyId ?? 0);
                Employee.Active = true;

                Employee.UpdatedAt = DateTime.Now;
                Employee.CreatedAt = DateTime.Now;

                context.Employees.Add(Employee);
                return Employee;
            }
            else
            {
                // Load item that will be updated
                Employee dbEntry = context.Employees
                    .FirstOrDefault(x => x.Identifier == Employee.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = Employee.CompanyId ?? null;
                    dbEntry.CreatedById = Employee.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = Employee.Code;
                    dbEntry.EmployeeCode = Employee.EmployeeCode;
                    dbEntry.Name = Employee.Name;
                    dbEntry.SurName = Employee.SurName;

                    dbEntry.DateOfBirth = Employee.DateOfBirth;

                    dbEntry.Address = Employee.Address;
                    dbEntry.Passport = Employee.Passport;
                    dbEntry.Interest = Employee.Interest;
                    dbEntry.License = Employee.License;

                    dbEntry.EmbassyDate = Employee.EmbassyDate;
                    dbEntry.VisaFrom = Employee.VisaFrom;
                    dbEntry.VisaTo = Employee.VisaTo;
                    dbEntry.WorkPermitFrom = Employee.WorkPermitFrom;
                    dbEntry.WorkPermitTo = Employee.WorkPermitTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        #endregion
    }
}
