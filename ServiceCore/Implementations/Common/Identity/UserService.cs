using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Identity;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.Identity
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public UserListResponse GetUsers(int companyId)
        {
            UserListResponse response = new UserListResponse();
            try
            {
                response.Users = unitOfWork.GetUserRepository().GetUsers(companyId)
               .ConvertToUserViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Users = new List<UserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserListResponse GetUsersNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            UserListResponse response = new UserListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Users = unitOfWork.GetUserRepository()
                        .GetUsersNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToUserViewModelList();
                }
                else
                {
                    response.Users = unitOfWork.GetUserRepository()
                        .GetUsers(companyId)
                        .ConvertToUserViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Users = new List<UserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Create(UserViewModel re)
        {
            UserResponse response = new UserResponse();
            try
            {
                var companyUsers = re.CompanyUsers?.ConvertToCompanyUserList();

                re.CompanyUsers = null;

                User addedUser = unitOfWork.GetUserRepository().Create(re.ConvertToUser());

                var itemsFromDB = unitOfWork.GetCompanyUserRepository().GetCompanyUsersByUser(addedUser.Id);
                foreach (var item in itemsFromDB)
                    if (!companyUsers.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetCompanyUserRepository().Delete(item.Identifier);

                foreach (var compUser in companyUsers)
                {
                    compUser.UserId = addedUser.Id;

                    unitOfWork.GetCompanyUserRepository().Create(compUser);
                }

                unitOfWork.Save();
                response.User = addedUser.ConvertToUserViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = new UserViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Delete(Guid identifier)
        {
            UserResponse response = new UserResponse();
            try
            {
                User deletedUser = unitOfWork.GetUserRepository().Delete(identifier);

                unitOfWork.Save();

                response.User = deletedUser.ConvertToUserViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = new UserViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserListResponse Sync(SyncUserRequest request)
        {
            UserListResponse response = new UserListResponse();
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                if (request.LastUpdatedAt != null)
                {
                    var users2 = unitOfWork.GetUserRepository()
                        .GetUsersNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToUserViewModelList() ?? new List<UserViewModel>();

                    users.AddRange(users2);
                }
                else
                {
                    var users2 = unitOfWork.GetUserRepository()
                        .GetUsers(request.CompanyId)
                        ?.ConvertToUserViewModelList() ?? new List<UserViewModel>();
                    users.AddRange(users2);
                }
                response.Users = users;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Users = new List<UserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
