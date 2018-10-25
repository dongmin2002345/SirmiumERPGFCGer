using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeLicenceRepository
    {
        List<EmployeeLicence> GetEmployeeItems(int companyId);
        List<EmployeeLicence> GetEmployeeItemsByEmployee(int EmployeeId);
        List<EmployeeLicence> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime);

        EmployeeLicence Create(EmployeeLicence EmployeeItem);
        EmployeeLicence Delete(Guid identifier);
    }
}
