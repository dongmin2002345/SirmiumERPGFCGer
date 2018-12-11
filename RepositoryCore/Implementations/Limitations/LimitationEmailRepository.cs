using DomainCore.Limitations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Limitations;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Limitations
{
    public class LimitationEmailRepository : ILimitationEmailRepository
    {
        ApplicationDbContext context;

        public LimitationEmailRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<LimitationEmail> GetLimitationEmails(int companyId)
        {
            List<LimitationEmail> LimitationEmails = context.LimitationEmails
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return LimitationEmails;
        }

        public List<LimitationEmail> GetLimitationEmailsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<LimitationEmail> LimitationEmails = context.LimitationEmails
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return LimitationEmails;
        }

        public LimitationEmail Create(LimitationEmail LimitationEmail)
        {
            if (context.LimitationEmails.Where(x => x.Identifier != null && x.Identifier == LimitationEmail.Identifier).Count() == 0)
            {
                LimitationEmail.Id = 0;

                LimitationEmail.Active = true;

                LimitationEmail.UpdatedAt = DateTime.Now;
                LimitationEmail.CreatedAt = DateTime.Now;

                context.LimitationEmails.Add(LimitationEmail);
                return LimitationEmail;
            }
            else
            {
                // Load LimitationEmail that will be updated
                LimitationEmail dbEntry = context.LimitationEmails
                .FirstOrDefault(x => x.Identifier == LimitationEmail.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = LimitationEmail.CompanyId ?? null;
                    dbEntry.CreatedById = LimitationEmail.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = LimitationEmail.Name;
                    dbEntry.LastName = LimitationEmail.LastName;
                    dbEntry.Email = LimitationEmail.Email;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public LimitationEmail Delete(Guid identifier)
        {
            // Load LimitationEmail that will be deleted
            LimitationEmail dbEntry = context.LimitationEmails
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
