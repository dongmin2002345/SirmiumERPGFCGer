using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Identity
{
    public class UserService : IUserService
    {
        public UserListResponse GetUsers()
        {
            UserListResponse response = new UserListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<UserViewModel>, UserListResponse>("GetUsers", new Dictionary<string, string>());
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
                response = WpfApiHandler.GetFromApi<UserViewModel, UserResponse>("GetUser", new Dictionary<string, string>() {
                    { "ID", id.ToString() }
                });
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
                response = WpfApiHandler.SendToApi<UserViewModel, UserViewModel, UserResponse>(user, "Create");
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
                response = WpfApiHandler.SendToApi<UserViewModel, UserViewModel, UserResponse>(user, "Update");
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
                response = WpfApiHandler.GetFromApi<UserViewModel, UserResponse>("Delete", new Dictionary<string, string>()
                {
                    { "id", id.ToString() }
                });
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

