using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonAttachmentRepository
    {
        List<PhysicalPersonAttachment> GetPhysicalPersonAttachments(int companyId);
        List<PhysicalPersonAttachment> GetPhysicalPersonAttachmentsByPhysicalPerson(int PhysicalPersonId);
        List<PhysicalPersonAttachment> GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime lastUpdateTime);

        PhysicalPersonAttachment Create(PhysicalPersonAttachment attachment);
        PhysicalPersonAttachment Delete(Guid identifier);
    }
}
