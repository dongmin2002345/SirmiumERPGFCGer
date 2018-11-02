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
    public class EmployeeByConstructionSiteService : IEmployeeByConstructionSiteService
    {
        public EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSites(int companyId)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByConstructionSiteViewModel>, EmployeeByConstructionSiteListResponse>("GetEmployeeByConstructionSites", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByConstructionSiteViewModel>, EmployeeByConstructionSiteListResponse>("GetEmployeeByConstructionSitesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteResponse Create(EmployeeByConstructionSiteViewModel employeeByConstructionSite)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeByConstructionSiteViewModel, EmployeeByConstructionSiteResponse>(employeeByConstructionSite, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSite = new EmployeeByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteResponse Delete(Guid identifier)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                EmployeeByConstructionSiteViewModel re = new EmployeeByConstructionSiteViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<EmployeeByConstructionSiteViewModel, EmployeeByConstructionSiteResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSite = new EmployeeByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteListResponse Sync(SyncEmployeeByConstructionSiteRequest request)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeByConstructionSiteRequest, EmployeeByConstructionSiteViewModel, EmployeeByConstructionSiteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
