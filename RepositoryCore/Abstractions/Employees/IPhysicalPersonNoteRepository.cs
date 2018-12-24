using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IPhysicalPersonNoteRepository
    {
        List<PhysicalPersonNote> GetPhysicalPersonNotes(int companyId);
        List<PhysicalPersonNote> GetPhysicalPersonNotesByPhysicalPerson(int physicalPersonId);
        List<PhysicalPersonNote> GetPhysicalPersonNotesNewerThen(int companyId, DateTime lastUpdateTime);

        PhysicalPersonNote Create(PhysicalPersonNote physicalPersonNote);
        PhysicalPersonNote Delete(Guid identifier);
    }
}
