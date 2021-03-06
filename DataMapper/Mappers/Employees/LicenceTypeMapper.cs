﻿using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class LicenceTypeMapper
    {
		public static List<LicenceTypeViewModel> ConvertToLicenceTypeViewModelList(this IEnumerable<LicenceType> licenceTypes)
		{
			List<LicenceTypeViewModel> licenceTypesViewModels = new List<LicenceTypeViewModel>();
			foreach (LicenceType licenceType in licenceTypes)
			{
				licenceTypesViewModels.Add(licenceType.ConvertToLicenceTypeViewModel());
			}
			return licenceTypesViewModels;
		}

		public static LicenceTypeViewModel ConvertToLicenceTypeViewModel(this LicenceType licenceType)
		{
			LicenceTypeViewModel licenceTypeViewModel = new LicenceTypeViewModel()
			{
				Id = licenceType.Id,
				Identifier = licenceType.Identifier,

				Code = licenceType.Code,
				Category = licenceType.Category,
				Description = licenceType.Description,

				Country = licenceType.Country?.ConvertToCountryViewModelLite(),

                IsActive = licenceType.Active,

                CreatedBy = licenceType.CreatedBy?.ConvertToUserViewModelLite(),
				Company = licenceType.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = licenceType.UpdatedAt,
				CreatedAt = licenceType.CreatedAt

			};
			return licenceTypeViewModel;
		}

		public static List<LicenceTypeViewModel> ConvertToLicenceTypeViewModelListLite(this IEnumerable<LicenceType> licenceTypes)
		{
			List<LicenceTypeViewModel> licenceTypeViewModels = new List<LicenceTypeViewModel>();
			foreach (LicenceType licenceType in licenceTypes)
			{
				licenceTypeViewModels.Add(licenceType.ConvertToLicenceTypeViewModelLite());
			}
			return licenceTypeViewModels;
		}


		public static LicenceTypeViewModel ConvertToLicenceTypeViewModelLite(this LicenceType licenceType)
		{
			LicenceTypeViewModel licenceTypeViewModel = new LicenceTypeViewModel()
			{
				Id = licenceType.Id,
				Identifier = licenceType.Identifier,

				Code = licenceType.Code,
				Category = licenceType.Category,
				Description = licenceType.Description,

				CreatedAt = licenceType.CreatedAt,
				UpdatedAt = licenceType.UpdatedAt
			};
			return licenceTypeViewModel;
		}

		public static LicenceType ConvertToLicenceType(this LicenceTypeViewModel licenceTypeViewModel)
		{
			LicenceType licenceType = new LicenceType()
			{
				Id = licenceTypeViewModel.Id,
				Identifier = licenceTypeViewModel.Identifier,

				Code = licenceTypeViewModel.Code,
				Category = licenceTypeViewModel.Category,
				Description = licenceTypeViewModel.Description,


				CountryId = licenceTypeViewModel.Country?.Id ?? null,
				CreatedById = licenceTypeViewModel.CreatedBy?.Id ?? null,
				CompanyId = licenceTypeViewModel.Company?.Id ?? null,

				CreatedAt = licenceTypeViewModel.CreatedAt,
				UpdatedAt = licenceTypeViewModel.UpdatedAt

			};
			return licenceType;
		}
	}
}
