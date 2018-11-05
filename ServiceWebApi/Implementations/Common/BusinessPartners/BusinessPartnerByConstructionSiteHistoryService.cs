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
    public class BusinessPartnerByConstructionSiteHistoryService : IBusinessPartnerByConstructionSiteHistoryService
    {
        public BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistories(int companyId)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerByConstructionSiteHistoryViewModel>, BusinessPartnerByConstructionSiteHistoryListResponse>("GetBusinessPartnerByConstructionSiteHistories", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerByConstructionSiteHistoryViewModel>, BusinessPartnerByConstructionSiteHistoryListResponse>("GetBusinessPartnerByConstructionSiteHistoriesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryResponse Create(BusinessPartnerByConstructionSiteHistoryViewModel re)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerByConstructionSiteHistoryViewModel, BusinessPartnerByConstructionSiteHistoryResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistory = new BusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryResponse Delete(Guid identifier)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                BusinessPartnerByConstructionSiteHistoryViewModel re = new BusinessPartnerByConstructionSiteHistoryViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerByConstructionSiteHistoryViewModel, BusinessPartnerByConstructionSiteHistoryResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistory = new BusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryListResponse Sync(SyncBusinessPartnerByConstructionSiteHistoryRequest request)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerByConstructionSiteHistoryRequest, BusinessPartnerByConstructionSiteHistoryViewModel, BusinessPartnerByConstructionSiteHistoryListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
