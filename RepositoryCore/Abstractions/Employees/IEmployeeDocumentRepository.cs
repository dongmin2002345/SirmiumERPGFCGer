using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeDocumentRepository
    {
        List<EmployeeDocument> GetEmployeeDocuments(int companyId);
        List<EmployeeDocument> GetEmployeeDocumentsByEmployee(int employeeId);
        List<EmployeeDocument> GetEmployeeDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeDocument Create(EmployeeDocument employeeDocument);
        EmployeeDocument Delete(Guid identifier);
    }
}
