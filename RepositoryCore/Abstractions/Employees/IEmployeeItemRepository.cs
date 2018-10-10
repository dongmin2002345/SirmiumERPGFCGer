using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeItemRepository
    {
        List<EmployeeItem> GetEmployeeItems(int companyId);
        List<EmployeeItem> GetEmployeeItemsByEmployee(int EmployeeId);
        List<EmployeeItem> GetEmployeeItemsNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeItem Create(EmployeeItem EmployeeItem);
        EmployeeItem Delete(Guid identifier);
    }
}
