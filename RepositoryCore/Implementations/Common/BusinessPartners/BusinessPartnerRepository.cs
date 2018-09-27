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
            return context.BusinessPartners
                .Include("CreatedBy")
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

        }

        public List<BusinessPartner> GetBusinessPartnersForPopup(string filterString)
        {
            return context.BusinessPartners
                .Where(x => String.IsNullOrEmpty(filterString) || x.Name.ToLower().Contains(filterString.ToLower()) || x.Code.ToString().Contains(filterString.ToLower()))
                .OrderBy(x => x.Code)
                .Take(20)
                .AsNoTracking()
                .ToList();
        }

        public BusinessPartner GetBusinessPartner(int id)
        {
            return context.BusinessPartners
                .Include("CreatedBy")
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public List<BusinessPartner> GetBusinessPartnersByPage(int currentPage = 1, int itemsPerPage = 20, string businessPartnerName = "")
        {
            BusinessPartnerViewModel filterObject = JsonConvert.DeserializeObject<BusinessPartnerViewModel>(businessPartnerName);

            List<BusinessPartner> businessPartners = context.BusinessPartners
                .Include("CreatedBy")
                .Where(x => x.Active == true)
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_BusinessPartnerName) || x.Name.ToLower().Contains(filterObject.SearchBy_BusinessPartnerName.ToLower()))
                .OrderBy(x => x.Id)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .AsNoTracking()
                .ToList();

            return businessPartners;
        }

        public int GetBusinessPartnersCount(string searchParameter = "")
        {
            BusinessPartnerViewModel filterObject = JsonConvert.DeserializeObject<BusinessPartnerViewModel>(searchParameter);

            return context.BusinessPartners
                .Where(x => x.Active == true)
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_BusinessPartnerName) || x.Name.ToLower().Contains(filterObject.SearchBy_BusinessPartnerName.ToLower()))
                .Count();
        }

        public int GetNewCodeValue()
        {
            var businessPartnerID = context.BusinessPartners.Max(x => (int?)x.Id);
            var maxId = (businessPartnerID == null ? 0 : businessPartnerID);
            var newId = maxId + 1;
            if (newId < 1000)
                newId = 1000 + newId;
            return (int)newId;
        }

        public BusinessPartner Create(BusinessPartner businessPartner)
        {
            // Attach user
            businessPartner.CreatedBy = context.Users
                .FirstOrDefault(x => x.Id == businessPartner.CreatedBy.Id && x.Active == true);

            //// Attach company
            //businessPartner.Company = context.Companies
            //    .FirstOrDefault(x => x.Id == businessPartner.Company.Id && x.Active == true);

            // Set activity
            businessPartner.Active = true;

            // Set timestamps
            businessPartner.CreatedAt = DateTime.Now;
            businessPartner.UpdatedAt = DateTime.Now;

            // Add business partner to database
            context.BusinessPartners.Add(businessPartner);

            return businessPartner;
        }

        public BusinessPartner Update(BusinessPartner businessPartner)
        {
            // Load business partner that will be updated
            BusinessPartner dbEntry = context.BusinessPartners
                .FirstOrDefault(x => x.Id == businessPartner.Id && x.Active == true);

            if (dbEntry != null)
            {
                // Attach user
                dbEntry.CreatedBy = context.Users
                .FirstOrDefault(x => x.Id == businessPartner.CreatedBy.Id && x.Active == true);

                //// Attach company
                //dbEntry.Company = context.Companies
                //    .FirstOrDefault(x => x.Id == businessPartner.Company.Id && x.Active == true);

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

        public BusinessPartner Delete(int id)
        {
            // Load business partner that will be deleted
            BusinessPartner dbEntry = context.BusinessPartners
                .FirstOrDefault(x => x.Id == id && x.Active == true);

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
