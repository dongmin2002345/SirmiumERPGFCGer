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
    public class CityService : ICityService
    {
        public CityListResponse GetCities(int companyId)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CityViewModel>, CityListResponse>("GetCities", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Cities = new List<CityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CityResponse Create(CityViewModel city)
        {
            CityResponse response = new CityResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CityViewModel, CityResponse>(city, "Create");
            }
            catch (Exception ex)
            {
                response.City = new CityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CityResponse Delete(Guid identifier)
        {
            CityResponse response = new CityResponse();
            try
            {
                CityViewModel re = new CityViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<CityViewModel, CityResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.City = new CityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CityListResponse Sync(SyncCityRequest request)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCityRequest, CityViewModel, CityListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Cities = new List<CityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
