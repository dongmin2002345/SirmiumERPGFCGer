using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonProfessionRepository
    {
        List<PhysicalPersonProfession> GetPhysicalPersonItems(int companyId);
        List<PhysicalPersonProfession> GetPhysicalPersonItemsByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonProfession> GetPhysicalPersonItemsNewerThan(int companyId, DateTime lastUpdateTime);

        PhysicalPersonProfession Create(PhysicalPersonProfession physicalPersonItem);
        PhysicalPersonProfession Delete(Guid identifier);
    }
}
