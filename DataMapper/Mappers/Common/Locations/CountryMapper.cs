using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Locations
{
    public static class CountryMapper
    {
        public static List<CountryViewModel> ConvertToCountryViewModelList(this IEnumerable<Country> countries)
        {
            List<CountryViewModel> countryViewModels = new List<CountryViewModel>();
            foreach (Country country in countries)
            {
                countryViewModels.Add(country.ConvertToCountryViewModel());
            }
            return countryViewModels;
        }

        public static CountryViewModel ConvertToCountryViewModel(this Country country)
        {
            CountryViewModel countryViewModel = new CountryViewModel()
            {
                Id = country.Id,
                Identifier = country.Identifier,

                Code = country.Code,
                AlfaCode = country.AlfaCode,
                NumericCode = country.NumericCode,
                Mark = country.Mark,
                Name = country.Name,

                CreatedBy = country.CreatedBy?.ConvertToUserViewModelLite(),
                Company = country.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = country.UpdatedAt,
                CreatedAt = country.CreatedAt
            };



            return countryViewModel;
        }

        public static List<CountryViewModel> ConvertToCountryViewModelListLite(this IEnumerable<Country> countries)
        {
            List<CountryViewModel> countryViewModels = new List<CountryViewModel>();
            foreach (Country country in countries)
            {
                countryViewModels.Add(country.ConvertToCountryViewModelLite());
            }
            return countryViewModels;
        }

        public static CountryViewModel ConvertToCountryViewModelLite(this Country country)
        {
            CountryViewModel countryViewModel = new CountryViewModel()
            {
                Id = country.Id,
                Identifier = country.Identifier,

                Code = country.Code,
                AlfaCode = country.AlfaCode,
                NumericCode = country.NumericCode,
                Mark = country.Mark,
                Name = country.Name,

                UpdatedAt = country.UpdatedAt,
                CreatedAt = country.CreatedAt
            };


            return countryViewModel;
        }

        public static Country ConvertToCountry(this CountryViewModel countryViewModel)
        {
            Country country = new Country()
            {
                Id = countryViewModel.Id,
                Identifier = countryViewModel.Identifier,

                Code = countryViewModel.Code,
                AlfaCode = countryViewModel.AlfaCode,
                NumericCode = countryViewModel.NumericCode,
                Mark = countryViewModel.Mark,
                Name = countryViewModel.Name,

                CreatedById = countryViewModel.CreatedBy?.Id ?? null,
                CompanyId = countryViewModel.Company?.Id ?? null,

                CreatedAt = countryViewModel.CreatedAt,
                UpdatedAt = countryViewModel.UpdatedAt
            };

            return country;
        }
    }
}
