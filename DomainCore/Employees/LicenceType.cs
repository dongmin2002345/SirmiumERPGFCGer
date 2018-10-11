using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
	public class LicenceType : BaseEntity
	{
		public string Code { get; set; }
		public string Category { get; set; }
		public string Description { get; set; }
		public int? CountryId { get; set; }
		public Country Country { get; set; }
	}
}
