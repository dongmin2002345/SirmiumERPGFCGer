using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.TaxAdministrations
{
    class TaxAdministrationService : ITaxAdministrationService
    {
        public TaxAdministrationListResponse GetTaxAdministrations(int companyId)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<TaxAdministrationViewModel>, TaxAdministrationListResponse>("GetTaxAdministrations", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationListResponse GetTaxAdministrationsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<TaxAdministrationViewModel>, TaxAdministrationListResponse>("GetTaxAdministrationsNewerThan", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationResponse Create(TaxAdministrationViewModel taxAdministration)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                response = WpfApiHandler.SendToApi<TaxAdministrationViewModel, TaxAdministrationResponse>(taxAdministration, "Create");
            }
            catch (Exception ex)
            {
                response.TaxAdministration = new TaxAdministrationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationResponse Delete(Guid identifier)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                TaxAdministrationViewModel re = new TaxAdministrationViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<TaxAdministrationViewModel, TaxAdministrationResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.TaxAdministration = new TaxAdministrationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationListResponse Sync(SyncTaxAdministrationRequest request)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncTaxAdministrationRequest, TaxAdministrationViewModel, TaxAdministrationListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
