using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IPhysicalPersonAttachmentService
    {
        PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachments(int companyId);
        PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime);

        PhysicalPersonAttachmentResponse Create(PhysicalPersonAttachmentViewModel PhysicalPersonAttachment);
        PhysicalPersonAttachmentListResponse CreateList(List<PhysicalPersonAttachmentViewModel> PhysicalPersonAttachment);
        PhysicalPersonAttachmentResponse Delete(Guid identifier);

        PhysicalPersonAttachmentListResponse Sync(SyncPhysicalPersonAttachmentRequest request);
    }
}
