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
    public class CountryService : ICountryService
    {
        public CountryListResponse GetCountries(int companyId)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CountryViewModel>, CountryListResponse>("GetCountries", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryListResponse GetCountriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CountryViewModel>, CountryListResponse>("GetCountriesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryResponse Create(CountryViewModel co)
        {
            CountryResponse response = new CountryResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CountryViewModel, CountryResponse>(co, "Create");
            }
            catch (Exception ex)
            {
                response.Country = new CountryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryResponse Delete(Guid identifier)
        {
            CountryResponse response = new CountryResponse();
            try
            {
                CountryViewModel co = new CountryViewModel();
                co.Identifier = identifier;
                response = WpfApiHandler.SendToApi<CountryViewModel, CountryResponse>(co, "Delete");
            }
            catch (Exception ex)
            {
                response.Country = new CountryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryListResponse Sync(SyncCountryRequest request)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCountryRequest, CountryViewModel, CountryListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
