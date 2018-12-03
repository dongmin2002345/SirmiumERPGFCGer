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
    public class CompanyUserService : ICompanyUserService
    {
        IUnitOfWork unitOfWork;

        public CompanyUserService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }


        public CompanyUserListResponse GetCompanyUsers()
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            response.CompanyUsers = new List<CompanyUserViewModel>();
            try
            {
                var itemsFromDb = unitOfWork.GetCompanyUserRepository()
                    ?.GetCompanyUsers()
                    ?.ConvertToCompanyUserViewModelList();

                response.CompanyUsers.AddRange(itemsFromDb);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.CompanyUsers = new List<CompanyUserViewModel>();
            }
            return response;
        }

        public CompanyUserListResponse GetCompanyUsersNewerThan(DateTime? dateFrom)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            response.CompanyUsers = new List<CompanyUserViewModel>();
            try
            {
                var itemsFromDb = unitOfWork.GetCompanyUserRepository()
                    ?.GetCompanyUsersNewerThan((DateTime)dateFrom)
                    ?.ConvertToCompanyUserViewModelList();

                response.CompanyUsers.AddRange(itemsFromDb);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.CompanyUsers = new List<CompanyUserViewModel>();
            }
            return response;
        }

        public CompanyUserListResponse Sync(SyncCompanyUserRequest request)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            List<CompanyUserViewModel> compUsers = new List<CompanyUserViewModel>();
            try
            {

                if (request.LastUpdatedAt == null)
                {
                    var compUsers2 =
                        unitOfWork.GetCompanyUserRepository()
                        ?.GetCompanyUsers()
                        ?.ConvertToCompanyUserViewModelList();

                    compUsers.AddRange(compUsers2);
                }
                else
                {
                    var compUsers2 =
                        unitOfWork.GetCompanyUserRepository()
                        ?.GetCompanyUsersNewerThan((DateTime)request.LastUpdatedAt)
                        ?.ConvertToCompanyUserViewModelList();
                    compUsers.AddRange(compUsers2);
                }
                response.CompanyUsers = compUsers;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.CompanyUsers = new List<CompanyUserViewModel>();
            }
            return response;
        }

        public CompanyUserResponse Create(CompanyUserViewModel companyUser)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {

                var savedItem = unitOfWork.GetCompanyUserRepository().Create(companyUser?.ConvertToCompanyUser());
                unitOfWork.Save();
                response.Success = true;

                response.CompanyUser = savedItem?.ConvertToCompanyUserViewModel();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.CompanyUser = new CompanyUserViewModel();
            }
            return response;
        }

        public CompanyUserResponse Delete(Guid identifier)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {

                var deletedItem = unitOfWork.GetCompanyUserRepository().Delete(identifier);

                unitOfWork.Save();
                response.Success = true;
                response.CompanyUser = deletedItem?.ConvertToCompanyUserViewModel();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.CompanyUser = new CompanyUserViewModel();
            }
            return response;
        }
    }
}
