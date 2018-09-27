using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Companies
{
    public interface ICompanyRepository
    {
        List<Company> GetCompanies();
        Company GetCompany(int id);
        int GetNewCodeValue();

        Company Create(Company company);
        Company Update(Company company);
        Company Delete(int id);
    }
}
