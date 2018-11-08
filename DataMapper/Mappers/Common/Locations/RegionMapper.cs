using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Locations
{
    public static class RegionMapper
    {
        public static List<RegionViewModel> ConvertToRegionViewModelList(this IEnumerable<Region> Regions)
        {
            List<RegionViewModel> viewModels = new List<RegionViewModel>();
            foreach (Region Region in Regions)
            {
                viewModels.Add(Region.ConvertToRegionViewModel());
            }
            return viewModels;
        }


        public static RegionViewModel ConvertToRegionViewModel(this Region region)
        {
            RegionViewModel regionViewModel = new RegionViewModel()
            {
                Id = region.Id,
                Identifier = region.Identifier,

                Code = region.Code,
                RegionCode = region.RegionCode,
                Name = region.Name,

                Country = region.Country?.ConvertToCountryViewModelLite(),

                IsActive = region.Active,

                CreatedBy = region.CreatedBy?.ConvertToUserViewModelLite(),
                Company = region.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = region.UpdatedAt,
                CreatedAt = region.CreatedAt
            };



            return regionViewModel;
        }

        public static List<RegionViewModel> ConvertToRegionViewModelListLite(this IEnumerable<Region> regions)
        {
            List<RegionViewModel> regionViewModels = new List<RegionViewModel>();
            foreach (Region region in regions)
            {
                regionViewModels.Add(region.ConvertToRegionViewModelLite());
            }
            return regionViewModels;
        }

        public static RegionViewModel ConvertToRegionViewModelLite(this Region region)
        {
            RegionViewModel regionViewModel = new RegionViewModel()
            {
                Id = region.Id,
                Identifier = region.Identifier,

                Code = region.Code,
                RegionCode = region.RegionCode,
                Name = region.Name,

                IsActive = region.Active,

                UpdatedAt = region.UpdatedAt,
                CreatedAt = region.CreatedAt
            };


            return regionViewModel;
        }

        public static Region ConvertToRegion(this RegionViewModel regionViewModel)
        {
            Region region = new Region()
            {
                Id = regionViewModel.Id,
                Identifier = regionViewModel.Identifier,

                Code = regionViewModel.Code,
                RegionCode = regionViewModel.RegionCode ?? "",
                Name = regionViewModel.Name,

                CountryId = regionViewModel.Country?.Id ?? null,
                CreatedById = regionViewModel.CreatedBy?.Id ?? null,
                CompanyId = regionViewModel.Company?.Id ?? null,

                CreatedAt = regionViewModel.CreatedAt,
                UpdatedAt = regionViewModel.UpdatedAt
            };

            return region;
        }
    }
}
