using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerOrganizationUnitService : IBusinessPartnerOrganizationUnitService
    {
        public BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnits(int companyId)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerOrganizationUnitViewModel>, BusinessPartnerOrganizationUnitListResponse>("GetBusinessPartnerOrganizationUnits", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerOrganizationUnitViewModel>, BusinessPartnerOrganizationUnitListResponse>("GetBusinessPartnerOrganizationUnitsNewerThen", new Dictionary<string, string>()
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

        public BusinessPartnerOrganizationUnitResponse Create(BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnitViewModel)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerOrganizationUnitViewModel, BusinessPartnerOrganizationUnitResponse>(businessPartnerOrganizationUnitViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitResponse Delete(Guid identifier)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                BusinessPartnerOrganizationUnitViewModel viewModel = new BusinessPartnerOrganizationUnitViewModel();
                viewModel.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerOrganizationUnitViewModel, BusinessPartnerOrganizationUnitResponse>(viewModel, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitListResponse Sync(SyncBusinessPartnerOrganizationUnitRequest request)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerOrganizationUnitRequest, BusinessPartnerOrganizationUnitViewModel, BusinessPartnerOrganizationUnitListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
