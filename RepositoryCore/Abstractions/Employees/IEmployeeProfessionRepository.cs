using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeProfessionRepository
    {
        List<EmployeeProfession> GetEmployeeItems(int companyId);
        List<EmployeeProfession> GetEmployeeItemsByEmployee(int EmployeeId);
        List<EmployeeProfession> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime);

        EmployeeProfession Create(EmployeeProfession EmployeeItem);
        EmployeeProfession Delete(Guid identifier);
    }
}
