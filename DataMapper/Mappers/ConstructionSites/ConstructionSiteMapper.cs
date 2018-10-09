﻿using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.ConstructionSites
{
    public static class ConstructionSiteMapper
    {
        public static List<ConstructionSiteViewModel> ConvertToConstructionSiteViewModelList(this IEnumerable<ConstructionSite> constructionSites)
        {
            List<ConstructionSiteViewModel> constructionSitesViewModels = new List<ConstructionSiteViewModel>();
            foreach (ConstructionSite constructionSite in constructionSites)
            {
                constructionSitesViewModels.Add(constructionSite.ConvertToConstructionSiteViewModel());
            }
            return constructionSitesViewModels;
        }

        public static ConstructionSiteViewModel ConvertToConstructionSiteViewModel(this ConstructionSite constructionSite)
        {
            ConstructionSiteViewModel constructionSiteViewModel = new ConstructionSiteViewModel()
            {
                Id = constructionSite.Id,
                Identifier = constructionSite.Identifier,

                Code = constructionSite.Code,
                Name = constructionSite.Name,
                Address = constructionSite.Address,
                MaxWorkers = constructionSite.MaxWorkers,
                ContractExpiration = constructionSite.ContractExpiration,

                City = constructionSite.City?.ConvertToCityViewModelLite(),

                CreatedBy = constructionSite.CreatedBy?.ConvertToUserViewModelLite(),
                Company = constructionSite.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = constructionSite.UpdatedAt,
                CreatedAt = constructionSite.CreatedAt

            };
            return constructionSiteViewModel;
        }

        public static List<ConstructionSiteViewModel> ConvertToConstructionSiteViewModelListLite(this IEnumerable<ConstructionSite> constructionSites)
        {
            List<ConstructionSiteViewModel> constructionSiteViewModels = new List<ConstructionSiteViewModel>();
            foreach (ConstructionSite constructionSite in constructionSites)
            {
                constructionSiteViewModels.Add(constructionSite.ConvertToConstructionSiteViewModelLite());
            }
            return constructionSiteViewModels;
        }


        public static ConstructionSiteViewModel ConvertToConstructionSiteViewModelLite(this ConstructionSite constructionSite)
        {
            ConstructionSiteViewModel constructionSiteViewModel = new ConstructionSiteViewModel()
            {
                Id = constructionSite.Id,
                Identifier = constructionSite.Identifier,

                Code = constructionSite.Code,
                Name = constructionSite.Name,
                Address = constructionSite.Address,
                MaxWorkers = constructionSite.MaxWorkers,
                ContractExpiration = constructionSite.ContractExpiration,

                CreatedAt = constructionSite.CreatedAt,
                UpdatedAt = constructionSite.UpdatedAt
            };
            return constructionSiteViewModel;
        }

        public static ConstructionSite ConvertToConstructionSite(this ConstructionSiteViewModel constructionSiteViewModel)
        {
            ConstructionSite constructionSite = new ConstructionSite()
            {
                Id = constructionSiteViewModel.Id,
                Identifier = constructionSiteViewModel.Identifier,

                Code = constructionSiteViewModel.Code,
                Address = constructionSiteViewModel.Address,
                Name = constructionSiteViewModel.Name,
                MaxWorkers = constructionSiteViewModel.MaxWorkers,
                ContractExpiration = constructionSiteViewModel.ContractExpiration,

                CityId = constructionSiteViewModel.City?.Id ?? null,

                CreatedById = constructionSiteViewModel.CreatedBy?.Id ?? null,
                CompanyId = constructionSiteViewModel.Company?.Id ?? null,

                CreatedAt = constructionSiteViewModel.CreatedAt,
                UpdatedAt = constructionSiteViewModel.UpdatedAt

            };
            return constructionSite;
        }
    }
}
