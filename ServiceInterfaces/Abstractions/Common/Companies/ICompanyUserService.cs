using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Companies
{
    public interface ICompanyUserService
    {
        CompanyUserListResponse GetCompanyUsers();
        CompanyUserListResponse GetCompanyUsersNewerThan(DateTime? dateFrom);

        CompanyUserResponse Create(CompanyUserViewModel companyUser);
        CompanyUserResponse Delete(Guid identifier);

        CompanyUserListResponse Sync(SyncCompanyUserRequest request);
    }
}
