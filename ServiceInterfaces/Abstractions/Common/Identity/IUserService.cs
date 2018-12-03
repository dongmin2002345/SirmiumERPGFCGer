using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Identity
{
    public interface IUserService
    {
        UserListResponse GetUsers(int companyId);
        UserListResponse GetUsersNewerThan(int companyId, DateTime? lastUpdateTime);

        UserResponse Create(UserViewModel user);
        UserResponse Delete(Guid identifier);

        UserListResponse Sync(SyncUserRequest request);
    }
}
