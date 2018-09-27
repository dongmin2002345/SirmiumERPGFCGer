using DomainCore.Common.Individuals;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepositoryCore.Abstractions.Common.Individuals;
using RepositoryCore.Context;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Individuals
{
    public class IndividualRepository : IIndividualsRepository
    {
        private ApplicationDbContext context;

        public IndividualRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Individual> GetIndividuals(string filterString)
        {
            return context.Individuals
                .Include("CreatedBy")
                .Where(x => String.IsNullOrEmpty(filterString) || x.Name.ToLower().Contains(filterString.ToLower()) || x.Code.ToString().Contains(filterString.ToLower()))
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .ToList();
        }

        public List<Individual> GetIndividualsForPopup(string filterString)
        {
            return context.Individuals
                .Where(x => String.IsNullOrEmpty(filterString) || x.Name.ToLower().Contains(filterString.ToLower()) || x.Code.ToString().Contains(filterString.ToLower()))
                .OrderBy(x => x.Code)
                .Take(20)
                .AsNoTracking()
                .ToList();
        }

        public Individual GetIndividual(int id)
        {
            return context.Individuals
                .Include("CreatedBy")
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public List<Individual> GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 20, string individualName = "")
        {
            IndividualViewModel filterObject = JsonConvert.DeserializeObject<IndividualViewModel>(individualName);

            List<Individual> individuals = context.Individuals
                .Include("CreatedBy")
                .Where(x => x.Active == true)
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Code) || x.Code.ToString().ToLower().Contains(filterObject.SearchBy_Code.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Name) || x.Name.ToLower().Contains(filterObject.SearchBy_Name.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_SurName) || x.SurName.ToLower().Contains(filterObject.SearchBy_SurName.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Interest) || x.Interest.ToLower().Contains(filterObject.SearchBy_Interest.ToLower()))
                .Where(x => filterObject == null || filterObject.SearchBy_VisaFrom == null || x.VisaFrom.Date >= filterObject.SearchBy_VisaFrom)
                .Where(x => filterObject == null || filterObject.SearchBy_VisaTo == null || x.VisaTo.Date <= filterObject.SearchBy_VisaTo)
                .Where(x => filterObject == null || filterObject.SearchBy_PermitFrom == null || x.WorkPermitFrom.Date >= filterObject.SearchBy_PermitFrom)
                .Where(x => filterObject == null || filterObject.SearchBy_PermitTo == null || x.WorkPermitTo.Date <= filterObject.SearchBy_PermitTo)
                .OrderBy(x => x.Id)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .AsNoTracking()
                .ToList();

            return individuals;
        }

        public int GetIndividualsCount(string searchParameter = "")
        {
            IndividualViewModel filterObject = JsonConvert.DeserializeObject<IndividualViewModel>(searchParameter);

            return context.Individuals
                .Where(x => x.Active == true)
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Code) || x.Code.ToString().ToLower().Contains(filterObject.SearchBy_Code.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Name) || x.Name.ToLower().Contains(filterObject.SearchBy_Name.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_SurName) || x.SurName.ToLower().Contains(filterObject.SearchBy_SurName.ToLower()))
                .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Interest) || x.Interest.ToLower().Contains(filterObject.SearchBy_Interest.ToLower()))
                .Where(x => filterObject == null || filterObject.SearchBy_VisaFrom == null || x.VisaFrom.Date >= filterObject.SearchBy_VisaFrom)
                .Where(x => filterObject == null || filterObject.SearchBy_VisaTo == null || x.VisaTo.Date <= filterObject.SearchBy_VisaTo)
                .Where(x => filterObject == null || filterObject.SearchBy_PermitFrom == null || x.WorkPermitFrom.Date >= filterObject.SearchBy_PermitFrom)
                .Where(x => filterObject == null || filterObject.SearchBy_PermitTo == null || x.WorkPermitTo.Date <= filterObject.SearchBy_PermitTo)
                .Count();
        }

        public int GetNewCodeValue()
        {
            var individualID = context.Individuals.Max(x => (int?)x.Id);
            var maxId = (individualID == null ? 0 : individualID);
            var newId = maxId + 1;
            if (newId < 1000)
                newId = 1000 + newId;
            return (int)newId;
        }

        public Individual Create(Individual individual)
        {
            // Attach user
            individual.CreatedBy = context.Users
                .FirstOrDefault(x => x.Id == individual.CreatedBy.Id && x.Active == true);

            //// Attach company
            //Individual.Company = context.Companies
            //    .FirstOrDefault(x => x.Id == Individual.Company.Id && x.Active == true);

            // Set activity
            individual.Active = true;

            // Set timestamps
            individual.CreatedAt = DateTime.Now;
            individual.UpdatedAt = DateTime.Now;

            // Add business partner to database
            context.Individuals.Add(individual);

            return individual;
        }

        public Individual Update(Individual individual)
        {
            // Load business partner that will be updated
            Individual dbEntry = context.Individuals
                .FirstOrDefault(x => x.Id == individual.Id && x.Active == true);

            if (dbEntry != null)
            {
                // Attach user
                dbEntry.CreatedBy = context.Users
                .FirstOrDefault(x => x.Id == individual.CreatedBy.Id && x.Active == true);

                //// Attach company
                //dbEntry.Company = context.Companies
                //    .FirstOrDefault(x => x.Id == Individual.Company.Id && x.Active == true);

                // Set properties 
                dbEntry.Code = individual.Code;
                dbEntry.Name = individual.Name;
                dbEntry.SurName = individual.SurName;

                dbEntry.DateOfBirth = individual.DateOfBirth;

                dbEntry.Address = individual.Address;
                dbEntry.Passport = individual.Passport;
                dbEntry.Interest = individual.Interest;
                dbEntry.License = individual.License;

                dbEntry.EmbassyDate = individual.EmbassyDate;
                dbEntry.VisaFrom = individual.VisaFrom;
                dbEntry.VisaTo = individual.VisaTo;
                dbEntry.WorkPermitFrom = individual.WorkPermitFrom;
                dbEntry.WorkPermitTo = individual.WorkPermitTo;

                dbEntry.Family = individual.Family;

                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }

        public Individual Delete(int id)
        {
            // Load business partner that will be deleted
            Individual dbEntry = context.Individuals
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
