using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Identity
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(int id);
        User Create(User user);
        User Update(User user);
        User Delete(int id);
    }
}

