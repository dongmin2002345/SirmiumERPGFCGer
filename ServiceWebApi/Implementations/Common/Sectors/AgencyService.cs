using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Sectors
{
    public class AgencyService : IAgencyService
    {
        public AgencyListResponse GetAgencies(int companyId)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<AgencyViewModel>, AgencyListResponse>("GetAgencies", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyListResponse GetAgenciesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<AgencyViewModel>, AgencyListResponse>("GetAgenciesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyResponse Create(AgencyViewModel Agency)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                response = WpfApiHandler.SendToApi<AgencyViewModel, AgencyResponse>(Agency, "Create");
            }
            catch (Exception ex)
            {
                response.Agency = new AgencyViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyResponse Delete(Guid identifier)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                AgencyViewModel re = new AgencyViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<AgencyViewModel, AgencyResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Agency = new AgencyViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyListResponse Sync(SyncAgencyRequest request)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncAgencyRequest, AgencyViewModel, AgencyListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
