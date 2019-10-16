using DomainCore.Base;
using DomainCore.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Shipments
{
    public class Shipment : BaseEntity
    {
        public string Code { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Address { get; set; }

        public int? ServiceDeliveryId { get; set; }
        public ServiceDelivery ServiceDelivery { get; set; }
        public string ShipmentNumber { get; set; }

        public string Acceptor { get; set; }
        public DateTime DeliveryDate { get; set; }

        public string ReturnReceipt { get; set; }

        public string DocumentName { get; set; }

        public string Note { get; set; }

    }
}
