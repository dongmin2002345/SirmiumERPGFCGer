using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.DocumentStores
{
    public interface IDocumentFolderService
    {
        DocumentFolderListResponse Sync(SyncDocumentFolderRequest request);
        DocumentFolderResponse Create(DocumentFolderViewModel toCreate);
        DocumentFolderListResponse SubmitList(List<DocumentFolderViewModel> toCreate);
        DocumentFolderResponse Delete(DocumentFolderViewModel toDelete);

        DocumentFolderResponse Clear(int companyId);
        
    }
}
