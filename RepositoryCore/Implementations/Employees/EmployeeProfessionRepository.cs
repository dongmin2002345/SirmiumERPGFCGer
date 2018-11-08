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
    public class EmployeeProfessionRepository : IEmployeeProfessionRepository
    {
        ApplicationDbContext context;

        public EmployeeProfessionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeProfession> GetEmployeeItems(int companyId)
        {
            List<EmployeeProfession> EmployeeProfessions = context.EmployeeProfessions
                .Include(x => x.Employee)
                .Include(x => x.Profession)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeProfessions;
        }

        public List<EmployeeProfession> GetEmployeeItemsByEmployee(int EmployeeId)
        {
            List<EmployeeProfession> Employees = context.EmployeeProfessions
                .Include(x => x.Employee)
                .Include(x => x.Profession)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeProfession> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeProfession> Employees = context.EmployeeProfessions
                .Include(x => x.Employee)
                .Include(x => x.Profession)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeProfession Create(EmployeeProfession EmployeeProfession)
        {
            if (context.EmployeeProfessions.Where(x => x.Identifier != null && x.Identifier == EmployeeProfession.Identifier).Count() == 0)
            {
                EmployeeProfession.Id = 0;

                EmployeeProfession.Active = true;

                context.EmployeeProfessions.Add(EmployeeProfession);
                return EmployeeProfession;
            }
            else
            {
                // Load item that will be updated
                EmployeeProfession dbEntry = context.EmployeeProfessions
                    .FirstOrDefault(x => x.Identifier == EmployeeProfession.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.ProfessionId = EmployeeProfession.ProfessionId ?? null;
                    dbEntry.CountryId = EmployeeProfession.CountryId ?? null;
                    dbEntry.CompanyId = EmployeeProfession.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeProfession.CreatedById ?? null;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeProfession Delete(Guid identifier)
        {
            EmployeeProfession dbEntry = context.EmployeeProfessions
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
