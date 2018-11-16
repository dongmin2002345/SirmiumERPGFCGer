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
    public class EmployeeCardService : IEmployeeCardService
    {
        public EmployeeCardListResponse Sync(SyncEmployeeCardRequest request)
        {
            EmployeeCardListResponse response = new EmployeeCardListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeCardRequest, EmployeeCardViewModel, EmployeeCardListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeCards = new List<EmployeeCardViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
