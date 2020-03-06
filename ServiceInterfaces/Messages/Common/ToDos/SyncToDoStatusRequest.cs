using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.ToDos
{
    public class SyncToDoStatusRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
