using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Companies
{
    public class CompanyUserService : ICompanyUserService
    {
        public CompanyUserResponse Create(CompanyUserViewModel companyUser)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CompanyUserViewModel, CompanyUserResponse>(companyUser, "Create");
            }
            catch (Exception ex)
            {
                response.CompanyUser = new CompanyUserViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyUserResponse Delete(Guid identifier)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<CompanyUserViewModel, CompanyUserResponse>("Delete", new Dictionary<string, string>()
                {
                    { "identifier", identifier.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.CompanyUser = new CompanyUserViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyUserListResponse GetCompanyUsers()
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CompanyUserViewModel>, CompanyUserListResponse>("GetCompanyUsers");
            }
            catch (Exception ex)
            {
                response.CompanyUsers = new List<CompanyUserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyUserListResponse GetCompanyUsersNewerThan(DateTime? dateFrom)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CompanyUserViewModel>, CompanyUserListResponse>("GetCompanyUsersNewerThan", new Dictionary<string, string>()
                {
                    { "dateFrom", dateFrom.ToString() },
                });
            }
            catch (Exception ex)
            {
                response.CompanyUsers = new List<CompanyUserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyUserListResponse Sync(SyncCompanyUserRequest request)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCompanyUserRequest, List<CompanyUserViewModel>, CompanyUserListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.CompanyUsers = new List<CompanyUserViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
