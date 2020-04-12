using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class DocumentFolderResponse : BaseResponse
    {
        public DocumentFolderViewModel DocumentFolder { get; set; }
    }
}
