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
    public class EmployeeNoteService : IEmployeeNoteService
    {
        public EmployeeNoteListResponse Sync(SyncEmployeeNoteRequest request)
        {
            EmployeeNoteListResponse response = new EmployeeNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeNoteRequest, EmployeeNoteViewModel, EmployeeNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeNotes = new List<EmployeeNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
