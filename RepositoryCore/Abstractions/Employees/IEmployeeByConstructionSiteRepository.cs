using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeByConstructionSiteRepository
    {
        List<EmployeeByConstructionSite> GetEmployeeByConstructionSites(int companyId);
        List<EmployeeByConstructionSite> GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeByConstructionSite Create(EmployeeByConstructionSite employeeByConstructionSite);
        EmployeeByConstructionSite Delete(EmployeeByConstructionSite employeeByConstructionSite);
    }
}
