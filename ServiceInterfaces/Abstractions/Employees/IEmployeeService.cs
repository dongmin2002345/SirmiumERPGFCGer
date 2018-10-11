using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeService
    {
        EmployeeListResponse GetEmployees(int companyId);
        EmployeeListResponse GetEmployeesNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeResponse Create(EmployeeViewModel Employee);

        EmployeeListResponse Sync(SyncEmployeeRequest request);
    }
}
