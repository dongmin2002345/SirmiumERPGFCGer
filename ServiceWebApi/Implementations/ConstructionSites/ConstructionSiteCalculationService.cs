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
    public class ConstructionSiteCalculationService : IConstructionSiteCalculationService
    {
        public ConstructionSiteCalculationListResponse GetConstructionSiteCalculations(int companyId)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<ConstructionSiteCalculationViewModel>, ConstructionSiteCalculationListResponse>("GetConstructionSiteCalculations", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ConstructionSiteCalculationListResponse GetConstructionSiteCalculationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ConstructionSiteCalculationViewModel>, ConstructionSiteCalculationListResponse>("GetConstructionSiteCalculationsNewerThen", new Dictionary<string, string>()
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

        public ConstructionSiteCalculationResponse Create(ConstructionSiteCalculationViewModel ConstructionSiteCalculationViewModel)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ConstructionSiteCalculationViewModel, ConstructionSiteCalculationResponse>(ConstructionSiteCalculationViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculation = new ConstructionSiteCalculationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ConstructionSiteCalculationListResponse Sync(SyncConstructionSiteCalculationRequest request)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncConstructionSiteCalculationRequest, ConstructionSiteCalculationViewModel, ConstructionSiteCalculationListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
