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
    public class PhysicalPersonItemService : IPhysicalPersonItemService
    {
        public PhysicalPersonItemListResponse Sync(SyncPhysicalPersonItemRequest request)
        {
            PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonItemRequest, PhysicalPersonItemViewModel, PhysicalPersonItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
