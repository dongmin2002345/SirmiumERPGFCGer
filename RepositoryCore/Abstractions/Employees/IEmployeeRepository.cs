using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees(int companyId);
        Employee GetEmployee(int employeeId);
        Employee GetEmployeeEntity(int employeeId);
        List<Employee> GetEmployeesNewerThen(int companyId, DateTime lastUpdateTime);

        Employee Create(Employee Employee);
        Employee Delete(Guid identifier);
    }
}
