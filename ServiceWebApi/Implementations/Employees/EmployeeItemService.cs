using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Employees
{
    public class EmployeeItemService : IEmployeeItemService
    {
        public EmployeeItemListResponse Sync(SyncEmployeeItemRequest request)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeItemRequest, EmployeeItemViewModel, EmployeeItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
