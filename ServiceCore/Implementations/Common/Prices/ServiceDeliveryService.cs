using DataMapper.Mappers.Common.Prices;
using DomainCore.Common.Prices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Prices
{
    public class ServiceDeliveryService : IServiceDeliveryService
    {
        private IUnitOfWork unitOfWork;

        public ServiceDeliveryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ServiceDeliveryListResponse GetServiceDeliverys(int companyId)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response.ServiceDeliverys = unitOfWork.GetServiceDeliveryRepository().GetServiceDeliverys(companyId)
                    .ConvertToServiceDeliveryViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryResponse Create(ServiceDeliveryViewModel re)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                ServiceDelivery addedServiceDelivery = unitOfWork.GetServiceDeliveryRepository().Create(re.ConvertToServiceDelivery());

                unitOfWork.Save();

                response.ServiceDelivery = addedServiceDelivery.ConvertToServiceDeliveryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ServiceDelivery = new ServiceDeliveryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryResponse Delete(Guid identifier)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                ServiceDelivery deletedServiceDelivery = unitOfWork.GetServiceDeliveryRepository().Delete(identifier);

                unitOfWork.Save();

                response.ServiceDelivery = deletedServiceDelivery.ConvertToServiceDeliveryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ServiceDelivery = new ServiceDeliveryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryListResponse Sync(SyncServiceDeliveryRequest request)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ServiceDeliverys.AddRange(unitOfWork.GetServiceDeliveryRepository()
                        .GetServiceDeliverysNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToServiceDeliveryViewModelList() ?? new List<ServiceDeliveryViewModel>());
                }
                else
                {
                    response.ServiceDeliverys.AddRange(unitOfWork.GetServiceDeliveryRepository()
                        .GetServiceDeliverys(request.CompanyId)
                        ?.ConvertToServiceDeliveryViewModelList() ?? new List<ServiceDeliveryViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

