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
    public class EmployeeByConstructionSiteHistoryService : IEmployeeByConstructionSiteHistoryService
    {
        public EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistories(int companyId)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByConstructionSiteHistoryViewModel>, EmployeeByConstructionSiteHistoryListResponse>("GetEmployeeByConstructionSiteHistories", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByConstructionSiteHistoryViewModel>, EmployeeByConstructionSiteHistoryListResponse>("GetEmployeeByConstructionSiteHistoriesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryResponse Create(EmployeeByConstructionSiteHistoryViewModel re)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeByConstructionSiteHistoryViewModel, EmployeeByConstructionSiteHistoryResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistory = new EmployeeByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryResponse Delete(Guid identifier)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                EmployeeByConstructionSiteHistoryViewModel re = new EmployeeByConstructionSiteHistoryViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<EmployeeByConstructionSiteHistoryViewModel, EmployeeByConstructionSiteHistoryResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistory = new EmployeeByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryListResponse Sync(SyncEmployeeByConstructionSiteHistoryRequest request)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeByConstructionSiteHistoryRequest, EmployeeByConstructionSiteHistoryViewModel, EmployeeByConstructionSiteHistoryListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
