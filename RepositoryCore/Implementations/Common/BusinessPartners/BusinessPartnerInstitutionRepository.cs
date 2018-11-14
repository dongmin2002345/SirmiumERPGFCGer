using DomainCore.Common.BusinessPartners;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerInstitutionRepository : IBusinessPartnerInstitutionRepository
    {
        ApplicationDbContext context;

        public BusinessPartnerInstitutionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutions(int companyId)
        {
            List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return businessPartnerInstitutions;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerInstitutions;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return businessPartnerInstitutions;
        }

        public BusinessPartnerInstitution GetBusinessPartnerInstitution(int id)
        {
            return context.BusinessPartnerInstitutions
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerInstitution Create(BusinessPartnerInstitution businessPartnerInstitution)
        {
            if (context.BusinessPartnerInstitutions.Where(x => x.Identifier != null && x.Identifier == businessPartnerInstitution.Identifier).Count() == 0)
            {
                businessPartnerInstitution.Id = 0;

                businessPartnerInstitution.Active = true;

                businessPartnerInstitution.CreatedAt = DateTime.Now;
                businessPartnerInstitution.UpdatedAt = DateTime.Now;

                context.BusinessPartnerInstitutions.Add(businessPartnerInstitution);
                return businessPartnerInstitution;
            }
            else
            {
                // Load businessPartnerInstitution that will be updated
                BusinessPartnerInstitution dbEntry = context.BusinessPartnerInstitutions
                    .FirstOrDefault(x => x.Identifier == businessPartnerInstitution.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerInstitution.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = businessPartnerInstitution.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerInstitution.CreatedById ?? null;

                    // Set properties
                    dbEntry.Institution = businessPartnerInstitution.Institution;
                    dbEntry.Username = businessPartnerInstitution.Username;
                    dbEntry.Password = businessPartnerInstitution.Password;
                    dbEntry.ContactPerson = businessPartnerInstitution.ContactPerson;
                    dbEntry.Phone = businessPartnerInstitution.Phone;
                    dbEntry.Fax = businessPartnerInstitution.Fax;
                    dbEntry.Email = businessPartnerInstitution.Email;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerInstitution Delete(Guid identifier)
        {
            BusinessPartnerInstitution dbEntry = context.BusinessPartnerInstitutions
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
