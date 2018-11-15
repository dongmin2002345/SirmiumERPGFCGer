using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.InputInvoices
{
	public class InputInvoiceResponse : BaseResponse
	{
		public InputInvoiceViewModel InputInvoice { get; set; }
	}
}
