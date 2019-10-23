using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Phonebook
{
    public interface IPhonebookService
    {
        PhonebookListResponse GetPhonebooks(int companyId);
        PhonebookListResponse GetPhonebooksNewerThen(int companyId, DateTime? lastUpdateTime);

        PhonebookResponse Create(PhonebookViewModel phonebook);
        PhonebookResponse Delete(Guid identifier);

        PhonebookListResponse Sync(SyncPhonebookRequest request);
    }
}
