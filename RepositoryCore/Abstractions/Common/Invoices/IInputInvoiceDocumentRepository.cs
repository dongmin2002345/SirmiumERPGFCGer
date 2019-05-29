using DomainCore.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
	public interface IInputInvoiceDocumentRepository
	{
		List<InputInvoiceDocument> GetInputInvoiceDocuments(int companyId);
		List<InputInvoiceDocument> GetInputInvoiceDocumentsByInputInvoice(int inputInvoiceId);
		List<InputInvoiceDocument> GetInputInvoiceDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

		InputInvoiceDocument Create(InputInvoiceDocument InputInvoiceDocument);
		InputInvoiceDocument Delete(Guid identifier);
	}
}
