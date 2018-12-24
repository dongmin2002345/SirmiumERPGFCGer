using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IPhysicalPersonProfessionService
    {
        PhysicalPersonProfessionListResponse GetPhysicalPersonItems(int companyId);
        PhysicalPersonProfessionListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime);

        PhysicalPersonProfessionResponse Create(PhysicalPersonProfessionViewModel PhysicalPersonItem);

        PhysicalPersonProfessionListResponse Sync(SyncPhysicalPersonProfessionRequest request);
    }
}
