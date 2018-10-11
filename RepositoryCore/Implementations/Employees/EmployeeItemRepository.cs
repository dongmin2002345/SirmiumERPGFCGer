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
    public class EmployeeItemRepository : IEmployeeItemRepository
    {
        ApplicationDbContext context;

        public EmployeeItemRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeItem> GetEmployeeItems(int companyId)
        {
            List<EmployeeItem> EmployeeItems = context.EmployeeItems
                .Include(x => x.Employee)
                .Include(x => x.FamilyMember)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeItems;
        }

        public List<EmployeeItem> GetEmployeeItemsByEmployee(int EmployeeId)
        {
            List<EmployeeItem> Employees = context.EmployeeItems
                .Include(x => x.Employee)
                .Include(x => x.FamilyMember)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeItem> GetEmployeeItemsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeItem> Employees = context.EmployeeItems
                .Include(x => x.Employee)
                .Include(x => x.FamilyMember)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeItem Create(EmployeeItem EmployeeItem)
        {
            if (context.EmployeeItems.Where(x => x.Identifier != null && x.Identifier == EmployeeItem.Identifier).Count() == 0)
            {
                EmployeeItem.Id = 0;

                EmployeeItem.Active = true;

                context.EmployeeItems.Add(EmployeeItem);
                return EmployeeItem;
            }
            else
            {
                // Load item that will be updated
                EmployeeItem dbEntry = context.EmployeeItems
                    .FirstOrDefault(x => x.Identifier == EmployeeItem.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.FamilyMemberId = EmployeeItem.FamilyMemberId ?? null;
                    dbEntry.CompanyId = EmployeeItem.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeItem.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = EmployeeItem.Name;
                    dbEntry.DateOfBirth = EmployeeItem.DateOfBirth;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeItem Delete(Guid identifier)
        {
            EmployeeItem dbEntry = context.EmployeeItems
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
