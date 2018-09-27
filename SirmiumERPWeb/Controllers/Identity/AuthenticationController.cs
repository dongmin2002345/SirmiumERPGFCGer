using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceInterfaces.Abstractions.Common.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;

namespace SirmiumERPWeb.Controllers.Identity
{
    /*[Produces("application/json")]
    [Route("api/Authentication")]*/
    public class AuthenticationController : Controller
    {
        IAuthenticationService authService;
        public AuthenticationController(IServiceProvider provider)
        {
            authService = provider.GetRequiredService<IAuthenticationService>();
        }

        [HttpPost]
        public JsonResult Authenticate([FromBody] AuthenticationViewModel auth)
        {
            UserResponse response = new UserResponse();
            try
            {
                response = authService.Authenticate(auth.Username, auth.Password);
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return Json(response);
        }
    }
}