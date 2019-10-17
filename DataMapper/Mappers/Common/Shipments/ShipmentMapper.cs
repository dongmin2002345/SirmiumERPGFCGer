using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Prices;
using DomainCore.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Shipments
{
    public static class ShipmentMapper
    {
        public static List<ShipmentViewModel> ConvertToShipmentViewModelList(this IEnumerable<Shipment> Shipments)
        {
            List<ShipmentViewModel> viewModels = new List<ShipmentViewModel>();
            foreach (Shipment Shipment in Shipments)
            {
                viewModels.Add(Shipment.ConvertToShipmentViewModel());
            }
            return viewModels;
        }


        public static ShipmentViewModel ConvertToShipmentViewModel(this Shipment shipment)
        {
            ShipmentViewModel shipmentViewModel = new ShipmentViewModel()
            {
                Id = shipment.Id,
                Identifier = shipment.Identifier,

                Code = shipment.Code,
                ShipmentDate = shipment.ShipmentDate,
                Address = shipment.Address,
                ShipmentNumber = shipment.ShipmentNumber,
                Acceptor = shipment.Acceptor,
                DeliveryDate = shipment.DeliveryDate,
                ReturnReceipt = shipment.ReturnReceipt,
                DocumentName = shipment.DocumentName,
                Note = shipment.Note,
                
                ServiceDelivery = shipment.ServiceDelivery?.ConvertToServiceDeliveryViewModelLite(),

                IsActive = shipment.Active,

                CreatedBy = shipment.CreatedBy?.ConvertToUserViewModelLite(),
                Company = shipment.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = shipment.UpdatedAt,
                CreatedAt = shipment.CreatedAt
            };



            return shipmentViewModel;
        }

        public static List<ShipmentViewModel> ConvertToShipmentViewModelListLite(this IEnumerable<Shipment> shipments)
        {
            List<ShipmentViewModel> shipmentViewModels = new List<ShipmentViewModel>();
            foreach (Shipment shipment in shipments)
            {
                shipmentViewModels.Add(shipment.ConvertToShipmentViewModelLite());
            }
            return shipmentViewModels;
        }

        public static ShipmentViewModel ConvertToShipmentViewModelLite(this Shipment shipment)
        {
            ShipmentViewModel shipmentViewModel = new ShipmentViewModel()
            {
                Id = shipment.Id,
                Identifier = shipment.Identifier,

                Code = shipment.Code,
                ShipmentDate = shipment.ShipmentDate,
                Address = shipment.Address,
                ShipmentNumber = shipment.ShipmentNumber,
                Acceptor = shipment.Acceptor,
                DeliveryDate = shipment.DeliveryDate,
                ReturnReceipt = shipment.ReturnReceipt,
                DocumentName = shipment.DocumentName,
                Note = shipment.Note,

                IsActive = shipment.Active,

                UpdatedAt = shipment.UpdatedAt,
                CreatedAt = shipment.CreatedAt
            };


            return shipmentViewModel;
        }

        public static Shipment ConvertToShipment(this ShipmentViewModel shipmentViewModel)
        {
            Shipment shipment = new Shipment()
            {
                Id = shipmentViewModel.Id,
                Identifier = shipmentViewModel.Identifier,

                Code = shipmentViewModel.Code,
                ShipmentDate = shipmentViewModel.ShipmentDate,
                Address = shipmentViewModel.Address,
                ShipmentNumber = shipmentViewModel.ShipmentNumber,
                Acceptor = shipmentViewModel.Acceptor,
                DeliveryDate = shipmentViewModel.DeliveryDate,
                ReturnReceipt = shipmentViewModel.ReturnReceipt,
                DocumentName = shipmentViewModel.DocumentName,
                Note = shipmentViewModel.Note,

                ServiceDeliveryId = shipmentViewModel.ServiceDelivery?.Id ?? null,

                Active = shipmentViewModel.IsActive,

                CreatedById = shipmentViewModel.CreatedBy?.Id ?? null,
                CompanyId = shipmentViewModel.Company?.Id ?? null,

                CreatedAt = shipmentViewModel.CreatedAt,
                UpdatedAt = shipmentViewModel.UpdatedAt
            };

            return shipment;
        }
    }
}
