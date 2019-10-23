using DomainCore.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Phonebooks
{
    public interface IPhonebookRepository
    {
        List<Phonebook> GetPhonebooks(int companyId);
        List<Phonebook> GetPhonebooksNewerThen(int companyId, DateTime lastUpdateTime);

        Phonebook GetPhonebook(int id);

        Phonebook Create(Phonebook phonebook);
        Phonebook Delete(Guid identifier);
    }
}
