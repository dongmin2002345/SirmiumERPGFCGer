using DomainCore.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IInputInvoiceRepository
    {
		List<InputInvoice> GetInputInvoices(int companyId);
		List<InputInvoice> GetInputInvoicesNewerThan(int companyId, DateTime lastUpdateTime);

		InputInvoice Create(InputInvoice inputInvoice);
		InputInvoice Delete(Guid identifier);
	}
}
