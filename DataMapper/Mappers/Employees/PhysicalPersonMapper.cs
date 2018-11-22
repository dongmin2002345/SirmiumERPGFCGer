using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
	public static class PhysicalPersonMapper
	{
		public static List<PhysicalPersonViewModel> ConvertToPhysicalPersonViewModelList(this IEnumerable<PhysicalPerson> PhysicalPersons)
		{
			List<PhysicalPersonViewModel> PhysicalPersonViewModels = new List<PhysicalPersonViewModel>();
			foreach (PhysicalPerson PhysicalPerson in PhysicalPersons)
			{
				PhysicalPersonViewModels.Add(PhysicalPerson.ConvertToPhysicalPersonViewModel());
			}
			return PhysicalPersonViewModels;
		}

		public static PhysicalPersonViewModel ConvertToPhysicalPersonViewModel(this PhysicalPerson physicalPerson)
		{
			PhysicalPersonViewModel PhysicalPersonViewModel = new PhysicalPersonViewModel()
			{
				Id = physicalPerson.Id,
				Identifier = physicalPerson.Identifier,

				Code = physicalPerson.Code,
				PhysicalPersonCode = physicalPerson.PhysicalPersonCode,
				Name = physicalPerson.Name,
				SurName = physicalPerson.SurName,

				ConstructionSiteCode = physicalPerson.ConstructionSiteCode,
				ConstructionSiteName = physicalPerson.ConstructionSiteName,

				DateOfBirth = physicalPerson.DateOfBirth,
				Gender = physicalPerson.Gender,

				Country = physicalPerson.Country?.ConvertToCountryViewModelLite(),
				Region = physicalPerson.Region?.ConvertToRegionViewModelLite(),
				Municipality = physicalPerson.Municipality?.ConvertToMunicipalityViewModelLite(),
				City = physicalPerson.City?.ConvertToCityViewModelLite(),

				Address = physicalPerson.Address,

				PassportCountry = physicalPerson.PassportCountry?.ConvertToCountryViewModelLite(),
				PassportCity = physicalPerson.PassportCity?.ConvertToCityViewModelLite(),
				Passport = physicalPerson.Passport,
				VisaFrom = physicalPerson.VisaFrom,
				VisaTo = physicalPerson.VisaTo,

				ResidenceCountry = physicalPerson.ResidenceCountry?.ConvertToCountryViewModelLite(),
				ResidenceCity = physicalPerson.ResidenceCity?.ConvertToCityViewModelLite(),
				ResidenceAddress = physicalPerson.ResidenceAddress,

				EmbassyDate = physicalPerson.EmbassyDate,
				VisaDate = physicalPerson.VisaDate,
				VisaValidFrom = physicalPerson.VisaValidFrom,
				VisaValidTo = physicalPerson.VisaValidTo,
				WorkPermitFrom = physicalPerson.WorkPermitFrom,
				WorkPermitTo = physicalPerson.WorkPermitTo,

				IsActive = physicalPerson.Active,

				CreatedBy = physicalPerson.CreatedBy?.ConvertToUserViewModelLite(),
				Company = physicalPerson.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = physicalPerson.UpdatedAt,
				CreatedAt = physicalPerson.CreatedAt
			};

			return PhysicalPersonViewModel;
		}

		public static PhysicalPersonViewModel ConvertToPhysicalPersonViewModelLite(this PhysicalPerson physicalPerson)
		{
			PhysicalPersonViewModel PhysicalPersonViewModel = new PhysicalPersonViewModel()
			{
				Id = physicalPerson.Id,
				Identifier = physicalPerson.Identifier,

				Code = physicalPerson.Code,
				PhysicalPersonCode = physicalPerson.PhysicalPersonCode,
				Name = physicalPerson.Name,
				SurName = physicalPerson.SurName,

				ConstructionSiteCode = physicalPerson.ConstructionSiteCode,
				ConstructionSiteName = physicalPerson.ConstructionSiteName,

				DateOfBirth = physicalPerson.DateOfBirth,

				Address = physicalPerson.Address,
				Passport = physicalPerson.Passport,

				EmbassyDate = physicalPerson.EmbassyDate,
				VisaFrom = physicalPerson.VisaFrom,
				VisaTo = physicalPerson.VisaTo,
				WorkPermitFrom = physicalPerson.WorkPermitFrom,
				WorkPermitTo = physicalPerson.WorkPermitTo,

				IsActive = physicalPerson.Active,

				UpdatedAt = physicalPerson.UpdatedAt,
				CreatedAt = physicalPerson.CreatedAt
			};

			return PhysicalPersonViewModel;
		}

		public static PhysicalPerson ConvertToPhysicalPerson(this PhysicalPersonViewModel physicalPersonViewModel)
		{
			PhysicalPerson PhysicalPerson = new PhysicalPerson()
			{
				Id = physicalPersonViewModel.Id,
				Identifier = physicalPersonViewModel.Identifier,

				Code = physicalPersonViewModel.Code,
				PhysicalPersonCode = physicalPersonViewModel.PhysicalPersonCode,
				Name = physicalPersonViewModel.Name,
				SurName = physicalPersonViewModel.SurName,

				ConstructionSiteCode = physicalPersonViewModel.ConstructionSiteCode,
				ConstructionSiteName = physicalPersonViewModel.ConstructionSiteName,

				DateOfBirth = (DateTime)physicalPersonViewModel.DateOfBirth,
				Gender = physicalPersonViewModel.Gender,
				CountryId = physicalPersonViewModel?.Country?.Id ?? null,
				RegionId = physicalPersonViewModel?.Region?.Id ?? null,
				MunicipalityId = physicalPersonViewModel?.Municipality?.Id ?? null,
				CityId = physicalPersonViewModel?.City?.Id ?? null,
				Address = physicalPersonViewModel.Address,

				PassportCountryId = physicalPersonViewModel?.PassportCountry?.Id ?? null,
				PassportCityId = physicalPersonViewModel?.PassportCity?.Id ?? null,

				Passport = physicalPersonViewModel.Passport,
				VisaFrom = physicalPersonViewModel.VisaFrom,
				VisaTo = physicalPersonViewModel.VisaTo,

				ResidenceCountryId = physicalPersonViewModel?.ResidenceCountry?.Id ?? null,
				ResidenceCityId = physicalPersonViewModel?.ResidenceCity?.Id ?? null,
				ResidenceAddress = physicalPersonViewModel.ResidenceAddress,

				EmbassyDate = physicalPersonViewModel.EmbassyDate,
				VisaDate = physicalPersonViewModel.VisaDate,
				VisaValidFrom = physicalPersonViewModel.VisaValidFrom,
				VisaValidTo = physicalPersonViewModel.VisaValidTo,
				WorkPermitFrom = physicalPersonViewModel.WorkPermitFrom,
				WorkPermitTo = physicalPersonViewModel.WorkPermitTo,

				CreatedById = physicalPersonViewModel.CreatedBy?.Id ?? null,
				CompanyId = physicalPersonViewModel.Company?.Id ?? null,

				CreatedAt = physicalPersonViewModel.CreatedAt,
				UpdatedAt = physicalPersonViewModel.UpdatedAt
			};

			return PhysicalPerson;
		}
	}
}
