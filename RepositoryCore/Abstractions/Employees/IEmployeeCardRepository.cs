using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeCardRepository
    {
        List<EmployeeCard> GetEmployeeCards(int companyId);
        List<EmployeeCard> GetEmployeeCardsByEmployee(int employeeId);
        List<EmployeeCard> GetEmployeeCardsNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeCard Create(EmployeeCard employeeCard);
        EmployeeCard Delete(Guid identifier);
    }
}
