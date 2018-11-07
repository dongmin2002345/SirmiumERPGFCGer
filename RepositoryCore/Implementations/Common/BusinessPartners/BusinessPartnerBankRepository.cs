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
    public class BusinessPartnerBankRepository : IBusinessPartnerBankRepository
    {
        ApplicationDbContext context;

        public BusinessPartnerBankRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanks(int companyId)
        {
            List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
                .Include(x => x.BusinessPartner)
                .Include(x => x.Bank)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return businessPartnerBanks;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanksByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
                .Include(x => x.BusinessPartner)
                .Include(x => x.Bank)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerBanks;
        }

        public List<BusinessPartnerBank> GetBusinessPartnerBanksNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerBank> businessPartnerBanks = context.BusinessPartnerBanks
                .Include(x => x.BusinessPartner)
                .Include(x => x.Bank)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .AsNoTracking()
                .ToList();

            return businessPartnerBanks;
        }

        public BusinessPartnerBank GetBusinessPartnerBank(int id)
        {
            return context.BusinessPartnerBanks
                .Include(x => x.BusinessPartner)
                .Include(x => x.Bank)
                .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerBank Create(BusinessPartnerBank businessPartnerBank)
        {
            if (context.BusinessPartnerBanks.Where(x => x.Identifier != null && x.Identifier == businessPartnerBank.Identifier).Count() == 0)
            {
                businessPartnerBank.Id = 0;

                businessPartnerBank.Active = true;

                context.BusinessPartnerBanks.Add(businessPartnerBank);
                return businessPartnerBank;
            }
            else
            {
                // Load businessPartnerBank that will be updated
                BusinessPartnerBank dbEntry = context.BusinessPartnerBanks
                    .FirstOrDefault(x => x.Identifier == businessPartnerBank.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerBank.BusinessPartnerId ?? null;
                    dbEntry.BankId = businessPartnerBank.BankId ?? null;
                    dbEntry.CountryId = businessPartnerBank.CountryId ?? null;
                    dbEntry.CompanyId = businessPartnerBank.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerBank.CreatedById ?? null;

                    // Set properties
                    dbEntry.AccountNumber = businessPartnerBank.AccountNumber;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerBank Delete(Guid identifier)
        {
            BusinessPartnerBank dbEntry = context.BusinessPartnerBanks
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
