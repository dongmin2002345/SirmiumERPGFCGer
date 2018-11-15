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
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        public EmployeeDocumentListResponse Sync(SyncEmployeeDocumentRequest request)
        {
            EmployeeDocumentListResponse response = new EmployeeDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeDocumentRequest, EmployeeDocumentViewModel, EmployeeDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeDocuments = new List<EmployeeDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
