using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Cities;
using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Cities
{
    public static class CityMapper
    {
        public static List<CityViewModel> ConvertToCityViewModelList(this IEnumerable<City> cities)
        {
            List<CityViewModel> CityViewModels = new List<CityViewModel>();

            foreach (City city in cities)
            {
                CityViewModels.Add(city.ConvertToCityViewModel());
            }
            return CityViewModels;
        }

        public static CityViewModel ConvertToCityViewModel(this City city)
        {
            CityViewModel cityViewModel = new CityViewModel()
            {
                Id = city.Id,
                Identifier = city.Identifier,

                Code = city.Code,
                Name = city.Name,

                CreatedBy = city.CreatedBy?.ConvertToUserViewModelLite(),

                UpdatedAt = city.UpdatedAt,
                CreatedAt = city.CreatedAt,
            };
            return cityViewModel;
        }

        public static CityViewModel ConvertToCityViewModelLite(this City City)
        {
            CityViewModel CityViewModel = new CityViewModel()
            {
                Id = City.Id,
                Identifier = City.Identifier,

                Code = City.Code,
                Name = City.Name,

                UpdatedAt = City.UpdatedAt,
                CreatedAt = City.CreatedAt,
            };
            return CityViewModel;
        }

        public static City ConvertToCity(this CityViewModel CityViewModel)
        {
            City City = new City()
            {
                Id = CityViewModel.Id,
                Identifier = CityViewModel.Identifier,

                Code = CityViewModel.Code,
                Name = CityViewModel.Name,

                CreatedById = CityViewModel.CreatedBy?.Id ?? null,
                CompanyId = CityViewModel.Company?.Id ?? null,

                UpdatedAt = CityViewModel.UpdatedAt,
                CreatedAt = CityViewModel.CreatedAt,
            };
            return City;
        }
    }
}

