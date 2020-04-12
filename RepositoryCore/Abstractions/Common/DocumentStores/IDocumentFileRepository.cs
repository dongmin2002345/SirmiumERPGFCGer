using DomainCore.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.DocumentStores
{
    public interface IDocumentFileRepository
    {
        List<DocumentFile> GetDocumentFiles(int companyId, DateTime? lastUpdate, int numOfPages = 1, int itemsPerPage = 500);

        DocumentFile GetDocumentFile(int id);

        DocumentFile Create(DocumentFile docFile);
        List<DocumentFile> SubmitList(List<DocumentFile> docFiles);
        DocumentFile Delete(int id);
        void Clear(int companyId);
    }
}
