using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Vats
{
    public class SyncVatRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
