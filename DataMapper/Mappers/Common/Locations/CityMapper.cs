using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Locations
{
    public static class CityMapper
    {
        public static List<CityViewModel> ConvertToCityViewModelList(this IEnumerable<City> cities)
        {
            List<CityViewModel> citiesViewModels = new List<CityViewModel>();
            foreach (City city in cities)
            {
                citiesViewModels.Add(city.ConvertToCityViewModel());
            }
            return citiesViewModels;
        }

        public static CityViewModel ConvertToCityViewModel(this City city)
        {
            CityViewModel cityViewModel = new CityViewModel()
            {
                Id = city.Id,
                Identifier = city.Identifier,

                Code = city.Code,
                ZipCode = city.ZipCode,
                Name = city.Name,

                Country = city.Country?.ConvertToCountryViewModelLite(),
                Region = city.Region?.ConvertToRegionViewModelLite(),
                Municipality = city.Municipality?.ConvertToMunicipalityViewModelLite(),

                IsActive = city.Active,

                CreatedBy = city.CreatedBy?.ConvertToUserViewModelLite(),
                Company = city.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = city.UpdatedAt,
                CreatedAt = city.CreatedAt

            };
            return cityViewModel;
        }

        public static List<CityViewModel> ConvertToCityViewModelListLite(this IEnumerable<City> cities)
        {
            List<CityViewModel> cityViewModels = new List<CityViewModel>();
            foreach (City remedy in cities)
            {
                cityViewModels.Add(remedy.ConvertToCityViewModelLite());
            }
            return cityViewModels;
        }


        public static CityViewModel ConvertToCityViewModelLite(this City city)
        {
            CityViewModel cityViewModel = new CityViewModel()
            {
                Id = city.Id,
                Identifier = city.Identifier,

                Code = city.Code,
                ZipCode = city.ZipCode,
                Name = city.Name,

                IsActive = city.Active,

                CreatedAt = city.CreatedAt,
                UpdatedAt = city.UpdatedAt
            };
            return cityViewModel;
        }

        public static City ConvertToCity(this CityViewModel cityViewModel)
        {
            City city = new City()
            {
                Id = cityViewModel.Id,
                Identifier = cityViewModel.Identifier,

                Code = cityViewModel.Code,
                ZipCode = cityViewModel.ZipCode ?? "",
                Name = cityViewModel.Name,

                CountryId = cityViewModel.Country?.Id ?? null,
                RegionId = cityViewModel.Region?.Id ?? null,
                MunicipalityId = cityViewModel.Municipality?.Id ?? null,

                Active = cityViewModel.IsActive, 

                CreatedById = cityViewModel.CreatedBy?.Id ?? null,
                CompanyId = cityViewModel.Company?.Id ?? null,

                CreatedAt = cityViewModel.CreatedAt,
                UpdatedAt = cityViewModel.UpdatedAt

            };
            return city;
        }
    }
}

