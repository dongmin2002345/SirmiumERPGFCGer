using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeAttachmentRepository
    {
        List<EmployeeAttachment> GetEmployeeAttachments(int companyId);
        List<EmployeeAttachment> GetEmployeeAttachmentsByEmployee(int EmployeeId);
        List<EmployeeAttachment> GetEmployeeAttachmentsNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeAttachment Create(EmployeeAttachment attachment);
        EmployeeAttachment Delete(Guid identifier);
    }
}
