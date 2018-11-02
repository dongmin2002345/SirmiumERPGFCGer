using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeByBusinessPartnerHistoryRepository
    {
        List<EmployeeByBusinessPartnerHistory> GetEmployeeByBusinessPartnerHistories(int companyId);
        List<EmployeeByBusinessPartnerHistory> GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeByBusinessPartnerHistory Create(EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory);
        EmployeeByBusinessPartnerHistory Delete(Guid identifier);
    }
}
