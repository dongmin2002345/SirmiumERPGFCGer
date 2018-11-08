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
    public class BusinessPartnerLocationRepository : IBusinessPartnerLocationRepository
    {
        ApplicationDbContext context;

        public BusinessPartnerLocationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocations(int companyId)
        {
            List<BusinessPartnerLocation> businessPartnerLocationes = context.BusinessPartnerLocations
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return businessPartnerLocationes;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocationssByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerLocation> businessPartnerLocationes = context.BusinessPartnerLocations
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                         .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerLocationes;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocationsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerLocation> businessPartnerLocationes = context.BusinessPartnerLocations
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                         .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return businessPartnerLocationes;
        }

        public BusinessPartnerLocation GetBusinessPartnerLocation(int id)
        {
            return context.BusinessPartnerLocations
                .Include(x => x.BusinessPartner)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Municipality)
                         .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerLocation Create(BusinessPartnerLocation businessPartnerLocation)
        {
            if (context.BusinessPartnerLocations.Where(x => x.Identifier != null && x.Identifier == businessPartnerLocation.Identifier).Count() == 0)
            {
                businessPartnerLocation.Id = 0;

                businessPartnerLocation.Active = true;

                context.BusinessPartnerLocations.Add(businessPartnerLocation);
                return businessPartnerLocation;
            }
            else
            {
                // Load businessPartnerLocation that will be updated
                BusinessPartnerLocation dbEntry = context.BusinessPartnerLocations
                    .FirstOrDefault(x => x.Identifier == businessPartnerLocation.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerLocation.BusinessPartnerId ?? null;
                    dbEntry.CountryId = businessPartnerLocation.CountryId ?? null;
                    dbEntry.CityId = businessPartnerLocation.CityId ?? null;
                    dbEntry.MunicipalityId = businessPartnerLocation.MunicipalityId ?? null;
                    dbEntry.RegionId = businessPartnerLocation.RegionId ?? null;
                    dbEntry.CompanyId = businessPartnerLocation.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerLocation.CreatedById ?? null;

                    // Set properties
                    dbEntry.Address = businessPartnerLocation.Address;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerLocation Delete(Guid identifier)
        {
            BusinessPartnerLocation dbEntry = context.BusinessPartnerLocations
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
