using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        public InvoiceListResponse GetInvoices(int companyId)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<InvoiceViewModel>, InvoiceListResponse>("GetInvoices", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceListResponse GetInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<InvoiceViewModel>, InvoiceListResponse>("GetInvoicesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceResponse Create(InvoiceViewModel re)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                response = WpfApiHandler.SendToApi<InvoiceViewModel, InvoiceResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.Invoice = new InvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceResponse Delete(Guid identifier)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                InvoiceViewModel re = new InvoiceViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<InvoiceViewModel, InvoiceResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Invoice = new InvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceListResponse Sync(SyncInvoiceRequest request)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncInvoiceRequest, InvoiceViewModel, InvoiceListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
