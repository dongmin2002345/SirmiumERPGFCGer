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
    public class EmployeeByBusinessPartnerHistoryService : IEmployeeByBusinessPartnerHistoryService
    {
        public EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistories(int companyId)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByBusinessPartnerHistoryViewModel>, EmployeeByBusinessPartnerHistoryListResponse>("GetEmployeeByBusinessPartnerHistories", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByBusinessPartnerHistoryViewModel>, EmployeeByBusinessPartnerHistoryListResponse>("GetEmployeeByBusinessPartnerHistoriesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryResponse Create(EmployeeByBusinessPartnerHistoryViewModel re)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeByBusinessPartnerHistoryViewModel, EmployeeByBusinessPartnerHistoryResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistory = new EmployeeByBusinessPartnerHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryResponse Delete(Guid identifier)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                EmployeeByBusinessPartnerHistoryViewModel re = new EmployeeByBusinessPartnerHistoryViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<EmployeeByBusinessPartnerHistoryViewModel, EmployeeByBusinessPartnerHistoryResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistory = new EmployeeByBusinessPartnerHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryListResponse Sync(SyncEmployeeByBusinessPartnerHistoryRequest request)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeByBusinessPartnerHistoryRequest, EmployeeByBusinessPartnerHistoryViewModel, EmployeeByBusinessPartnerHistoryListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
