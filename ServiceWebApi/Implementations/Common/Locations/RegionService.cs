using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Locations
{
    public class RegionService : IRegionService
    {
        public RegionListResponse GetRegions(int companyId)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<RegionViewModel>, RegionListResponse>("GetRegions", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionListResponse GetRegionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<RegionViewModel>, RegionListResponse>("GetRegionsNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionResponse Create(RegionViewModel re)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                response = WpfApiHandler.SendToApi<RegionViewModel, RegionResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.Region = new RegionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionResponse Delete(Guid identifier)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                RegionViewModel re = new RegionViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<RegionViewModel, RegionResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Region = new RegionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionListResponse Sync(SyncRegionRequest request)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncRegionRequest, RegionViewModel, RegionListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
