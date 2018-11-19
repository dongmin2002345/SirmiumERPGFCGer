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
    public class EmployeeCardRepository : IEmployeeCardRepository
    {
        ApplicationDbContext context;

        public EmployeeCardRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeCard> GetEmployeeCards(int companyId)
        {
            List<EmployeeCard> EmployeeCards = context.EmployeeCards
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeCards;
        }

        public List<EmployeeCard> GetEmployeeCardsByEmployee(int EmployeeId)
        {
            List<EmployeeCard> Employees = context.EmployeeCards
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeCard> GetEmployeeCardsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeCard> Employees = context.EmployeeCards
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeCard Create(EmployeeCard EmployeeCard)
        {
            if (context.EmployeeCards.Where(x => x.Identifier != null && x.Identifier == EmployeeCard.Identifier).Count() == 0)
            {
                EmployeeCard.Id = 0;

                EmployeeCard.Active = true;

                context.EmployeeCards.Add(EmployeeCard);
                return EmployeeCard;
            }
            else
            {
                // Load item that will be updated
                EmployeeCard dbEntry = context.EmployeeCards
                    .FirstOrDefault(x => x.Identifier == EmployeeCard.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeCard.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeCard.CreatedById ?? null;

                    // Set properties
                    dbEntry.CardDate = EmployeeCard.CardDate;
                    dbEntry.Description = EmployeeCard.Description;
                    dbEntry.PlusMinus = EmployeeCard.PlusMinus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeCard Delete(Guid identifier)
        {
            EmployeeCard dbEntry = context.EmployeeCards
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
