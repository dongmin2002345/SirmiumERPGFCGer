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
    public class EmployeeService : IEmployeeService
    {
        public EmployeeListResponse GetEmployees(int companyId)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<EmployeeViewModel, EmployeeListResponse>("GetEmployees",
                    new Dictionary<string, string>() { { "CompanyID", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeListResponse GetEmployeesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeViewModel>, EmployeeListResponse>("GetEmployeesNewerThen",
                   new Dictionary<string, string>() {
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

        public EmployeeResponse Create(EmployeeViewModel Employee)
        {
            EmployeeResponse response = new EmployeeResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeViewModel, EmployeeResponse>(Employee, "Create");
            }
            catch (Exception ex)
            {
                response.Employee = new EmployeeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public EmployeeResponse Delete(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();
            try
            {
                response = WpfApiHandler.SendToApi<Guid, EmployeeViewModel, EmployeeResponse>(identifier, "Delete");
            }
            catch (Exception ex)
            {
                response.Employee = new EmployeeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public EmployeeListResponse Sync(SyncEmployeeRequest request)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeRequest, EmployeeViewModel, EmployeeListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Employees = new List<EmployeeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
