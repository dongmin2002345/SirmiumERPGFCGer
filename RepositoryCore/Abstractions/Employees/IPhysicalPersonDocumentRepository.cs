using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonDocumentRepository
    {
        List<PhysicalPersonDocument> GetPhysicalPersonDocuments(int companyId);
        List<PhysicalPersonDocument> GetPhysicalPersonDocumentsByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonDocument> GetPhysicalPersonDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        PhysicalPersonDocument Create(PhysicalPersonDocument physicalPersonDocument);
        PhysicalPersonDocument Delete(Guid identifier);
    }
}
