using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Banks
{
	public class BankResponse : BaseResponse
	{
		public BankViewModel Bank { get; set; }
	}
}
