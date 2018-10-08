using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Professions
{
    public class ProfessionService : IProfessionService
    {
        public ProfessionListResponse GetProfessions(int companyId)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ProfessionViewModel>, ProfessionListResponse>("GetProfessions", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionListResponse GetProfessionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ProfessionViewModel>, ProfessionListResponse>("GetProfessionsNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionResponse Create(ProfessionViewModel profession)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ProfessionViewModel, ProfessionResponse>(profession, "Create");
            }
            catch (Exception ex)
            {
                response.Profession = new ProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionResponse Delete(Guid identifier)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                ProfessionViewModel re = new ProfessionViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ProfessionViewModel, ProfessionResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Profession = new ProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionListResponse Sync(SyncProfessionRequest request)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncProfessionRequest, ProfessionViewModel, ProfessionListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
