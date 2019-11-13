using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeAttachmentService
    {
        EmployeeAttachmentListResponse GetEmployeeAttachments(int companyId);
        EmployeeAttachmentListResponse GetEmployeeAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeAttachmentResponse Create(EmployeeAttachmentViewModel EmployeeAttachment);
        EmployeeAttachmentListResponse CreateList(List<EmployeeAttachmentViewModel> EmployeeAttachment);
        EmployeeAttachmentResponse Delete(Guid identifier);

        EmployeeAttachmentListResponse Sync(SyncEmployeeAttachmentRequest request);
    }
}
