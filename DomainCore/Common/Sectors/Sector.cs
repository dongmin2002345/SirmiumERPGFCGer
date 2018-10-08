using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Sectors
{
    public class Sector: BaseEntity
    {
		public string Code { get; set; }
		public string SecondCode { get; set; }
		public string Name { get; set; }

		//public int? CountryId { get; set; }
		//public Country Country { get; set; }
    }
}
