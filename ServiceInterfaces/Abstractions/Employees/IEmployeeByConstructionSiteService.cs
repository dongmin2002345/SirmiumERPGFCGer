using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeByConstructionSiteService
    {
        EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSites(int companyId);
        EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeByConstructionSiteResponse Create(EmployeeByConstructionSiteViewModel employeeByConstructionSite);
        EmployeeByConstructionSiteResponse Delete(Guid identifier);

        EmployeeByConstructionSiteListResponse Sync(SyncEmployeeByConstructionSiteRequest request);
    }
}
