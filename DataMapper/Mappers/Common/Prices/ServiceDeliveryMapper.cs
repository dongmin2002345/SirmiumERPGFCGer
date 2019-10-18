using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Prices
{
    public static class ServiceDeliveryMapper
    {
        public static List<ServiceDeliveryViewModel> ConvertToServiceDeliveryViewModelList(this IEnumerable<ServiceDelivery> serviceDeliverys)
        {
            List<ServiceDeliveryViewModel> serviceDeliveryViewModels = new List<ServiceDeliveryViewModel>();
            foreach (ServiceDelivery serviceDelivery in serviceDeliverys)
            {
                serviceDeliveryViewModels.Add(serviceDelivery.ConvertToServiceDeliveryViewModel());
            }
            return serviceDeliveryViewModels;
        }

        public static ServiceDeliveryViewModel ConvertToServiceDeliveryViewModel(this ServiceDelivery serviceDelivery)
        {
            ServiceDeliveryViewModel serviceDeliveryViewModel = new ServiceDeliveryViewModel()
            {
                Id = serviceDelivery.Id,
                Identifier = serviceDelivery.Identifier,

                Code = serviceDelivery.Code,
                InternalCode = serviceDelivery.InternalCode,
                Name = serviceDelivery.Name,
                URL = serviceDelivery.URL,

                IsActive = serviceDelivery.Active,

                CreatedBy = serviceDelivery.CreatedBy?.ConvertToUserViewModelLite(),
                Company = serviceDelivery.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = serviceDelivery.UpdatedAt,
                CreatedAt = serviceDelivery.CreatedAt,
            };

            return serviceDeliveryViewModel;
        }

        public static ServiceDeliveryViewModel ConvertToServiceDeliveryViewModelLite(this ServiceDelivery serviceDelivery)
        {
            ServiceDeliveryViewModel serviceDeliveryViewModel = new ServiceDeliveryViewModel()
            {
                Id = serviceDelivery.Id,
                Identifier = serviceDelivery.Identifier,

                Code = serviceDelivery.Code,
                InternalCode = serviceDelivery.InternalCode,
                Name = serviceDelivery.Name,
                URL = serviceDelivery.URL,

                IsActive = serviceDelivery.Active,

                UpdatedAt = serviceDelivery.UpdatedAt,
                CreatedAt = serviceDelivery.CreatedAt
            };

            return serviceDeliveryViewModel;
        }

        public static ServiceDelivery ConvertToServiceDelivery(this ServiceDeliveryViewModel serviceDeliveryViewModel)
        {
            ServiceDelivery serviceDelivery = new ServiceDelivery()
            {
                Id = serviceDeliveryViewModel.Id,
                Identifier = serviceDeliveryViewModel.Identifier,

                Code = serviceDeliveryViewModel.Code,
                InternalCode = serviceDeliveryViewModel.InternalCode,
                Name = serviceDeliveryViewModel.Name,
                URL = serviceDeliveryViewModel.URL,

                Active = serviceDeliveryViewModel.IsActive,

                CreatedById = serviceDeliveryViewModel.CreatedBy?.Id ?? null,
                CompanyId = serviceDeliveryViewModel.Company?.Id ?? null,

                CreatedAt = serviceDeliveryViewModel.CreatedAt,
                UpdatedAt = serviceDeliveryViewModel.UpdatedAt,
            };

            return serviceDelivery;
        }
    }
}
