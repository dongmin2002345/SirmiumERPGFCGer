using ServiceInterfaces.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Banks
{
	public class BankCodeResponse : BaseResponse
	{
		public int Code { get; set; }
	}
}
