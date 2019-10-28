using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.CallCentars
{
    public class SyncCallCentarRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
