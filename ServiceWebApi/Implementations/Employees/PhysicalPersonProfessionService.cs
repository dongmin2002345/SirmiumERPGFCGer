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
    public class PhysicalPersonProfessionService : IPhysicalPersonProfessionService
    {
        public PhysicalPersonProfessionListResponse GetPhysicalPersonItems(int companyId)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<PhysicalPersonProfessionViewModel>, PhysicalPersonProfessionListResponse>("GetPhysicalPersonItems", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<PhysicalPersonProfessionViewModel>, PhysicalPersonProfessionListResponse>("GetPhysicalPersonItemsNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionResponse Create(PhysicalPersonProfessionViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();
            try
            {
                response = WpfApiHandler.SendToApi<PhysicalPersonProfessionViewModel, PhysicalPersonProfessionResponse>(PhysicalPersonItemViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfession = new PhysicalPersonProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionListResponse Sync(SyncPhysicalPersonProfessionRequest request)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonProfessionRequest, PhysicalPersonProfessionViewModel, PhysicalPersonProfessionListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
