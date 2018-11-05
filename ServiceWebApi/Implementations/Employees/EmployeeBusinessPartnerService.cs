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
    public class EmployeeBusinessPartnerService : IEmployeeByBusinessPartnerService
    {
        public EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartners(int companyId)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByBusinessPartnerViewModel>, EmployeeByBusinessPartnerListResponse>("GetEmployeeByBusinessPartners", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<EmployeeByBusinessPartnerViewModel>, EmployeeByBusinessPartnerListResponse>("GetEmployeeByBusinessPartnersNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerResponse Create(EmployeeByBusinessPartnerViewModel employeeByBusinessPartner)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeByBusinessPartnerViewModel, EmployeeByBusinessPartnerResponse>(employeeByBusinessPartner, "Create");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartner = new EmployeeByBusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerResponse Delete(Guid identifier)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                EmployeeByBusinessPartnerViewModel re = new EmployeeByBusinessPartnerViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<EmployeeByBusinessPartnerViewModel, EmployeeByBusinessPartnerResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartner = new EmployeeByBusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerListResponse Sync(SyncEmployeeByBusinessPartnerRequest request)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncEmployeeByBusinessPartnerRequest, EmployeeByBusinessPartnerViewModel, EmployeeByBusinessPartnerListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
