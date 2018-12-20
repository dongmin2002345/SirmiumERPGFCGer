using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonCardRepository
    {
        List<PhysicalPersonCard> GetPhysicalPersonCards(int companyId);
        List<PhysicalPersonCard> GetPhysicalPersonCardsByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonCard> GetPhysicalPersonCardsNewerThen(int companyId, DateTime lastUpdateTime);

        PhysicalPersonCard Create(PhysicalPersonCard physicalPersonCard);
        PhysicalPersonCard Delete(Guid identifier);
    }
}
