﻿using DomainCore.Common.BusinessPartners;
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
                .Include(x => x.Country)
                .Include(x => x.Sector)
                .Include(x => x.Agency)
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
                .Include(x => x.Country)
                .Include(x => x.Sector)
                .Include(x => x.Agency)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return businessPartners;
        }

        public BusinessPartner GetBusinessPartner(int id)
        {
            return context.BusinessPartners
                .Include(x => x.Country)
                .Include(x => x.Sector)
                .Include(x => x.Agency)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
            .FirstOrDefault(x => x.Id == id && x.Active == true);
        }


        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartners
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartner))
                    .Select(x => x.Entity as BusinessPartner))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "BP/00001";
            else
            {
                string activeCode = context.BusinessPartners
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartner))
                        .Select(x => x.Entity as BusinessPartner))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("BP/", ""));
                    return "BP/" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }


        public BusinessPartner Create(BusinessPartner businessPartner)
        {
            if (context.BusinessPartners.Where(x => x.Identifier != null && x.Identifier == businessPartner.Identifier).Count() == 0)
            {
                if (context.BusinessPartners.Where(x => x.InternalCode == businessPartner.InternalCode).Count() > 0)
                    throw new Exception("Firma sa datom šifrom već postoji u bazi! / Eine Firma mit einem bestimmten Code ist bereits in der Datenbank vorhanden");

                businessPartner.Id = 0;

                businessPartner.Active = true;

                businessPartner.Code = GetNewCodeValue(businessPartner.CompanyId ?? 0);

                businessPartner.CreatedAt = DateTime.Now;
                businessPartner.UpdatedAt = DateTime.Now;

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
                    dbEntry.CountryId = businessPartner.CountryId ?? null;
                    dbEntry.SectorId = businessPartner.SectorId ?? null;
                    dbEntry.AgencyId = businessPartner.AgencyId ?? null;
                    dbEntry.CompanyId = businessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartner.CreatedById ?? null;
                    dbEntry.TaxAdministrationId = businessPartner.TaxAdministrationId ?? null;

                    // Set properties
                    dbEntry.Code = businessPartner.Code;
                    dbEntry.InternalCode = businessPartner.InternalCode;
                    dbEntry.Name = businessPartner.Name;

                    dbEntry.PIB = businessPartner.PIB;
                    dbEntry.PIO = businessPartner.PIO;
                    dbEntry.PDV = businessPartner.PDV;
                    dbEntry.IdentificationNumber = businessPartner.IdentificationNumber;

                    // Set GER properties
                    dbEntry.NameGer = businessPartner.NameGer;

                    dbEntry.IsInPDVGer = businessPartner.IsInPDVGer;
                    dbEntry.IBAN = businessPartner.IBAN;
                    dbEntry.BetriebsNumber = businessPartner.BetriebsNumber;

                    dbEntry.TaxNr = businessPartner.TaxNr;
                    dbEntry.CommercialNr = businessPartner.CommercialNr;
                    dbEntry.ContactPersonGer = businessPartner.ContactPersonGer;

                    dbEntry.Rebate = businessPartner.Rebate;
                    dbEntry.DueDate = businessPartner.DueDate;

                    dbEntry.WebSite = businessPartner.WebSite;
                    dbEntry.ContactPerson = businessPartner.ContactPerson;

                    dbEntry.IsInPDV = businessPartner.IsInPDV;

                    dbEntry.JBKJS = businessPartner.JBKJS;

                    dbEntry.VatDeductionFrom = businessPartner.VatDeductionFrom;
                    dbEntry.VatDeductionTo = businessPartner.VatDeductionTo;

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
