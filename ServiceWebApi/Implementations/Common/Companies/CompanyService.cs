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
    public class CompanyService : ICompanyService
    {
        public CompanyListResponse GetCompanies()
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CompanyViewModel>, CompanyListResponse>("GetCompanies", null);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyResponse Create(CompanyViewModel company)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CompanyViewModel, CompanyResponse>(company, "Create");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyResponse Delete(int id)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                CompanyViewModel viewModel = new CompanyViewModel();
                viewModel.Id = id;
                response = WpfApiHandler.SendToApi<CompanyViewModel, CompanyResponse>(viewModel, "Delete");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyListResponse GetCompaniesNewerThan(DateTime? dateFrom)
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CompanyViewModel>, CompanyListResponse>("GetCompaniesNewerThan", new Dictionary<string, string>()
                {
                    { "dateFrom", dateFrom.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Companies = new List<CompanyViewModel>();
                response.Message = ex.Message;
            }

            return response;
        }

        public CompanyListResponse Sync(SyncCompanyRequest request)
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCompanyRequest, CompanyViewModel, CompanyListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }

}
