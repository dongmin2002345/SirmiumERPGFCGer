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

        public Company GetCompany(int id)
        {
            return context.Companies
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        //public Company GetFromDictionary(int userId, int companyId)
        //{
        //    lock (CompanyDictionary)
        //    {
        //        if (CompanyDictionary.ContainsKey(userId))
        //        {
        //            if (CompanyDictionary[userId].ContainsKey(companyId))
        //            {
        //                return CompanyDictionary[userId][companyId];
        //            }
        //            else
        //            {
        //                Company company = GetCompany(companyId);
        //                ConcurrentDictionary<int, Company> tmpDictionary = CompanyDictionary[userId];
        //                tmpDictionary.TryAdd(companyId, company);
        //                CompanyDictionary[userId] = tmpDictionary;

        //                return CompanyDictionary[userId][companyId];
        //            }
        //        }
        //        else
        //        {
        //            Company company = GetCompany(companyId);
        //            ConcurrentDictionary<int, Company> tmpDictionary = new ConcurrentDictionary<int, Company>();
        //            tmpDictionary.TryAdd(companyId, company);
        //            CompanyDictionary.TryAdd(userId, tmpDictionary);

        //            return CompanyDictionary[userId][companyId];
        //        }
        //    }
        //}

        //public void ClearDictionary(int userId)
        //{
        //    if (CompanyDictionary.ContainsKey(userId))
        //        CompanyDictionary[userId] = new ConcurrentDictionary<int, Company>();
        //}

        public int GetNewCodeValue()
        {
            var companyID = context.Companies.Max(x => (int?)x.Id);
            var maxId = (companyID == null ? 0 : companyID);
            var newId = maxId + 1;
            if (newId < 1000)
                newId = 1000 + newId;
            return (int)newId;
        }

        public Company Create(Company company)
        {
            // Set activity
            company.Active = true;

            // Set timestamps
            company.CreatedAt = DateTime.Now;
            company.UpdatedAt = DateTime.Now;

            // Add Company to database
            context.Companies.Add(company);
            return company;
        }

        public Company Update(Company company)
        {
            // Load Company that will be updated
            Company dbEntry = context.Companies
                .FirstOrDefault(x => x.Id == company.Id && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Code = company.Code;
                dbEntry.Name = company.Name;
                dbEntry.Address = company.Address;
                dbEntry.BankAccountNo = company.BankAccountNo;
                dbEntry.BankAccountName = company.BankAccountName;
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
    }
}
