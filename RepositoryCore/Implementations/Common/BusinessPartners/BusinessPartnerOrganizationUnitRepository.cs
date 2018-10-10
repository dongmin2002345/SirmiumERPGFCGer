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
    public class BusinessPartnerOrganizationUnitRepository : IBusinessPartnerOrganizationUnitRepository
    {
        ApplicationDbContext context;

        public BusinessPartnerOrganizationUnitRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnits(int companyId)
        {
            List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return businessPartnerOrganizationUnits;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerOrganizationUnits;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerOrganizationUnits;
        }

        public BusinessPartnerOrganizationUnit GetBusinessPartnerOrganizationUnit(int id)
        {
            return context.BusinessPartnerOrganizationUnits
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerOrganizationUnit Create(BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit)
        {
            if (context.BusinessPartnerOrganizationUnits.Where(x => x.Identifier != null && x.Identifier == businessPartnerOrganizationUnit.Identifier).Count() == 0)
            {
                businessPartnerOrganizationUnit.Id = 0;

                businessPartnerOrganizationUnit.Active = true;

                context.BusinessPartnerOrganizationUnits.Add(businessPartnerOrganizationUnit);
                return businessPartnerOrganizationUnit;
            }
            else
            {
                // Load businessPartnerOrganizationUnit that will be updated
                BusinessPartnerOrganizationUnit dbEntry = context.BusinessPartnerOrganizationUnits
                    .FirstOrDefault(x => x.Identifier == businessPartnerOrganizationUnit.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerOrganizationUnit.BusinessPartnerId ?? null;
                    dbEntry.CountryId = businessPartnerOrganizationUnit.CountryId ?? null;
                    dbEntry.CityId = businessPartnerOrganizationUnit.CityId ?? null;
                    dbEntry.MunicipalityId = businessPartnerOrganizationUnit.MunicipalityId ?? null;
                    dbEntry.CompanyId = businessPartnerOrganizationUnit.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerOrganizationUnit.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerOrganizationUnit.Code;
                    dbEntry.Name = businessPartnerOrganizationUnit.Name;
                    dbEntry.Address = businessPartnerOrganizationUnit.Address;
                    dbEntry.ContactPerson = businessPartnerOrganizationUnit.ContactPerson;
                    dbEntry.Phone = businessPartnerOrganizationUnit.Phone;
                    dbEntry.Mobile = businessPartnerOrganizationUnit.Mobile;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerOrganizationUnit Delete(Guid identifier)
        {
            BusinessPartnerOrganizationUnit dbEntry = context.BusinessPartnerOrganizationUnits
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
