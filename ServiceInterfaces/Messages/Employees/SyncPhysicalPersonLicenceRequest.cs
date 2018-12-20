using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Employees
{
    public class SyncPhysicalPersonLicenceRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public List<PhysicalPersonProfessionViewModel> UnSyncedPhysicalPersonProfessions { get; set; }
    }
}
