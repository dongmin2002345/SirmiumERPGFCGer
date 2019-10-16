using DataMapper.Mappers.Common.Shipments;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Shipments
{
    public class ShipmentDocumentService : IShipmentDocumentService
    {
        IUnitOfWork unitOfWork;

        public ShipmentDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ShipmentDocumentListResponse Sync(SyncShipmentDocumentRequest request)
        {
            ShipmentDocumentListResponse response = new ShipmentDocumentListResponse();
            try
            {
                response.ShipmentDocuments = new List<ShipmentDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ShipmentDocuments.AddRange(unitOfWork.GetShipmentDocumentRepository()
                        .GetShipmentDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToShipmentDocumentViewModelList() ?? new List<ShipmentDocumentViewModel>());
                }
                else
                {
                    response.ShipmentDocuments.AddRange(unitOfWork.GetShipmentDocumentRepository()
                        .GetShipmentDocuments(request.CompanyId)
                        ?.ConvertToShipmentDocumentViewModelList() ?? new List<ShipmentDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ShipmentDocuments = new List<ShipmentDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
