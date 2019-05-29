using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.OutputInvoices
{
	public class OutputInvoiceDocumentResponse : BaseResponse
	{
		public OutputInvoiceDocumentViewModel OutputInvoiceDocument { get; set; }
	}
}
