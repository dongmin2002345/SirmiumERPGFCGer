using DataMapper.Mappers.Common.Individuals;
using DomainCore.Common.Individuals;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Individuals
{
    public class IndividualService : IIndividualService
    {
        IUnitOfWork unitOfWork;

        /// <summary>
        /// individual service constructor
        /// </summary>
        /// <param name="IndividualRepository"></param>
        public IndividualService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all active individuals for selected company
        /// </summary>
        /// <returns></returns>
        public IndividualListResponse GetIndividuals(string filterString)
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                response.Individuals = unitOfWork.GetIndividualsRepository()
                    .GetIndividuals(filterString)
                    .ConvertToIndividualViewModelList();
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
                List<Individual> returnList = new List<Individual>();
                returnList = unitOfWork.GetIndividualsRepository()
                    .GetIndividualsForPopup(filterString);

                response.Individuals = returnList.ConvertToIndividualViewModelList();
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
                response.Individual = unitOfWork.GetIndividualsRepository()
                    .GetIndividual(id)
                    .ConvertToIndividualViewModel();
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

        ///<summary>
        /// Gets new code for business partner creation
        ///</summary>
        ///<returns></returns>
        public IndividualCodeResponse GetNewCodeValue()
        {
            IndividualCodeResponse response = new IndividualCodeResponse();

            try
            {
                response.Code = unitOfWork.GetIndividualsRepository().GetNewCodeValue();
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
        public IndividualResponse Create(IndividualViewModel individual)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {

                Individual createdIndividual = unitOfWork.GetIndividualsRepository().Create(individual.ConvertToIndividual());


                unitOfWork.Save();
                response.Individual = createdIndividual.ConvertToIndividualViewModel();

                //Thread td = new Thread(() =>
                //{
                //    var resp = FirebaseHelper.Send<IndividualViewModel>("Individuals", response.Individual);
                //});
                //td.IsBackground = true;
                //td.Start();

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
        public IndividualResponse Update(IndividualViewModel individual)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                Individual createdIndividual = unitOfWork.GetIndividualsRepository().Update(individual.ConvertToIndividual());

                unitOfWork.Save();
                response.Individual = createdIndividual.ConvertToIndividualViewModel();

                //Thread td = new Thread(() =>
                //{
                //    var resp = FirebaseHelper.Send<IndividualViewModel>("Individuals", response.Individual);
                //});
                //td.IsBackground = true;
                //td.Start();

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
        /// Deactivate business partner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndividualResponse Delete(int id)
        {
            IndividualResponse response = new IndividualResponse();
            try
            {
                Individual deletedIndividual = unitOfWork.GetIndividualsRepository().Delete(id);
                unitOfWork.Save();

                response.Individual = deletedIndividual.ConvertToIndividualViewModel();

                //Thread td = new Thread(() =>
                //{
                //    var resp = FirebaseHelper.Send<IndividualViewModel>("Individuals", response.Individual);
                //});
                //td.IsBackground = true;
                //td.Start();

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

        public IndividualListResponse GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 6, string searchParameter = "")
        {
            IndividualListResponse response = new IndividualListResponse();
            try
            {
                response.IndividualsByPage = unitOfWork.GetIndividualsRepository()
                    .GetIndividualsByPage(currentPage, itemsPerPage, searchParameter)
                    .ConvertToIndividualViewModelList();
                response.TotalItems = unitOfWork.GetIndividualsRepository().GetIndividualsCount(searchParameter);
                response.Success = true;
            }
            catch (Exception ex)
            {
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
                response.TotalItems = unitOfWork.GetIndividualsRepository().GetIndividualsCount(searchParameter);
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

