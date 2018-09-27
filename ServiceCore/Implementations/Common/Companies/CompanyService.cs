using DataMapper.Mappers.Common.Companies;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Companies
{
    public class CompanyService : ICompanyService
    {
        IUnitOfWork unitOfWork;

        /// <summary>
        /// Company service constructor
        /// </summary>
        /// <param name="CompanyRepository"></param>
        public CompanyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all active Companies from database
        /// </summary>
        /// <returns></returns>
        public CompanyListResponse GetCompanies()
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response.Companies = unitOfWork.GetCompanyRepository().GetCompanies()
                    .ConvertToCompanyViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Companies = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get single active Company by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyResponse GetCompany(int id)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.Company = unitOfWork.GetCompanyRepository().GetCompany(id)
                    .ConvertToCompanyViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Company = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Gets new code for company addition
        /// </summary>
        /// <returns></returns>
        public CompanyResponse GetNewCodeValue()
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.NewCode = unitOfWork.GetCompanyRepository().GetNewCodeValue();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Company = new CompanyViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Create new Company
        /// </summary>
        /// <param name="Company"></param>
        /// <returns></returns>
        public CompanyResponse Create(CompanyViewModel Company)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.Company = unitOfWork.GetCompanyRepository().Create(Company.ConvertToCompany())
                    .ConvertToCompanyViewModel();

                //var resp = FirebaseHelper.Send<CompanyViewModel>("Companies", response.Company);

                unitOfWork.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Company = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Update Company data
        /// </summary>
        /// <param name="Company"></param>
        /// <returns></returns>
        public CompanyResponse Update(CompanyViewModel Company)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.Company = unitOfWork.GetCompanyRepository().Update(Company.ConvertToCompany())
                    .ConvertToCompanyViewModel();
                unitOfWork.Save();

                //var resp = FirebaseHelper.Send<CompanyViewModel>("Companies", response.Company);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Company = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Deactivate Company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyResponse Delete(int id)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.Company = unitOfWork.GetCompanyRepository().Delete(id)
                    .ConvertToCompanyViewModel();
                unitOfWork.Save();

                //var resp = FirebaseHelper.Send<CompanyViewModel>("Companies", response.Company);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Company = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
