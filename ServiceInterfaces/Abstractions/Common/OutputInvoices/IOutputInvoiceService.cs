using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.OutputInvoices
{
    public interface IOutputInvoiceService
    {
        OutputInvoiceListResponse GetOutputInvoices(int companyId);
        OutputInvoiceListResponse GetOutputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime);

        OutputInvoiceResponse Create(OutputInvoiceViewModel outputInvoice);
        OutputInvoiceResponse Delete(Guid identifier);

        OutputInvoiceListResponse Sync(SyncOutputInvoiceRequest request);
    }
}