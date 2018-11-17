using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeNoteRepository
    {
        List<EmployeeNote> GetEmployeeNotes(int companyId);
        List<EmployeeNote> GetEmployeeNotesByEmployee(int EmployeeId);
        List<EmployeeNote> GetEmployeeNotesNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeNote Create(EmployeeNote EmployeeNote);
        EmployeeNote Delete(Guid identifier);
    }
}
