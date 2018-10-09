using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.ConstructionSites
{
    public class ConstructionSiteService : IConstructionSiteService
    {
        public ConstructionSiteListResponse GetConstructionSites(int companyId)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ConstructionSiteViewModel>, ConstructionSiteListResponse>("GetConstructionSites", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteListResponse GetConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ConstructionSiteViewModel>, ConstructionSiteListResponse>("GetConstructionSitesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteResponse Create(ConstructionSiteViewModel constructionSite)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ConstructionSiteViewModel, ConstructionSiteResponse>(constructionSite, "Create");
            }
            catch (Exception ex)
            {
                response.ConstructionSite = new ConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteResponse Delete(Guid identifier)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                ConstructionSiteViewModel re = new ConstructionSiteViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ConstructionSiteViewModel, ConstructionSiteResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.ConstructionSite = new ConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteListResponse Sync(SyncConstructionSiteRequest request)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncConstructionSiteRequest, ConstructionSiteViewModel, ConstructionSiteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
