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
    public class EmployeeItemService : IEmployeeItemService
    {
        public EmployeeItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<EmployeeItemViewModel>, EmployeeItemListResponse>("GetEmployeeItems", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeItemViewModel>, EmployeeItemListResponse>("GetEmployeeItemsNewerThen", new Dictionary<string, string>()
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

        public EmployeeItemResponse Create(EmployeeItemViewModel EmployeeItemViewModel)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeItemViewModel, EmployeeItemResponse>(EmployeeItemViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeItem = new EmployeeItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeItemListResponse Sync(SyncEmployeeItemRequest request)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeItemRequest, EmployeeItemViewModel, EmployeeItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
