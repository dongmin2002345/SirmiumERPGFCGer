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

        public List<BusinessPartner> GetBusinessPartners()
        {
            List<BusinessPartner> BusinessPartners = context.BusinessPartners
                .Include("CreatedBy")
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartners;
        }

        public List<BusinessPartner> GetBusinessPartnersNewerThen(DateTime lastUpdateTime)
        {
            List<BusinessPartner> BusinessPartners = context.BusinessPartners
                .Include(x => x.CreatedBy)
                .Where(x => x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderBy(x => x.Code)
                .Take(20)
                .AsNoTracking()
                .ToList();

            return BusinessPartners;
        }

        public BusinessPartner GetBusinessPartner(int id)
        {
            return context.BusinessPartners
                .Include("CreatedBy")
                .OrderByDescending(x => x.UpdatedAt)
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
                // Load businessPartner that will be updated
                BusinessPartner dbEntry = context.BusinessPartners
                .FirstOrDefault(x => x.Identifier == businessPartner.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CreatedById = businessPartner.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartner.Code;
                    dbEntry.Name = businessPartner.Name;

                    dbEntry.Director = businessPartner.Director;

                    dbEntry.Address = businessPartner.Address;
                    dbEntry.InoAddress = businessPartner.InoAddress;

                    dbEntry.PIB = businessPartner.PIB;
                    dbEntry.MatCode = businessPartner.MatCode;

                    dbEntry.Mobile = businessPartner.Mobile;
                    dbEntry.Phone = businessPartner.Phone;
                    dbEntry.Email = businessPartner.Email;

                    dbEntry.ActivityCode = businessPartner.ActivityCode;

                    dbEntry.BankAccountNumber = businessPartner.BankAccountNumber;

                    dbEntry.OpeningDate = businessPartner.OpeningDate;
                    dbEntry.BranchOpeningDate = businessPartner.BranchOpeningDate;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartner Delete(Guid identifier)
        {
            // Load business partner that will be deleted
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
