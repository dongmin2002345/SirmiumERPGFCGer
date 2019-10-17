using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Shipments
{
    public static class ShipmentDocumentMapper
    {
        public static List<ShipmentDocumentViewModel> ConvertToShipmentDocumentViewModelList(this IEnumerable<ShipmentDocument> shipmentDocuments)
        {
            List<ShipmentDocumentViewModel> ShipmentDocumentViewModels = new List<ShipmentDocumentViewModel>();
            foreach (ShipmentDocument ShipmentDocument in shipmentDocuments)
            {
                ShipmentDocumentViewModels.Add(ShipmentDocument.ConvertToShipmentDocumentViewModel());
            }
            return ShipmentDocumentViewModels;
        }

        public static ShipmentDocumentViewModel ConvertToShipmentDocumentViewModel(this ShipmentDocument shipmentDocument)
        {
            ShipmentDocumentViewModel ShipmentDocumentViewModel = new ShipmentDocumentViewModel()
            {
                Id = shipmentDocument.Id,
                Identifier = shipmentDocument.Identifier,

                Shipment = shipmentDocument.Shipment?.ConvertToShipmentViewModelLite(),

                Name = shipmentDocument.Name,
                CreateDate = shipmentDocument.CreateDate,
                Path = shipmentDocument.Path,
                ItemStatus = shipmentDocument.ItemStatus,
                IsActive = shipmentDocument.Active,

                CreatedBy = shipmentDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = shipmentDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = shipmentDocument.UpdatedAt,
                CreatedAt = shipmentDocument.CreatedAt
            };

            return ShipmentDocumentViewModel;
        }

        public static ShipmentDocumentViewModel ConvertToShipmentDocumentViewModelLite(this ShipmentDocument shipmentDocument)
        {
            ShipmentDocumentViewModel ShipmentDocumentViewModel = new ShipmentDocumentViewModel()
            {
                Id = shipmentDocument.Id,
                Identifier = shipmentDocument.Identifier,

                Name = shipmentDocument.Name,
                CreateDate = shipmentDocument.CreateDate,
                Path = shipmentDocument.Path,
                ItemStatus = shipmentDocument.ItemStatus,
                IsActive = shipmentDocument.Active,

                UpdatedAt = shipmentDocument.UpdatedAt,
                CreatedAt = shipmentDocument.CreatedAt
            };

            return ShipmentDocumentViewModel;
        }

        public static ShipmentDocument ConvertToShipmentDocument(this ShipmentDocumentViewModel shipmentDocumentViewModel)
        {
            ShipmentDocument ShipmentDocument = new ShipmentDocument()
            {
                Id = shipmentDocumentViewModel.Id,
                Identifier = shipmentDocumentViewModel.Identifier,

                ShipmentId = shipmentDocumentViewModel.Shipment?.Id ?? null,

                Name = shipmentDocumentViewModel.Name,
                CreateDate = shipmentDocumentViewModel.CreateDate,
                Path = shipmentDocumentViewModel.Path,
                ItemStatus = shipmentDocumentViewModel.ItemStatus,
                Active = shipmentDocumentViewModel.IsActive,

                CreatedById = shipmentDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = shipmentDocumentViewModel.Company?.Id ?? null,

                CreatedAt = shipmentDocumentViewModel.CreatedAt,
                UpdatedAt = shipmentDocumentViewModel.UpdatedAt
            };

            return ShipmentDocument;
        }
    }
}
