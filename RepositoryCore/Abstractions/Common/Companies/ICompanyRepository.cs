using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Companies
{
    public interface ICompanyRepository
    {
        List<Company> GetCompanies();
        List<Company> GetCompaniesNewerThan(DateTime dateFrom);

        Company Create(Company company);
        Company Delete(int id);
    }
}
