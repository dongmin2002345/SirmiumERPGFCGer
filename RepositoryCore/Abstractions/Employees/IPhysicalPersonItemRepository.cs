using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonItemRepository
    {
        List<PhysicalPersonItem> GetPhysicalPersonItems(int companyId);
        List<PhysicalPersonItem> GetPhysicalPersonItemsByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonItem> GetPhysicalPersonItemsNewerThen(int companyId, DateTime lastUpdateTime);

        PhysicalPersonItem Create(PhysicalPersonItem physicalPersonItem);
        PhysicalPersonItem Delete(Guid identifier);
    }
}
