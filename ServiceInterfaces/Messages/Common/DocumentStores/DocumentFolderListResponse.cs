using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class DocumentFolderListResponse : BaseResponse
    {
        public List<DocumentFolderViewModel> DocumentFolders { get; set; }
        public int TotalItems { get; set; }
    }
}
