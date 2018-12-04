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
    public class LimitationRepository : ILimitationRepository
    {
        private ApplicationDbContext context;

        public LimitationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Limitation> GetLimitations(int companyId)
        {
            List<Limitation> limitations = context.Limitations
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return limitations;
        }

        public List<Limitation> GetLimitationsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Limitation> limitations = context.Limitations
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return limitations;
        }

        public Limitation Create(Limitation limitation)
        {
            if (context.Limitations.Where(x => x.Identifier != null && x.Identifier == limitation.Identifier).Count() == 0)
            {
                limitation.Id = 0;

                limitation.Active = true;

                limitation.UpdatedAt = DateTime.Now;
                limitation.CreatedAt = DateTime.Now;

                context.Limitations.Add(limitation);
                return limitation;
            }
            else
            {
                Limitation dbEntry = context.Limitations
                    .FirstOrDefault(x => x.Identifier == limitation.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = limitation.CompanyId ?? null;
                    dbEntry.CreatedById = limitation.CreatedById ?? null;

                    // Set properties
                    dbEntry.ConstructionSiteLimit = limitation.ConstructionSiteLimit;
                    dbEntry.BusinessPartnerConstructionSiteLimit = limitation.BusinessPartnerConstructionSiteLimit;
                    dbEntry.EmployeeConstructionSiteLimit = limitation.EmployeeConstructionSiteLimit;
                    dbEntry.EmployeeBusinessPartnerLimit = limitation.EmployeeBusinessPartnerLimit;
                    dbEntry.EmployeeBirthdayLimit = limitation.EmployeeBirthdayLimit;

                    dbEntry.EmployeePassportLimit = limitation.EmployeePassportLimit;
                    dbEntry.EmployeeEmbasyLimit = limitation.EmployeeEmbasyLimit;
                    dbEntry.EmployeeVisaTakeOffLimit = limitation.EmployeeVisaTakeOffLimit;
                    dbEntry.EmployeeVisaLimit = limitation.EmployeeVisaLimit;
                    dbEntry.EmployeeWorkLicenceLimit = limitation.EmployeeWorkLicenceLimit;
                    dbEntry.EmployeeDriveLicenceLimit = limitation.EmployeeDriveLicenceLimit;
                    dbEntry.EmployeeEmbasyFamilyLimit = limitation.EmployeeEmbasyFamilyLimit;

                    dbEntry.PersonPassportLimit = limitation.PersonPassportLimit;
                    dbEntry.PersonEmbasyLimit = limitation.PersonEmbasyLimit;
                    dbEntry.PersonVisaTakeOffLimit = limitation.PersonVisaTakeOffLimit;
                    dbEntry.PersonVisaLimit = limitation.PersonVisaLimit;
                    dbEntry.PersonWorkLicenceLimit = limitation.PersonWorkLicenceLimit;
                    dbEntry.PersonDriveLicenceLimit = limitation.PersonDriveLicenceLimit;
                    dbEntry.PersonEmbasyFamilyLimit = limitation.PersonEmbasyFamilyLimit;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }
    }
}
