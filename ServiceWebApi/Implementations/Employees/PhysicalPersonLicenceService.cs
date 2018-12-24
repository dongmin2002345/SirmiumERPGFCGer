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
    public class PhysicalPersonLicenceService : IPhysicalPersonLicenceService
    {
        public PhysicalPersonLicenceListResponse GetPhysicalPersonItems(int companyId)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<PhysicalPersonLicenceViewModel>, PhysicalPersonLicenceListResponse>("GetPhysicalPersonItems", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonLicenceListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<PhysicalPersonLicenceViewModel>, PhysicalPersonLicenceListResponse>("GetPhysicalPersonItemsNewerThen", new Dictionary<string, string>()
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

        public PhysicalPersonLicenceResponse Create(PhysicalPersonLicenceViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();
            try
            {
                response = WpfApiHandler.SendToApi<PhysicalPersonLicenceViewModel, PhysicalPersonLicenceResponse>(PhysicalPersonItemViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicence = new PhysicalPersonLicenceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonLicenceListResponse Sync(SyncPhysicalPersonLicenceRequest request)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonLicenceRequest, PhysicalPersonLicenceViewModel, PhysicalPersonLicenceListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
