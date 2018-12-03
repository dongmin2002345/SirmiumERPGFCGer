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
        CompanyResponse Create(CompanyViewModel prod);
        CompanyResponse Delete(int id);

        CompanyListResponse GetCompaniesNewerThan(DateTime? dateFrom);
        CompanyListResponse Sync(SyncCompanyRequest request);
    }
}

