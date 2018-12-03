using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Companies
{
    public interface ICompanyUserRepository
    {
        List<CompanyUser> GetCompanyUsers();
        List<CompanyUser> GetCompanyUsersNewerThan(DateTime dateFrom);
        List<CompanyUser> GetCompanyUsersByUser(int userId);

        CompanyUser Create(CompanyUser compUser);
        CompanyUser Delete(Guid identifier);
    }
}
