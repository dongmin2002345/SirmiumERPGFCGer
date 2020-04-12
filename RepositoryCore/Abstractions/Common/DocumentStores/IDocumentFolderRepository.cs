using DomainCore.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.DocumentStores
{
    public interface IDocumentFolderRepository
    {
        List<DocumentFolder> GetDocumentFolders(int companyId, DateTime? lastUpdate, int numOfPages = 1, int itemsPerPage = 500);

        DocumentFolder GetDocumentFolder(int id);

        DocumentFolder Create(DocumentFolder docFolder);
        List<DocumentFolder> SubmitList(List<DocumentFolder> docFolders);
        DocumentFolder Delete(int id);
        void Clear(int companyId);
    }
}
