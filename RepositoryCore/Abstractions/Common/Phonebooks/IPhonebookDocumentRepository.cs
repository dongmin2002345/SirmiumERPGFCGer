using DomainCore.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Phonebooks
{
    public interface IPhonebookDocumentRepository
    {
        List<PhonebookDocument> GetPhonebookDocuments(int companyId);
        List<PhonebookDocument> GetPhonebookDocumentsByPhonebook(int PhonebookId);
        List<PhonebookDocument> GetPhonebookDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        PhonebookDocument Create(PhonebookDocument phonebookDocument);
        PhonebookDocument Delete(Guid identifier);
    }
}
