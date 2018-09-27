using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        public UserResponse Authenticate(string username, string password)
        {
            AuthenticationViewModel user = new AuthenticationViewModel();
            UserResponse response = new UserResponse();
            try
            {
                user.Username = username;
                user.Password = password;
                response = WpfApiHandler.SendToApi<AuthenticationViewModel, AuthenticationViewModel, UserResponse>(user, "Authenticate");
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
