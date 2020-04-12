using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.DocumentStores
{
    public interface IDocumentFileService
    {
        DocumentFileListResponse Sync(SyncDocumentFileRequest request);
        DocumentFileResponse Create(DocumentFileViewModel toCreate);
        DocumentFileListResponse SubmitList(List<DocumentFileViewModel> toCreate);
        DocumentFileResponse Delete(DocumentFileViewModel toDelete);

        DocumentFileResponse Clear(int companyId);
        
    }
}
