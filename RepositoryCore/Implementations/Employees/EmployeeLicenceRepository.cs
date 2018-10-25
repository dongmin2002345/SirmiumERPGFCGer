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
    public class EmployeeLicenceRepository : IEmployeeLicenceRepository
    {
        ApplicationDbContext context;

        public EmployeeLicenceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeLicence> GetEmployeeItems(int companyId)
        {
            List<EmployeeLicence> EmployeeLicences = context.EmployeeLicences
                .Include(x => x.Employee)
                .Include(x => x.Licence)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeLicences;
        }

        public List<EmployeeLicence> GetEmployeeItemsByEmployee(int EmployeeId)
        {
            List<EmployeeLicence> Employees = context.EmployeeLicences
                .Include(x => x.Employee)
                .Include(x => x.Licence)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeLicence> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeLicence> Employees = context.EmployeeLicences
                .Include(x => x.Employee)
                .Include(x => x.Licence)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeLicence Create(EmployeeLicence EmployeeLicence)
        {
            if (context.EmployeeLicences.Where(x => x.Identifier != null && x.Identifier == EmployeeLicence.Identifier).Count() == 0)
            {
                EmployeeLicence.Id = 0;

                EmployeeLicence.Active = true;

                context.EmployeeLicences.Add(EmployeeLicence);
                return EmployeeLicence;
            }
            else
            {
                // Load item that will be updated
                EmployeeLicence dbEntry = context.EmployeeLicences
                    .FirstOrDefault(x => x.Identifier == EmployeeLicence.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.LicenceId = EmployeeLicence.LicenceId ?? null;
                    dbEntry.CountryId = EmployeeLicence.CountryId ?? null;
                    dbEntry.CompanyId = EmployeeLicence.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeLicence.CreatedById ?? null;

                    dbEntry.ValidFrom = EmployeeLicence.ValidFrom;
                    dbEntry.ValidTo = EmployeeLicence.ValidTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeLicence Delete(Guid identifier)
        {
            EmployeeLicence dbEntry = context.EmployeeLicences
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
