using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Individuals
{
    public class IndividualService : IIndividualService
    {
        /// <summary>
        /// Get all active business partners for selected company
        /// </summary>
        /// <returns></returns>
        public IndividualListResponse GetIndividuals(string filterString)
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<IndividualViewModel>, IndividualListResponse>("GetIndividuals",
                    new Dictionary<string, string>() {
                        { "FilterString", filterString }
                    });
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Individuals = new List<IndividualViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public IndividualListResponse GetIndividualsForPopup(string filterString)
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FilterString", filterString);

                response = WpfApiHandler.GetFromApi<List<IndividualViewModel>, IndividualListResponse>("GetIndividualsForPopup", parameters);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public IndividualListResponse GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 6, string IndividualName = "")
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<IndividualViewModel>, IndividualListResponse>("GetIndividualsByPage", new Dictionary<string, string>()
                {
                    { "CurrentPage", currentPage.ToString() },
                    { "itemsPerPage", itemsPerPage.ToString() },
                    { "IndividualName", IndividualName },
                });
                //response.TotalItems = WpfApiHandler.GetFromApi<int, IndividualViewModel, IndividualListResponse>("GetIndividualsCount", new Dictionary<string, string>() {
                //    { "CompanyID", companyId.ToString() },
                //    { "searchParameter", IndividualName }
                //}).TotalItems;

                response.Success = true;
            }

            catch (Exception ex)
            {
                response = new IndividualListResponse();
                response.IndividualsByPage = new List<IndividualViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public IndividualListResponse GetIndividualsCount(string searchParameter = "")
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("searchParameter", searchParameter);

                response = WpfApiHandler.GetFromApi<List<IndividualViewModel>, IndividualListResponse>("GetIndividualsCount", parameters);
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
        /// Get single active business partner for selected company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndividualResponse GetIndividual(int id)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<IndividualViewModel, IndividualResponse>("GetIndividual", new Dictionary<string, string>()
                {
                    { "id", id.ToString() },
                });
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
        public IndividualCodeResponse GetNewCodeValue()
        {
            IndividualCodeResponse response = new IndividualCodeResponse();

            try
            {
                response = WpfApiHandler.GetFromApi<int, IndividualViewModel, IndividualCodeResponse>("GetNewCodeValue", null);
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
        /// Create new business partner
        /// </summary>
        /// <param name="Individual"></param>
        /// <returns></returns>
        public IndividualResponse Create(IndividualViewModel Individual)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                response = WpfApiHandler.SendToApi<IndividualViewModel, IndividualResponse>(Individual, "Create");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Individual = new IndividualViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Update business partner 
        /// </summary>
        /// <param name="Individual"></param>
        /// <returns></returns>
        public IndividualResponse Update(IndividualViewModel Individual)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                response = WpfApiHandler.SendToApi<IndividualViewModel, IndividualResponse>(Individual, "Update");
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
        public IndividualResponse Delete(int id)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                IndividualViewModel viewModel = new IndividualViewModel();
                viewModel.Id = id;
                response = WpfApiHandler.SendToApi<IndividualViewModel, IndividualResponse>(viewModel, "Delete");
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

