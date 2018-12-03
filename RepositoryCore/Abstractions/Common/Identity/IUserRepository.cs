using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Identity
{
    public interface IUserRepository
    {
        List<User> GetUsers(int companyId);
        List<User> GetUsersNewerThan(int companyId, DateTime lastUpdateTime);

        User Create(User user);
        User Delete(Guid identifier);
    }
}

