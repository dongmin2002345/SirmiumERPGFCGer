using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Statuses
{
    public class SyncStatusRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
