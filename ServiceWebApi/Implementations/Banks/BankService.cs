using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Banks
{
	public class BankService : IBankService
	{
		public BankListResponse GetBanks(int companyId)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<BankViewModel>, BankListResponse>("GetBanks", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankListResponse GetBanksNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<BankViewModel>, BankListResponse>("GetBanksNewerThen", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() },
					{ "LastUpdateTime", lastUpdateTime.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankResponse Create(BankViewModel bank)
		{
			BankResponse response = new BankResponse();
			try
			{
				response = WpfApiHandler.SendToApi<BankViewModel, BankResponse>(bank, "Create");
			}
			catch (Exception ex)
			{
				response.Bank = new BankViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankResponse Delete(Guid identifier)
		{
			BankResponse response = new BankResponse();
			try
			{
				BankViewModel re = new BankViewModel();
				re.Identifier = identifier;
				response = WpfApiHandler.SendToApi<BankViewModel, BankResponse>(re, "Delete");
			}
			catch (Exception ex)
			{
				response.Bank = new BankViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankListResponse Sync(SyncBankRequest request)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncBankRequest, BankViewModel, BankListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
