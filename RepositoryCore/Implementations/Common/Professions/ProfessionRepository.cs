using DomainCore.Common.Professions;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Professions
{
    public class ProfessionRepository : IProfessionRepository
    {
        private ApplicationDbContext context;

        public ProfessionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Profession> GetProfessions(int companyId)
        {
            List<Profession> professions = context.Professions
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return professions;
        }

        public List<Profession> GetProfessionsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Profession> professions = context.Professions
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return professions;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Professions
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Profession))
                    .Select(x => x.Entity as Profession))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "PROF-00001";
            else
            {
                string activeCode = context.Professions
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Profession))
                        .Select(x => x.Entity as Profession))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("PROF-", ""));
                    return "PROF-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Profession Create(Profession profession)
        {
            if (context.Professions.Where(x => x.Identifier != null && x.Identifier == profession.Identifier).Count() == 0)
            {
                profession.Id = 0;

                profession.Code = GetNewCodeValue(profession.CompanyId ?? 0);
                profession.Active = true;

                profession.UpdatedAt = DateTime.Now;
                profession.CreatedAt = DateTime.Now;

                context.Professions.Add(profession);
                return profession;
            }
            else
            {
                // Load profession that will be updated
                Profession dbEntry = context.Professions
                .FirstOrDefault(x => x.Identifier == profession.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = profession.CountryId ?? null;
                    dbEntry.CompanyId = profession.CompanyId ?? null;
                    dbEntry.CreatedById = profession.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = profession.Code;
                    dbEntry.SecondCode = profession.SecondCode;
                    dbEntry.Name = profession.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Profession Delete(Guid identifier)
        {
            // Load Profession that will be deleted
            Profession dbEntry = context.Professions
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
