using ServiceInterfaces.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Employees
{
	public class LicenceTypeCodeResponse : BaseResponse
	{
		public int Code { get; set; }
	}
}
