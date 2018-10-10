using DomainCore.Common.BusinessPartners;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerRepository : IBusinessPartnerRepository
    {
        private ApplicationDbContext context;

        public BusinessPartnerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartner> GetBusinessPartners(int companyId)
        {
            List<BusinessPartner> businessPartners = context.BusinessPartners
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartners;
        }

        public List<BusinessPartner> GetBusinessPartnersNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartner> businessPartners = context.BusinessPartners
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return businessPartners;
        }

        public BusinessPartner GetBusinessPartner(int id)
        {
            return context.BusinessPartners
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
            .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartner Create(BusinessPartner businessPartner)
        {
            if (context.BusinessPartners.Where(x => x.Identifier != null && x.Identifier == businessPartner.Identifier).Count() == 0)
            {
                businessPartner.Id = 0;

                businessPartner.Active = true;

                context.BusinessPartners.Add(businessPartner);
                return businessPartner;
            }
            else
            {
                // Load item that will be updated
                BusinessPartner dbEntry = context.BusinessPartners
                    .FirstOrDefault(x => x.Identifier == businessPartner.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = businessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartner.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartner.Code;
                    dbEntry.Name = businessPartner.Name;

                    dbEntry.PIB = businessPartner.PIB;
                    dbEntry.PIO = businessPartner.PIO;
                    dbEntry.PDV = businessPartner.PDV;
                    dbEntry.IndustryCode = businessPartner.IndustryCode;
                    dbEntry.IdentificationNumber = businessPartner.IdentificationNumber;

                    dbEntry.Rebate = businessPartner.Rebate;
                    dbEntry.DueDate = businessPartner.DueDate;

                    dbEntry.WebSite = businessPartner.WebSite;
                    dbEntry.ContactPerson = businessPartner.ContactPerson;

                    dbEntry.IsInPDV = businessPartner.IsInPDV;

                    dbEntry.JBKJS = businessPartner.JBKJS;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartner Delete(Guid identifier)
        {
            // Load BusinessPartner that will be deleted
            BusinessPartner dbEntry = context.BusinessPartners
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
