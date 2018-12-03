using DomainCore.Common.Companies;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        public static ConcurrentDictionary<int, ConcurrentDictionary<int, Company>> CompanyDictionary = new ConcurrentDictionary<int, ConcurrentDictionary<int, Company>>();

        private ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Company> GetCompanies()
        {
            List<Company> Companies = context.Companies
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .ToList();

            return Companies;
        }

        public string GetNewCodeValue()
        {
            var CODE_TEMPLATE = "KOR-";
            int count = context.Companies
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Company))
                    .Select(x => x.Entity as Company))
                    .Count();
            if (count == 0)
                return CODE_TEMPLATE + "000001";
            else
            {
                string activeCode = context.Companies
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Company))
                        .Select(x => x.Entity as Company))
                        .Where(x => !String.IsNullOrEmpty(x.Code))
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault()
                    ?.Code ?? "";

                activeCode = activeCode.Replace(CODE_TEMPLATE, "");
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return CODE_TEMPLATE + (intValue + 1).ToString("000000");
                }
                else
                    return "";
            }
        }

        public Company Create(Company company)
        {
            if (company.Id < 1)
            {
                company.Id = 0;
                company.Code = GetNewCodeValue();

                // Set activity
                company.Active = true;

                // Set timestamps
                company.CreatedAt = DateTime.Now;
                company.UpdatedAt = DateTime.Now;

                // Add Company to database
                context.Companies.Add(company);
                return company;
            }
            else
            {
                // Load Company that will be updated
                Company dbEntry = context.Companies
                    .FirstOrDefault(x => x.Id == company.Id && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.Code = company.Code;
                    dbEntry.Name = company.Name;
                    dbEntry.Address = company.Address;
                    dbEntry.IdentificationNumber = company.IdentificationNumber;
                    dbEntry.PIBNumber = company.PIBNumber;
                    dbEntry.PIONumber = company.PIONumber;
                    dbEntry.PDVNumber = company.PDVNumber;
                    dbEntry.IndustryCode = company.IndustryCode;
                    dbEntry.IndustryName = company.IndustryName;
                    dbEntry.Email = company.Email;
                    dbEntry.WebSite = company.WebSite;

                    dbEntry.UpdatedAt = DateTime.Now;
                }
                return dbEntry;
            }
        }

        public Company Delete(int id)
        {
            // Load Company that will be deleted
            Company dbEntry = context.Companies
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

        public List<Company> GetCompaniesNewerThan(DateTime dateFrom)
        {
            List<Company> Companies = context.Companies
                .Where(x => x.UpdatedAt > dateFrom)
                .ToList();

            return Companies;
        }
    }
}
