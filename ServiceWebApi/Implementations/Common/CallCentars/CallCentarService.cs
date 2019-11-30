using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.CallCentars
{
    public class CallCentarService : ICallCentarService
    {
        public CallCentarListResponse GetCallCentars(int companyId)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CallCentarViewModel>, CallCentarListResponse>("GetCallCentars", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.CallCentars = new List<CallCentarViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarResponse Create(CallCentarViewModel CallCentar)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CallCentarViewModel, CallCentarResponse>(CallCentar, "Create");
            }
            catch (Exception ex)
            {
                response.CallCentar = new CallCentarViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarResponse Delete(Guid identifier)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                CallCentarViewModel CallCentar = new CallCentarViewModel();
                CallCentar.Identifier = identifier;
                response = WpfApiHandler.SendToApi<CallCentarViewModel, CallCentarResponse>(CallCentar, "Delete");
            }
            catch (Exception ex)
            {
                response.CallCentar = new CallCentarViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarListResponse Sync(SyncCallCentarRequest request)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCallCentarRequest, CallCentarViewModel, CallCentarListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.CallCentars = new List<CallCentarViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarResponse NotifyUser(CallCentarViewModel callCentar)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CallCentarViewModel, CallCentarResponse>(callCentar, "NotifyUser");
            }
            catch (Exception ex)
            {
                response.CallCentar = new CallCentarViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
