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
    public class PhysicalPersonDocumentService : IPhysicalPersonDocumentService
    {
        public PhysicalPersonDocumentListResponse Sync(SyncPhysicalPersonDocumentRequest request)
        {
            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonDocumentRequest, PhysicalPersonDocumentViewModel, PhysicalPersonDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonDocuments = new List<PhysicalPersonDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
