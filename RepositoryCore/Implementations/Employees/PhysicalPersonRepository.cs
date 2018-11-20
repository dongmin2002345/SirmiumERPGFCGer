using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
	public class PhysicalPersonRepository : IPhysicalPersonRepository
	{
		private ApplicationDbContext context;

		public PhysicalPersonRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region GET methods

		public List<PhysicalPerson> GetPhysicalPersons(int companyId)
		{
			return context.PhysicalPersons
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Include(x => x.Country)
				.Include(x => x.Region)
				.Include(x => x.Municipality)
				.Include(x => x.City)
				.Include(x => x.PassportCountry)
				.Include(x => x.PassportCity)
				.Include(x => x.ResidenceCountry)
				.Include(x => x.ResidenceCity)
				.Where(x => x.Company.Id == companyId && x.Active == true)
				.AsNoTracking()
				.ToList();
		}

		public PhysicalPerson GetPhysicalPerson(int physicalPersonId)
		{
			return context.PhysicalPersons
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Include(x => x.Country)
				.Include(x => x.Region)
				.Include(x => x.Municipality)
				.Include(x => x.City)
				.Include(x => x.PassportCountry)
				.Include(x => x.PassportCity)
				.Include(x => x.ResidenceCountry)
				.Include(x => x.ResidenceCity)
				.FirstOrDefault(x => x.Id == physicalPersonId && x.Active == true);
		}

		public List<PhysicalPerson> GetPhysicalPersonsNewerThen(int companyId, DateTime lastUpdateTime)
		{
			return context.PhysicalPersons
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Include(x => x.Country)
				.Include(x => x.Region)
				.Include(x => x.Municipality)
				.Include(x => x.City)
				.Include(x => x.PassportCountry)
				.Include(x => x.PassportCity)
				.Include(x => x.ResidenceCountry)
				.Include(x => x.ResidenceCity)
				.Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
				.AsNoTracking()
				.ToList();
		}

		private string GetNewCodeValue(int companyId)
		{
			int count = context.PhysicalPersons
				.Union(context.ChangeTracker.Entries()
					.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPerson))
					.Select(x => x.Entity as PhysicalPerson))
				.Where(x => x.CompanyId == companyId).Count();
			if (count == 0)
				return "F-LICA-00001";
			else
			{
				string activeCode = context.PhysicalPersons
					.Union(context.ChangeTracker.Entries()
						.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPerson))
						.Select(x => x.Entity as PhysicalPerson))
					.Where(x => x.CompanyId == companyId)
					.OrderByDescending(x => x.Id).FirstOrDefault()
					.Code;
				if (!String.IsNullOrEmpty(activeCode))
				{
					int intValue = Int32.Parse(activeCode.Replace("F-LICA-", ""));
					return "F-LICA-" + (intValue + 1).ToString("00000");
				}
				else
					return "";
			}
		}

		#endregion

		#region CREATE methods

		public PhysicalPerson Create(PhysicalPerson physicalPerson)
		{
			if (context.PhysicalPersons.Where(x => x.Identifier != null && x.Identifier == physicalPerson.Identifier).Count() == 0)
			{
				physicalPerson.Id = 0;

				physicalPerson.Code = GetNewCodeValue(physicalPerson.CompanyId ?? 0);
				physicalPerson.Active = true;

				physicalPerson.UpdatedAt = DateTime.Now;
				physicalPerson.CreatedAt = DateTime.Now;

				context.PhysicalPersons.Add(physicalPerson);
				return physicalPerson;
			}
			else
			{
				// Load item that will be updated
				PhysicalPerson dbEntry = context.PhysicalPersons
					.FirstOrDefault(x => x.Identifier == physicalPerson.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.CompanyId = physicalPerson.CompanyId ?? null;
					dbEntry.CreatedById = physicalPerson.CreatedById ?? null;

					// Set properties
					dbEntry.Code = physicalPerson.Code;
					dbEntry.PhysicalPersonCode = physicalPerson.PhysicalPersonCode;
					dbEntry.Name = physicalPerson.Name;
					dbEntry.SurName = physicalPerson.SurName;

					dbEntry.ConstructionSiteCode = physicalPerson.ConstructionSiteCode;
					dbEntry.ConstructionSiteName = physicalPerson.ConstructionSiteName;

					dbEntry.DateOfBirth = physicalPerson.DateOfBirth;
					dbEntry.Gender = physicalPerson.Gender;
					dbEntry.CountryId = physicalPerson.CountryId;
					dbEntry.RegionId = physicalPerson.RegionId;
					dbEntry.MunicipalityId = physicalPerson.MunicipalityId;
					dbEntry.CityId = physicalPerson.CityId;
					dbEntry.Address = physicalPerson.Address;

					dbEntry.Passport = physicalPerson.Passport;
					dbEntry.VisaFrom = physicalPerson.VisaFrom;
					dbEntry.VisaTo = physicalPerson.VisaTo;

					dbEntry.PassportCountryId = physicalPerson.PassportCountryId;
					dbEntry.PassportCityId = physicalPerson.PassportCityId;

					dbEntry.ResidenceCountryId = physicalPerson.ResidenceCountryId;
					dbEntry.ResidenceCityId = physicalPerson.ResidenceCityId;
					dbEntry.ResidenceAddress = physicalPerson.ResidenceAddress;

					dbEntry.EmbassyDate = physicalPerson.EmbassyDate;
					dbEntry.VisaDate = physicalPerson.VisaDate;
					dbEntry.VisaValidFrom = physicalPerson.VisaValidFrom;
					dbEntry.VisaValidTo = physicalPerson.VisaValidTo;
					dbEntry.WorkPermitFrom = physicalPerson.WorkPermitFrom;
					dbEntry.WorkPermitTo = physicalPerson.WorkPermitTo;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public PhysicalPerson Delete(Guid identifier)
		{
			// Load Employee that will be deleted
			PhysicalPerson dbEntry = context.PhysicalPersons
				.FirstOrDefault(x => x.Identifier == identifier);

			if (dbEntry != null)
			{
				// Set activity
				dbEntry.Active = false;
				// Set timestamp
				dbEntry.UpdatedAt = DateTime.Now;
			}

			return dbEntry;
		}

		#endregion
	}
}
