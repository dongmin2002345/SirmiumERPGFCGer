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
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.City)
                .Include(x => x.PassportCountry)
                .Include(x => x.PassportCity)
                .Include(x => x.ResidenceCity)
                .Where(x => x.Company.Id == companyId && x.Active == true)
                .AsNoTracking()
                .ToList();
        }

        public Employee GetEmployee(int employeeId)
        {
            return context.Employees
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.City)
                .Include(x => x.PassportCountry)
                .Include(x => x.PassportCity)
                .Include(x => x.ResidenceCity)
                .FirstOrDefault(x => x.Id == employeeId && x.Active == true);
        }

        public List<Employee> GetEmployeesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            return context.Employees
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.City)
                .Include(x => x.PassportCountry)
                .Include(x => x.PassportCity)
                .Include(x => x.ResidenceCity)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
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

        public Employee Create(Employee employee)
        {
            if (context.Employees.Where(x => x.Identifier != null && x.Identifier == employee.Identifier).Count() == 0)
            {
                employee.Id = 0;

                employee.Code = GetNewCodeValue(employee.CompanyId ?? 0);
                employee.Active = true;

                employee.UpdatedAt = DateTime.Now;
                employee.CreatedAt = DateTime.Now;

                context.Employees.Add(employee);
                return employee;
            }
            else
            {
                // Load item that will be updated
                Employee dbEntry = context.Employees
                    .FirstOrDefault(x => x.Identifier == employee.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = employee.CompanyId ?? null;
                    dbEntry.CreatedById = employee.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employee.Code;
                    dbEntry.EmployeeCode = employee.EmployeeCode;
                    dbEntry.Name = employee.Name;
                    dbEntry.SurName = employee.SurName;

                    dbEntry.ConstructionSiteCode = employee.ConstructionSiteCode;

                    dbEntry.DateOfBirth = employee.DateOfBirth;
                    dbEntry.Gender = employee.Gender;
                    dbEntry.CountryId = employee.CountryId;
                    dbEntry.RegionId = employee.RegionId;
                    dbEntry.MunicipalityId = employee.MunicipalityId;
                    dbEntry.CityId = employee.CityId;
                    dbEntry.Address = employee.Address;

                    dbEntry.Passport = employee.Passport;
                    dbEntry.VisaFrom = employee.VisaFrom;
                    dbEntry.VisaTo = employee.VisaTo;

                    dbEntry.PassportCountryId = employee.PassportCountryId;
                    dbEntry.PassportCityId = employee.PassportCityId;

                    dbEntry.ResidenceCityId = employee.ResidenceCityId;
                    dbEntry.ResidenceAddress = employee.ResidenceAddress;

                    dbEntry.EmbassyDate = employee.EmbassyDate;
                    dbEntry.VisaDate = employee.VisaDate;
                    dbEntry.VisaValidFrom = employee.VisaValidFrom;
                    dbEntry.VisaValidTo = employee.VisaValidTo;
                    dbEntry.WorkPermitFrom = employee.WorkPermitFrom;
                    dbEntry.WorkPermitTo = employee.WorkPermitTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Employee Delete(Guid identifier)
        {
            // Load Employee that will be deleted
            Employee dbEntry = context.Employees
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

        #endregion
    }
}
