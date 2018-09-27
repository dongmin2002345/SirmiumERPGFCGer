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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        /// <summary>
        /// Get all active business partners for selected company
        /// </summary>
        /// <returns></returns>
        public BusinessPartnerListResponse GetBusinessPartners(string filterString)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartners",
                    new Dictionary<string, string>() {
                        { "FilterString", filterString }
                    });
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersForPopup(string filterString)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FilterString", filterString);

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartnersForPopup", parameters);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersByPage(int currentPage = 1, int itemsPerPage = 6, string businessPartnerName = "")
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartnersByPage", new Dictionary<string, string>()
                {
                    { "CurrentPage", currentPage.ToString() },
                    { "itemsPerPage", itemsPerPage.ToString() },
                    { "BusinessPartnerName", businessPartnerName },
                });
                //response.TotalItems = WpfApiHandler.GetFromApi<int, BusinessPartnerViewModel, BusinessPartnerListResponse>("GetBusinessPartnersCount", new Dictionary<string, string>() {
                //    { "CompanyID", companyId.ToString() },
                //    { "searchParameter", businessPartnerName }
                //}).TotalItems;

                response.Success = true;
            }

            catch (Exception ex)
            {
                response = new BusinessPartnerListResponse();
                response.BusinessPartnersByPage = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersCount(string searchParameter = "")
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("searchParameter", searchParameter);

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartnersCount", parameters);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get single active business partner for selected company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessPartnerResponse GetBusinessPartner(int id)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<BusinessPartnerViewModel, BusinessPartnerResponse>("GetBusinessPartner", new Dictionary<string, string>()
                {
                    { "id", id.ToString() },
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        ///<summary>
        /// Gets new code for business partner creation
        ///</summary>
        ///<returns></returns>
        public BusinessPartnerCodeResponse GetNewCodeValue()
        {
            BusinessPartnerCodeResponse response = new BusinessPartnerCodeResponse();

            try
            {
                response = WpfApiHandler.GetFromApi<int, BusinessPartnerViewModel, BusinessPartnerCodeResponse>("GetNewCodeValue", null);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Create new business partner
        /// </summary>
        /// <param name="businessPartner"></param>
        /// <returns></returns>
        public BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerViewModel, BusinessPartnerResponse>(businessPartner, "Create");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Update business partner 
        /// </summary>
        /// <param name="businessPartner"></param>
        /// <returns></returns>
        public BusinessPartnerResponse Update(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerViewModel, BusinessPartnerResponse>(businessPartner, "Update");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Deactivate business partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessPartnerResponse Delete(int id)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                BusinessPartnerViewModel viewModel = new BusinessPartnerViewModel();
                viewModel.Id = id;
                response = WpfApiHandler.SendToApi<BusinessPartnerViewModel, BusinessPartnerResponse>(viewModel, "Delete");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
