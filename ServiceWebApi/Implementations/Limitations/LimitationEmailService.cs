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
    public class LimitationEmailService : ILimitationEmailService
    {
        public LimitationEmailListResponse GetLimitationEmails(int companyId)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<LimitationEmailViewModel>, LimitationEmailListResponse>("GetLimitationEmails", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailListResponse GetLimitationEmailsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<LimitationEmailViewModel>, LimitationEmailListResponse>("GetLimitationEmailsNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailResponse Create(LimitationEmailViewModel re)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                response = WpfApiHandler.SendToApi<LimitationEmailViewModel, LimitationEmailResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.LimitationEmail = new LimitationEmailViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailResponse Delete(Guid identifier)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                LimitationEmailViewModel re = new LimitationEmailViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<LimitationEmailViewModel, LimitationEmailResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.LimitationEmail = new LimitationEmailViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailListResponse Sync(SyncLimitationEmailRequest request)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncLimitationEmailRequest, LimitationEmailViewModel, LimitationEmailListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
