using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Prices
{
    public class SyncServiceDeliveryRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
