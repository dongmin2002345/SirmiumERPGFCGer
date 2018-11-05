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
    public class BusinessPartnerByConstructionSiteService : IBusinessPartnerByConstructionSiteService
    {
        public BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSites(int companyId)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerByConstructionSiteViewModel>, BusinessPartnerByConstructionSiteListResponse>("GetBusinessPartnerByConstructionSites", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerByConstructionSiteViewModel>, BusinessPartnerByConstructionSiteListResponse>("GetBusinessPartnerByConstructionSitesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteResponse Create(BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSite)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerByConstructionSiteViewModel, BusinessPartnerByConstructionSiteResponse>(businessPartnerByConstructionSite, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSite = new BusinessPartnerByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteResponse Delete(Guid identifier)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                BusinessPartnerByConstructionSiteViewModel re = new BusinessPartnerByConstructionSiteViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerByConstructionSiteViewModel, BusinessPartnerByConstructionSiteResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSite = new BusinessPartnerByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteListResponse Sync(SyncBusinessPartnerByConstructionSiteRequest request)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerByConstructionSiteRequest, BusinessPartnerByConstructionSiteViewModel, BusinessPartnerByConstructionSiteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
