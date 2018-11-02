using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeByConstructionSiteHistoryRepository
    {
        List<EmployeeByConstructionSiteHistory> GetEmployeeByConstructionSiteHistories(int companyId);
        List<EmployeeByConstructionSiteHistory> GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeByConstructionSiteHistory Create(EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory);
        EmployeeByConstructionSiteHistory Delete(Guid identifier);
    }
}
