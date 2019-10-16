using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Shipments
{
    public class ShipmentDocument : BaseEntity
    {
        public int? ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }

        public int ItemStatus { get; set; }
    }
}
