using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerBankService : IBusinessPartnerBankService
    {
        public BusinessPartnerBankListResponse GetBusinessPartnerBanks(int companyId)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerBankViewModel>, BusinessPartnerBankListResponse>("GetBusinessPartnerBanks", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerBankListResponse GetBusinessPartnerBanksNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerBankViewModel>, BusinessPartnerBankListResponse>("GetBusinessPartnerBanksNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerBankResponse Create(BusinessPartnerBankViewModel businessPartnerBankViewModel)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerBankViewModel, BusinessPartnerBankResponse>(businessPartnerBankViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBank = new BusinessPartnerBankViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerBankResponse Delete(Guid identifier)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                BusinessPartnerBankViewModel viewModel = new BusinessPartnerBankViewModel();
                viewModel.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerBankViewModel, BusinessPartnerBankResponse>(viewModel, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBank = new BusinessPartnerBankViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerBankListResponse Sync(SyncBusinessPartnerBankRequest request)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerBankRequest, BusinessPartnerBankViewModel, BusinessPartnerBankListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBanks = new List<BusinessPartnerBankViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
