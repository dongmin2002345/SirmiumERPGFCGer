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

        public CompanyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

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

        public CompanyResponse Create(CompanyViewModel Company)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response.Company = unitOfWork.GetCompanyRepository().Create(Company.ConvertToCompany())
                    .ConvertToCompanyViewModel();

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

        public CompanyListResponse GetCompaniesNewerThan(DateTime? dateFrom)
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response.Companies = unitOfWork.GetCompanyRepository().GetCompaniesNewerThan((DateTime)dateFrom)
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

        public CompanyListResponse Sync(SyncCompanyRequest request)
        {
            CompanyListResponse response = new CompanyListResponse();
            List<CompanyViewModel> comp = new List<CompanyViewModel>();
            try
            {

                if (request.LastUpdatedAt == null)
                {
                    var comp2 =
                        unitOfWork.GetCompanyRepository()
                        ?.GetCompanies()
                        ?.ConvertToCompanyViewModelList();

                    comp.AddRange(comp2);
                }
                else
                {
                    var comp2 =
                        unitOfWork.GetCompanyRepository()
                        ?.GetCompaniesNewerThan((DateTime)request.LastUpdatedAt)
                        ?.ConvertToCompanyViewModelList();
                    comp.AddRange(comp2);
                }
                response.Companies = comp;
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
    }
}
