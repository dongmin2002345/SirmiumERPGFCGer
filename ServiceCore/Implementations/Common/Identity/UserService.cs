using DataMapper.Mappers.Common.Identity;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
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

        public UserListResponse GetUsers()
        {
            UserListResponse response = new UserListResponse();
            try
            {
                response.Users = unitOfWork.GetUserRepository().GetUsers()
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

        public UserResponse GetUser(int id)
        {
            UserResponse response = new UserResponse();
            try
            {
                response.User = unitOfWork.GetUserRepository().GetUser(id).ConvertToUserViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Create(UserViewModel user)
        {
            UserResponse response = new UserResponse();
            try
            {
                response.User = unitOfWork.GetUserRepository().Create(user.ConvertToUser())
                    .ConvertToUserViewModel();
                unitOfWork.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Update(UserViewModel user)
        {
            UserResponse response = new UserResponse();
            try
            {
                response.User = unitOfWork.GetUserRepository().Update(user.ConvertToUser())
                    .ConvertToUserViewModel();
                unitOfWork.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Delete(int id)
        {
            UserResponse response = new UserResponse();
            try
            {
                response.User = unitOfWork.GetUserRepository().Delete(id)
                    .ConvertToUserViewModel();
                unitOfWork.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
