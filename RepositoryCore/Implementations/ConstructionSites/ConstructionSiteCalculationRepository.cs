using DomainCore.ConstructionSites;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteCalculationRepository : IConstructionSiteCalculationRepository
    {
        ApplicationDbContext context;

        public ConstructionSiteCalculationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculations(int companyId)
        {
            List<ConstructionSiteCalculation> ConstructionSiteCalculations = context.ConstructionSiteCalculations
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return ConstructionSiteCalculations;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculationsByConstructionSite(int constructionSiteId)
        {
            List<ConstructionSiteCalculation> ConstructionSites = context.ConstructionSiteCalculations
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.ConstructionSiteId == constructionSiteId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return ConstructionSites;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculationsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSiteCalculation> ConstructionSites = context.ConstructionSiteCalculations
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return ConstructionSites;
        }

        public ConstructionSiteCalculation GetLastConstructionSiteCalculation(int companyId, int constructionSiteId)
        {
            ConstructionSiteCalculation constructionSite = context.ConstructionSiteCalculations
                .Where(x => x.Company.Id == companyId && x.ConstructionSiteId == constructionSiteId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            Console.WriteLine("NOVO");

            return constructionSite;
        }

        public ConstructionSiteCalculation Create(ConstructionSiteCalculation constructionSiteCalculation)
        {
            if (context.ConstructionSiteCalculations.Where(x => x.Identifier != null && x.Identifier == constructionSiteCalculation.Identifier).Count() == 0)
            {
                constructionSiteCalculation.Id = 0;

                constructionSiteCalculation.Active = true;

                context.ConstructionSiteCalculations.Add(constructionSiteCalculation);
                return constructionSiteCalculation;
            }
            else
            {
                // Load item that will be updated
                ConstructionSiteCalculation dbEntry = context.ConstructionSiteCalculations
                    .FirstOrDefault(x => x.Identifier == constructionSiteCalculation.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = constructionSiteCalculation.CompanyId ?? null;
                    dbEntry.CreatedById = constructionSiteCalculation.CreatedById ?? null;

                    // Set properties
                    dbEntry.StatusDate = constructionSiteCalculation.StatusDate;
                    dbEntry.NumOfEmployees = constructionSiteCalculation.NumOfEmployees;
                    dbEntry.EmployeePrice = constructionSiteCalculation.EmployeePrice;
                    dbEntry.NumOfMonths = constructionSiteCalculation.NumOfMonths;
                    dbEntry.OldValue = constructionSiteCalculation.OldValue;
                    dbEntry.NewValue = constructionSiteCalculation.NewValue;
                    dbEntry.ValueDifference = constructionSiteCalculation.ValueDifference;
                    dbEntry.PlusMinus = constructionSiteCalculation.PlusMinus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSiteCalculation Delete(Guid identifier)
        {
            ConstructionSiteCalculation dbEntry = context.ConstructionSiteCalculations
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
