using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeByConstructionSiteHistoryService
    {
        EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistories(int companyId);
        EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeByConstructionSiteHistoryResponse Create(EmployeeByConstructionSiteHistoryViewModel employeeByConstructionSiteHistory);
        EmployeeByConstructionSiteHistoryResponse Delete(Guid identifier);

        EmployeeByConstructionSiteHistoryListResponse Sync(SyncEmployeeByConstructionSiteHistoryRequest request);
    }
}
