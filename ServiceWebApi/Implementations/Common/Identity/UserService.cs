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
        public UserListResponse GetUsers(int companyId)
        {
            UserListResponse response = new UserListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<UserViewModel>, UserListResponse>("GetUsers", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
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
                response = WpfApiHandler.GetFromApi<List<UserViewModel>, UserListResponse>("GetUsersNewerThan", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
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
                response = WpfApiHandler.SendToApi<UserViewModel, UserResponse>(re, "Create");
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
                UserViewModel re = new UserViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<UserViewModel, UserResponse>(re, "Delete");
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
            try
            {
                response = WpfApiHandler.SendToApi<SyncUserRequest, UserViewModel, UserListResponse>(request, "Sync");
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

