using DomainCore.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Phonebooks
{
    public interface IPhonebookNoteRepository
    {
        List<PhonebookNote> GetPhonebookNotes(int companyId);
        List<PhonebookNote> GetPhonebookNotesByPhonebook(int PhonebookId);
        List<PhonebookNote> GetPhonebookNotesNewerThen(int companyId, DateTime lastUpdateTime);

        PhonebookNote Create(PhonebookNote phonebookNote);
        PhonebookNote Delete(Guid identifier);
    }
}
