using DomainCore.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
	public interface IOutputInvoiceDocumentRepository
	{
		List<OutputInvoiceDocument> GetOutputInvoiceDocuments(int companyId);
		List<OutputInvoiceDocument> GetOutputInvoiceDocumentsByOutputInvoice(int outputInvoiceId);
		List<OutputInvoiceDocument> GetOutputInvoiceDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

		OutputInvoiceDocument Create(OutputInvoiceDocument OutputInvoiceDocument);
		OutputInvoiceDocument Delete(Guid identifier);
	}
}
