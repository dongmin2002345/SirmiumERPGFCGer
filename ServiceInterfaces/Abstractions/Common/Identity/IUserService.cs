using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Identity
{
    public interface IUserService
    {
        UserListResponse GetUsers();
        UserResponse GetUser(int id);
        UserResponse Create(UserViewModel user);
        UserResponse Update(UserViewModel user);
        UserResponse Delete(int id);
    }
}
