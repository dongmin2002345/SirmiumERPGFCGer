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
    public class EmployeeLicenceService : IEmployeeLicenceService
    {
        public EmployeeLicenceItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<EmployeeLicenceItemViewModel>, EmployeeLicenceItemListResponse>("GetEmployeeItems", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeLicenceItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeLicenceItemViewModel>, EmployeeLicenceItemListResponse>("GetEmployeeItemsNewerThen", new Dictionary<string, string>()
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

        public EmployeeLicenceItemResponse Create(EmployeeLicenceItemViewModel EmployeeItemViewModel)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeLicenceItemViewModel, EmployeeLicenceItemResponse>(EmployeeItemViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItem = new EmployeeLicenceItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeLicenceItemListResponse Sync(SyncEmployeeLicenceItemRequest request)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeLicenceItemRequest, EmployeeLicenceItemViewModel, EmployeeLicenceItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
