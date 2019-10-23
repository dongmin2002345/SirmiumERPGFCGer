using DomainCore.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Phonebooks
{
    public interface IPhonebookPhoneRepository
    {
        List<PhonebookPhone> GetPhonebookPhones(int companyId);
        List<PhonebookPhone> GetPhonebookPhonesByPhonebook(int PhonebookId);
        List<PhonebookPhone> GetPhonebookPhonesNewerThen(int companyId, DateTime lastUpdateTime);

        PhonebookPhone Create(PhonebookPhone phonebookPhone);
        PhonebookPhone Delete(Guid identifier);
    }
}
