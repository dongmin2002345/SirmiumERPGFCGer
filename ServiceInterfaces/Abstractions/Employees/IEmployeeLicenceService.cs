using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeLicenceService
    {
        EmployeeLicenceItemListResponse GetEmployeeItems(int companyId);
        EmployeeLicenceItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeLicenceItemResponse Create(EmployeeLicenceItemViewModel EmployeeItem);

        EmployeeLicenceItemListResponse Sync(SyncEmployeeLicenceItemRequest request);
    }
}
