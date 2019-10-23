using ServiceInterfaces.Messages.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Phonebook
{
    public interface IPhonebookNoteService
    {
        PhonebookNoteListResponse Sync(SyncPhonebookNoteRequest request);
    }
}
