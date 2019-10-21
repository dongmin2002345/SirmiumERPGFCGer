using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.OutputInvoices
{
    public class OutputInvoiceService : IOutputInvoiceService
    {
        public OutputInvoiceListResponse GetOutputInvoices(int companyId)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<OutputInvoiceViewModel>, OutputInvoiceListResponse>("GetOutputInvoices", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceListResponse GetOutputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<OutputInvoiceViewModel>, OutputInvoiceListResponse>("GetOutputInvoicesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceResponse Create(OutputInvoiceViewModel re)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = WpfApiHandler.SendToApi<OutputInvoiceViewModel, OutputInvoiceResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.OutputInvoice = new OutputInvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceResponse Delete(Guid identifier)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                OutputInvoiceViewModel re = new OutputInvoiceViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<OutputInvoiceViewModel, OutputInvoiceResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.OutputInvoice = new OutputInvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceListResponse Sync(SyncOutputInvoiceRequest request)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncOutputInvoiceRequest, OutputInvoiceViewModel, OutputInvoiceListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

