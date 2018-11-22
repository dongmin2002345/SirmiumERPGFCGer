using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
	public class PhysicalPerson : BaseEntity
	{
		public string Code { get; set; }
		public string PhysicalPersonCode { get; set; }
		public string Name { get; set; }
		public string SurName { get; set; }

		public string ConstructionSiteCode { get; set; }
		public string ConstructionSiteName { get; set; }

		public DateTime DateOfBirth { get; set; }
		public int Gender { get; set; }

		public int? CountryId { get; set; }
		public Country Country { get; set; }

		public int? RegionId { get; set; }
		public Region Region { get; set; }

		public int? MunicipalityId { get; set; }
		public Municipality Municipality { get; set; }

		public int? CityId { get; set; }
		public City City { get; set; }

		public string Address { get; set; }

		public int? PassportCountryId { get; set; }
		public Country PassportCountry { get; set; }

		public int? PassportCityId { get; set; }
		public City PassportCity { get; set; }

		public string Passport { get; set; }
		public DateTime? VisaFrom { get; set; }
		public DateTime? VisaTo { get; set; }

		public int? ResidenceCountryId { get; set; }
		public Country ResidenceCountry { get; set; }
		public int? ResidenceCityId { get; set; }
		public City ResidenceCity { get; set; }
		public string ResidenceAddress { get; set; }

		public DateTime? EmbassyDate { get; set; }
		public DateTime? VisaDate { get; set; }
		public DateTime? VisaValidFrom { get; set; }
		public DateTime? VisaValidTo { get; set; }
		public DateTime? WorkPermitFrom { get; set; }
		public DateTime? WorkPermitTo { get; set; }

		//public List<PhysicalPersonItem> PhysicalPersonItems { get; set; }
		//public List<PhysicalPersonLicence> PhysicalPersonLicences { get; set; }
		//public List<PhysicalPersonProfession> PhysicalPersonProfessions { get; set; }
	}
}
