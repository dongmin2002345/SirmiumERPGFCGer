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

        //OutputInvoiceListResponse GetOutputInvoicesByPage(int currentPage = 1, int itemsPerPage = 50, string filterString = "");

        //OutputInvoiceListResponse GetOutputInvoicesForPopup(string filterString);

        //OutputInvoiceResponse GetOutputInvoice(int id);

        //OutputInvoiceResponse GetNewCodeValue();

        //OutputInvoiceResponse CancelOutputInvoice(int id);

        //OutputInvoiceResponse Create(OutputInvoiceViewModel outputinvoice);
        //OutputInvoiceResponse Update(OutputInvoiceViewModel outputinvoice);
        //OutputInvoiceResponse SetInvoiceLock(int id, bool locked);
        //OutputInvoiceResponse Delete(int id);
    }
}