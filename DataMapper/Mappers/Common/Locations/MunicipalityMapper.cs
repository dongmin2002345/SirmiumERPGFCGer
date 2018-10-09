using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Locations
{
    public static class MunicipalityMapper
    {
        public static List<MunicipalityViewModel> ConvertToMunicipalityViewModelList(this IEnumerable<Municipality> municipalities)
        {
            List<MunicipalityViewModel> municipalitiesViewModels = new List<MunicipalityViewModel>();
            foreach (Municipality municipality in municipalities)
            {
                municipalitiesViewModels.Add(municipality.ConvertToMunicipalityViewModel());
            }
            return municipalitiesViewModels;
        }

        public static MunicipalityViewModel ConvertToMunicipalityViewModel(this Municipality municipality)
        {
            MunicipalityViewModel municipalityViewModel = new MunicipalityViewModel()
            {
                Id = municipality.Id,
                Identifier = municipality.Identifier,

                Code = municipality.Code,
                MunicipalityCode = municipality.MunicipalityCode,
                Name = municipality.Name,

                Country = municipality.Country?.ConvertToCountryViewModelLite(),
                Region = municipality.Region?.ConvertToRegionViewModelLite(),

                CreatedBy = municipality.CreatedBy?.ConvertToUserViewModelLite(),
                Company = municipality.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = municipality.UpdatedAt,
                CreatedAt = municipality.CreatedAt
            };
            return municipalityViewModel;
        }

        public static MunicipalityViewModel ConvertToMunicipalityViewModelLite(this Municipality municipality)
        {
            MunicipalityViewModel municipalityViewModel = new MunicipalityViewModel()
            {
                Id = municipality.Id,
                Identifier = municipality.Identifier,

                Code = municipality.Code,
                MunicipalityCode = municipality.MunicipalityCode,
                Name = municipality.Name,

                UpdatedAt = municipality.UpdatedAt,
                CreatedAt = municipality.CreatedAt
            };
            return municipalityViewModel;
        }

        public static Municipality ConvertToMunicipality(this MunicipalityViewModel municipalityViewModel)
        {
            Municipality municipality = new Municipality()
            {
                Id = municipalityViewModel.Id,
                Identifier = municipalityViewModel.Identifier,

                Code = municipalityViewModel.Code,
                MunicipalityCode = municipalityViewModel.MunicipalityCode,
                Name = municipalityViewModel.Name,

                CountryId = municipalityViewModel.Country?.Id ?? null,
                RegionId = municipalityViewModel.Region?.Id ?? null,

                CreatedById = municipalityViewModel.CreatedBy?.Id ?? null,
                CompanyId = municipalityViewModel.Company?.Id ?? null,

                CreatedAt = municipalityViewModel.CreatedAt,
                UpdatedAt = municipalityViewModel.UpdatedAt
            };

            return municipality;
        }
    }
}
