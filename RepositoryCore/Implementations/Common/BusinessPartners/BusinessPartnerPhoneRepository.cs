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
    public class BusinessPartnerPhoneRepository : IBusinessPartnerPhoneRepository
    {
        ApplicationDbContext context;

        public BusinessPartnerPhoneRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerPhone> GetBusinessPartnerPhones(int companyId)
        {
            List<BusinessPartnerPhone> businessPartnerPhones = context.BusinessPartnerPhones
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return businessPartnerPhones;
        }

        public List<BusinessPartnerPhone> GetBusinessPartnerPhonesByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerPhone> businessPartnerPhones = context.BusinessPartnerPhones
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerPhones;
        }

        public List<BusinessPartnerPhone> GetBusinessPartnerPhonesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerPhone> businessPartnerPhones = context.BusinessPartnerPhones
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return businessPartnerPhones;
        }

        public BusinessPartnerPhone GetBusinessPartnerPhone(int id)
        {
            return context.BusinessPartnerPhones
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerPhone Create(BusinessPartnerPhone businessPartnerPhone)
        {
            if (context.BusinessPartnerPhones.Where(x => x.Identifier != null && x.Identifier == businessPartnerPhone.Identifier).Count() == 0)
            {
                businessPartnerPhone.Id = 0;

                businessPartnerPhone.Active = true;

                context.BusinessPartnerPhones.Add(businessPartnerPhone);
                return businessPartnerPhone;
            }
            else
            {
                // Load businessPartnerPhone that will be updated
                BusinessPartnerPhone dbEntry = context.BusinessPartnerPhones
                    .FirstOrDefault(x => x.Identifier == businessPartnerPhone.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerPhone.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = businessPartnerPhone.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerPhone.CreatedById ?? null;

                    // Set properties
                    dbEntry.Phone = businessPartnerPhone.Phone;
                    dbEntry.Mobile = businessPartnerPhone.Mobile;
                    dbEntry.Fax = businessPartnerPhone.Fax;
                    dbEntry.Email = businessPartnerPhone.Email;
                    dbEntry.ContactPersonFirstName = businessPartnerPhone.ContactPersonFirstName;
                    dbEntry.ContactPersonLastName = businessPartnerPhone.ContactPersonLastName;

                    dbEntry.Description = businessPartnerPhone.Description;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerPhone Delete(Guid identifier)
        {
            BusinessPartnerPhone dbEntry = context.BusinessPartnerPhones
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
