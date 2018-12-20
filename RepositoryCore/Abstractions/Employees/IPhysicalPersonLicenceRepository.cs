using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonLicenceRepository
    {
        List<PhysicalPersonLicence> GetPhysicalPersonItems(int companyId);
        List<PhysicalPersonLicence> GetPhysicalPersonItemsByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonLicence> GetPhysicalPersonItemsNewerThan(int companyId, DateTime lastUpdateTime);

        PhysicalPersonLicence Create(PhysicalPersonLicence physicalPersonLicence);
        PhysicalPersonLicence Delete(Guid identifier);
    }
}
