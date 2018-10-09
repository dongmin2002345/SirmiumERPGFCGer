using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Sectors
{
    public static class AgencyMapper
    {
        public static List<AgencyViewModel> ConvertToAgencyViewModelList(this IEnumerable<Agency> Agencies)
        {
            List<AgencyViewModel> AgenciesViewModels = new List<AgencyViewModel>();
            foreach (Agency Agency in Agencies)
            {
                AgenciesViewModels.Add(Agency.ConvertToAgencyViewModel());
            }
            return AgenciesViewModels;
        }

        public static AgencyViewModel ConvertToAgencyViewModel(this Agency Agency)
        {
            AgencyViewModel AgencyViewModel = new AgencyViewModel()
            {
                Id = Agency.Id,
                Identifier = Agency.Identifier,

                Code = Agency.Code,
                Name = Agency.Name,

                Country = Agency.Country?.ConvertToCountryViewModelLite(),
                Sector = Agency.Sector?.ConvertToSectorViewModelLite(),

                CreatedBy = Agency.CreatedBy?.ConvertToUserViewModelLite(),
                Company = Agency.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = Agency.UpdatedAt,
                CreatedAt = Agency.CreatedAt

            };
            return AgencyViewModel;
        }

        public static List<AgencyViewModel> ConvertToAgencyViewModelListLite(this IEnumerable<Agency> Agencies)
        {
            List<AgencyViewModel> AgencyViewModels = new List<AgencyViewModel>();
            foreach (Agency remedy in Agencies)
            {
                AgencyViewModels.Add(remedy.ConvertToAgencyViewModelLite());
            }
            return AgencyViewModels;
        }


        public static AgencyViewModel ConvertToAgencyViewModelLite(this Agency Agency)
        {
            AgencyViewModel AgencyViewModel = new AgencyViewModel()
            {
                Id = Agency.Id,
                Identifier = Agency.Identifier,

                Code = Agency.Code,
                Name = Agency.Name,

                CreatedAt = Agency.CreatedAt,
                UpdatedAt = Agency.UpdatedAt
            };
            return AgencyViewModel;
        }

        public static Agency ConvertToAgency(this AgencyViewModel AgencyViewModel)
        {
            Agency Agency = new Agency()
            {
                Id = AgencyViewModel.Id,
                Identifier = AgencyViewModel.Identifier,

                Code = AgencyViewModel.Code,
                Name = AgencyViewModel.Name,

                CountryId = AgencyViewModel.Country?.Id ?? null,
                SectorId = AgencyViewModel.Sector?.Id ?? null,

                CreatedById = AgencyViewModel.CreatedBy?.Id ?? null,
                CompanyId = AgencyViewModel.Company?.Id ?? null,

                CreatedAt = AgencyViewModel.CreatedAt,
                UpdatedAt = AgencyViewModel.UpdatedAt

            };
            return Agency;
        }
    }
}
