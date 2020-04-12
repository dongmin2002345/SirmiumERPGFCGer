using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class SyncDocumentFolderRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 500;
    }
}
