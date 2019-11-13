using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Employees
{
    public class SyncPhysicalPersonAttachmentRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
