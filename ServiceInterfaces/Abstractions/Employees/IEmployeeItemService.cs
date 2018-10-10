using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeItemService
    {
        EmployeeItemListResponse GetEmployeeItems(int companyId);
        EmployeeItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeItemResponse Create(EmployeeItemViewModel EmployeeItem);

        EmployeeItemListResponse Sync(SyncEmployeeItemRequest request);
    }
}
