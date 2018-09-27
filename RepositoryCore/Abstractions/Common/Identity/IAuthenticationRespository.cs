using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Identity
{
    public interface IAuthenticationRepository
    {
        User Authenticate(string username, string password);
    }
}
