using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Employees
{
    public class SyncEmployeeByBusinessPartnerHistoryRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdateAt { get; set; }
    }
}
