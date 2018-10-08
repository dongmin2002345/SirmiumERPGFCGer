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
    public class MunicipalityService : IMunicipalityService
    {
        public MunicipalityListResponse GetMunicipalities(int companyId)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<MunicipalityViewModel>, MunicipalityListResponse>("GetMunicipalities", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityListResponse GetMunicipalitiesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<MunicipalityViewModel>, MunicipalityListResponse>("GetMunicipalitiesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityResponse Create(MunicipalityViewModel re)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                response = WpfApiHandler.SendToApi<MunicipalityViewModel, MunicipalityResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.Municipality = new MunicipalityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityResponse Delete(Guid identifier)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                MunicipalityViewModel re = new MunicipalityViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<MunicipalityViewModel, MunicipalityResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Municipality = new MunicipalityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityListResponse Sync(SyncMunicipalityRequest request)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncMunicipalityRequest, MunicipalityViewModel, MunicipalityListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
