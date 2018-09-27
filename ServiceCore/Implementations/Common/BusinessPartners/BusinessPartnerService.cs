using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        IUnitOfWork unitOfWork;

        /// <summary>
        /// Business partner service constructor
        /// </summary>
        /// <param name="BusinessPartnerRepository"></param>
        public BusinessPartnerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all active business partners for selected company
        /// </summary>
        /// <returns></returns>
        public BusinessPartnerListResponse GetBusinessPartners(string filterString)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                //response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                //    .GetBusinessPartners(filterString)
                //    .ConvertToBusinessPartnerViewModelList();
                //response.Success = true;
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
                //List<BusinessPartner> returnList = new List<BusinessPartner>();
                //returnList = unitOfWork.GetBusinessPartnerRepository()
                //    .GetBusinessPartnersForPopup(filterString);

                //response.BusinessPartners = returnList.ConvertToBusinessPartnerViewModelList();
                //response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
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
                response.BusinessPartner = unitOfWork.GetBusinessPartnerRepository()
                    .GetBusinessPartner(id)
                    .ConvertToBusinessPartnerViewModel();
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

        ///<summary>
        /// Gets new code for business partner creation
        ///</summary>
        ///<returns></returns>
        public BusinessPartnerCodeResponse GetNewCodeValue()
        {
            BusinessPartnerCodeResponse response = new BusinessPartnerCodeResponse();

            try
            {
                //response.Code = unitOfWork.GetBusinessPartnerRepository().GetNewCodeValue();
                //response.Success = true;
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

                BusinessPartner createdBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Create(businessPartner.ConvertToBusinessPartner());


                unitOfWork.Save();
                response.BusinessPartner = createdBusinessPartner.ConvertToBusinessPartnerViewModel();

                //Thread td = new Thread(() =>
                //{
                //    var resp = FirebaseHelper.Send<BusinessPartnerViewModel>("BusinessPartners", response.BusinessPartner);
                //});
                //td.IsBackground = true;
                //td.Start();

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
                //BusinessPartner createdBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Update(businessPartner.ConvertToBusinessPartner());

                //unitOfWork.Save();
                //response.BusinessPartner = createdBusinessPartner.ConvertToBusinessPartnerViewModel();

                ////Thread td = new Thread(() =>
                ////{
                ////    var resp = FirebaseHelper.Send<BusinessPartnerViewModel>("BusinessPartners", response.BusinessPartner);
                ////});
                ////td.IsBackground = true;
                ////td.Start();

                //response.Success = true;
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
        /// Deactivate business partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessPartnerResponse Delete(int id)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                //BusinessPartner deletedBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Delete(id);
                //unitOfWork.Save();

                //response.BusinessPartner = deletedBusinessPartner.ConvertToBusinessPartnerViewModel();

                ////Thread td = new Thread(() =>
                ////{
                ////    var resp = FirebaseHelper.Send<BusinessPartnerViewModel>("BusinessPartners", response.BusinessPartner);
                ////});
                ////td.IsBackground = true;
                ////td.Start();

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

        public BusinessPartnerListResponse GetBusinessPartnersByPage(int currentPage = 1, int itemsPerPage = 6, string searchParameter = "")
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                //response.BusinessPartnersByPage = unitOfWork.GetBusinessPartnerRepository()
                //    .GetBusinessPartnersByPage(currentPage, itemsPerPage, searchParameter)
                //    .ConvertToBusinessPartnerViewModelList();
                //response.TotalItems = unitOfWork.GetBusinessPartnerRepository().GetBusinessPartnersCount(searchParameter);
                //response.Success = true;
            }
            catch (Exception ex)
            {
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
                //response.TotalItems = unitOfWork.GetBusinessPartnerRepository().GetBusinessPartnersCount(searchParameter);
                //response.Success = true;
            }
            catch (Exception ex)
            {
                //response.Success = false;
                //response.Message = ex.Message;
            }

            return response;
        }
    }
}

