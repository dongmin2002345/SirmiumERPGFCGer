using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Shipments
{
    public class SyncShipmentDocumentRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
