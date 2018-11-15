using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.InputInvoices
{
	public class InputInvoiceService : IInputInvoiceService
	{
		public InputInvoiceListResponse GetInputInvoices(int companyId)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<InputInvoiceViewModel>, InputInvoiceListResponse>("GetInputInvoices", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse GetInputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<InputInvoiceViewModel>, InputInvoiceListResponse>("GetInputInvoicesNewerThan", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() },
					{ "LastUpdateTime", lastUpdateTime.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Create(InputInvoiceViewModel re)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				response = WpfApiHandler.SendToApi<InputInvoiceViewModel, InputInvoiceResponse>(re, "Create");
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Delete(Guid identifier)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				InputInvoiceViewModel re = new InputInvoiceViewModel();
				re.Identifier = identifier;
				response = WpfApiHandler.SendToApi<InputInvoiceViewModel, InputInvoiceResponse>(re, "Delete");
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse Sync(SyncInputInvoiceRequest request)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncInputInvoiceRequest, InputInvoiceViewModel, InputInvoiceListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
