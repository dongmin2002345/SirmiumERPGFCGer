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
    public class PhysicalPersonNoteService : IPhysicalPersonNoteService
    {
        public PhysicalPersonNoteListResponse Sync(SyncPhysicalPersonNoteRequest request)
        {
            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonNoteRequest, PhysicalPersonNoteViewModel, PhysicalPersonNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
