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
    public class PhysicalPersonCardService : IPhysicalPersonCardService
    {
        public PhysicalPersonCardListResponse Sync(SyncPhysicalPersonCardRequest request)
        {
            PhysicalPersonCardListResponse response = new PhysicalPersonCardListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonCardRequest, PhysicalPersonCardViewModel, PhysicalPersonCardListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonCards = new List<PhysicalPersonCardViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
