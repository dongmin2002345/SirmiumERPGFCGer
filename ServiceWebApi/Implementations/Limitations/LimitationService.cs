using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Limitations
{
    public class LimitationService : ILimitationService
    {
        public LimitationListResponse GetLimitations(int companyId)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<LimitationViewModel>, LimitationListResponse>("GetLimitations", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationListResponse GetLimitationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<LimitationViewModel>, LimitationListResponse>("GetLimitationsNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationResponse Create(LimitationViewModel limitation)
        {
            LimitationResponse response = new LimitationResponse();
            try
            {
                response = WpfApiHandler.SendToApi<LimitationViewModel, LimitationResponse>(limitation, "Create");
            }
            catch (Exception ex)
            {
                response.Limitation = new LimitationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationListResponse Sync(SyncLimitationRequest request)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncLimitationRequest, LimitationViewModel, LimitationListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
