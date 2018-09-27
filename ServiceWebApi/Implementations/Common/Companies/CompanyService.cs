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

        /// <summary>
        /// Get all active Companies from database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get single active Company by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyResponse GetCompany(int id)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("ID", id.ToString());


                response.Company = WpfApiHandler.GetFromApi<CompanyViewModel>("GetCompany", parameters);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        ///<summary>
        /// Gets new code for business partner creation
        ///</summary>
        ///<returns></returns>
        public CompanyResponse GetNewCodeValue()
        {
            CompanyResponse response = new CompanyResponse();

            try
            {
                response = WpfApiHandler.GetFromApi<CompanyViewModel, CompanyResponse>("GetNewCodeValue", null);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Create new company
        /// </summary>
        /// <param name="Company"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update company 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public CompanyResponse Update(CompanyViewModel company)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CompanyViewModel, CompanyResponse>(company, "Update");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Deactivate business partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

    }

}
