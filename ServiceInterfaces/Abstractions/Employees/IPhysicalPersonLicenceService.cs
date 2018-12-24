using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IPhysicalPersonLicenceService
    {
        PhysicalPersonLicenceListResponse GetPhysicalPersonItems(int companyId);
        PhysicalPersonLicenceListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime);

        PhysicalPersonLicenceResponse Create(PhysicalPersonLicenceViewModel PhysicalPersonItem);

        PhysicalPersonLicenceListResponse Sync(SyncPhysicalPersonLicenceRequest request);
    }
}
