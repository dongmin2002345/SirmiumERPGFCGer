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
    public class BusinessPartnerTypeService : IBusinessPartnerTypeService
    {
        public BusinessPartnerTypeListResponse GetBusinessPartnerTypes(int companyId)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerTypeViewModel>, BusinessPartnerTypeListResponse>("GetBusinessPartnerTypes", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerTypeViewModel>, BusinessPartnerTypeListResponse>("GetBusinessPartnerTypesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeResponse Create(BusinessPartnerTypeViewModel re)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerTypeViewModel, BusinessPartnerTypeResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerType = new BusinessPartnerTypeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeResponse Delete(Guid identifier)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                BusinessPartnerTypeViewModel re = new BusinessPartnerTypeViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerTypeViewModel, BusinessPartnerTypeResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerType = new BusinessPartnerTypeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeListResponse Sync(SyncBusinessPartnerTypeRequest request)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerTypeRequest, BusinessPartnerTypeViewModel, BusinessPartnerTypeListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
