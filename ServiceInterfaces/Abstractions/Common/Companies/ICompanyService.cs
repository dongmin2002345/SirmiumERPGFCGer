using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Companies
{
    public interface ICompanyService
    {
        CompanyListResponse GetCompanies();
        CompanyResponse GetCompany(int id);
        CompanyResponse GetNewCodeValue();
        CompanyResponse Create(CompanyViewModel comp);
        CompanyResponse Update(CompanyViewModel comp);
        CompanyResponse Delete(int id);
    }
}

