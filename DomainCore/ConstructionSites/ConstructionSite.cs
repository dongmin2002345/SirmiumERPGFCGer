﻿using DomainCore.Base;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Locations;
using DomainCore.Common.Shipments;
using DomainCore.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.ConstructionSites
{
    public class ConstructionSite : BaseEntity
    {
        public string Code { get; set; }
        public string InternalCode { get; set; }

        public string Name { get; set; }

        public string NamePartner { get; set; }
        public string AddressPartner { get; set; }
        public int? CityPartnerId { get; set; }
        public City CityPartner { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public int? StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Address { get; set; }

        public int MaxWorkers { get; set; }

        public DateTime ProContractDate { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractExpiration { get; set; }

        public DateTime PaymentDate { get; set; }
        public string Path { get; set; }
        public decimal PaymentValue { get; set; }

        public int? ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }
}
