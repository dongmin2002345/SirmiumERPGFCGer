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
    public class EmployeeProfessionService : IEmployeeProfessionService
    {
        public EmployeeProfessionItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<EmployeeProfessionItemViewModel>, EmployeeProfessionItemListResponse>("GetEmployeeItems", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeProfessionItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeProfessionItemViewModel>, EmployeeProfessionItemListResponse>("GetEmployeeItemsNewerThen", new Dictionary<string, string>()
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

        public EmployeeProfessionItemResponse Create(EmployeeProfessionItemViewModel EmployeeItemViewModel)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeProfessionItemViewModel, EmployeeProfessionItemResponse>(EmployeeItemViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItem = new EmployeeProfessionItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeProfessionItemListResponse Sync(SyncEmployeeProfessionItemRequest request)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeProfessionItemRequest, EmployeeProfessionItemViewModel, EmployeeProfessionItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
