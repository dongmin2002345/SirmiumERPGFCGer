using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeProfessionService
    {
        EmployeeProfessionItemListResponse GetEmployeeItems(int companyId);
        EmployeeProfessionItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeProfessionItemResponse Create(EmployeeProfessionItemViewModel EmployeeItem);

        EmployeeProfessionItemListResponse Sync(SyncEmployeeProfessionItemRequest request);
    }
}
