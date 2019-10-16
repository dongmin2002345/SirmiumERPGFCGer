using DataMapper.Mappers.Common.Shipments;
using DomainCore.Common.Shipments;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.Shipments
{
    public class ShipmentService : IShipmentService
    {
        IUnitOfWork unitOfWork;

        public ShipmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ShipmentListResponse GetShipments(int companyId)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response.Shipments = unitOfWork.GetShipmentRepository().GetShipments(companyId)
               .ConvertToShipmentViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentListResponse GetShipmentsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Shipments = unitOfWork.GetShipmentRepository()
                        .GetShipmentsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToShipmentViewModelList();
                }
                else
                {
                    response.Shipments = unitOfWork.GetShipmentRepository()
                        .GetShipments(companyId)
                        .ConvertToShipmentViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentResponse Create(ShipmentViewModel re)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
           
                // Backup documents
                List<ShipmentDocumentViewModel> shipmentDocuments = re.ShipmentDocuments?.ToList();
                re.ShipmentDocuments = null;

                Shipment createdShipment = unitOfWork.GetShipmentRepository().Create(re.ConvertToShipment());

                

                // Update documents
                if (shipmentDocuments != null && shipmentDocuments.Count > 0)
                {
                    // Items for create or update
                    foreach (var shipmentDocument in shipmentDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<ShipmentDocumentViewModel>())
                    {
                        shipmentDocument.Shipment = new ShipmentViewModel() { Id = createdShipment.Id };
                        shipmentDocument.ItemStatus = ItemStatus.Submited;
                        ShipmentDocument createdShipmentDocument = unitOfWork.GetShipmentDocumentRepository()
                            .Create(shipmentDocument.ConvertToShipmentDocument());
                    }

                    foreach (var item in shipmentDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<ShipmentDocumentViewModel>())
                    {
                        item.Shipment = new ShipmentViewModel() { Id = createdShipment.Id };
                        unitOfWork.GetShipmentDocumentRepository().Create(item.ConvertToShipmentDocument());

                        unitOfWork.GetShipmentDocumentRepository().Delete(item.Identifier);
                    }
                }

                unitOfWork.Save();

                response.Shipment = createdShipment.ConvertToShipmentViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Shipment = new ShipmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentResponse Delete(Guid identifier)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
                DomainCore.Common.Shipments.Shipment deletedShipment = unitOfWork.GetShipmentRepository().Delete(identifier);

                unitOfWork.Save();

                response.Shipment = deletedShipment.ConvertToShipmentViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Shipment = new ShipmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentListResponse Sync(SyncShipmentRequest request)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response.Shipments = new List<ShipmentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Shipments.AddRange(unitOfWork.GetShipmentRepository()
                        .GetShipmentsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToShipmentViewModelList() ?? new List<ShipmentViewModel>());
                }
                else
                {
                    response.Shipments.AddRange(unitOfWork.GetShipmentRepository()
                        .GetShipments(request.CompanyId)
                        ?.ConvertToShipmentViewModelList() ?? new List<ShipmentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
