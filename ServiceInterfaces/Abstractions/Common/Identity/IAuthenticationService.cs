using ServiceInterfaces.Messages.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Identity
{
    public interface IAuthenticationService
    {
        UserResponse Authenticate(string username, string password);
    }
}
