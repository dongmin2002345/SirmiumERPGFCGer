using DataMapper.Mappers.ConstructionSites;
using DomainCore.ConstructionSites;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.ConstructionSites
{
    public class ConstructionSiteCalculationService : IConstructionSiteCalculationService
    {
        IUnitOfWork unitOfWork;

        public ConstructionSiteCalculationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ConstructionSiteCalculationListResponse GetConstructionSiteCalculations(int companyId)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response.ConstructionSiteCalculations = unitOfWork.GetConstructionSiteCalculationRepository()
                    .GetConstructionSiteCalculations(companyId)
                    .ConvertToConstructionSiteCalculationViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ConstructionSiteCalculationListResponse GetConstructionSiteCalculationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.ConstructionSiteCalculations = unitOfWork.GetConstructionSiteCalculationRepository()
                        .GetConstructionSiteCalculationsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToConstructionSiteCalculationViewModelList();
                }
                else
                {
                    response.ConstructionSiteCalculations = unitOfWork.GetConstructionSiteCalculationRepository()
                        .GetConstructionSiteCalculations(companyId)
                        .ConvertToConstructionSiteCalculationViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ConstructionSiteCalculationResponse Create(ConstructionSiteCalculationViewModel constructionSiteCalculationViewModel)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();
            try
            {
                ConstructionSiteCalculation lastConstructionSiteCalculation = unitOfWork.GetConstructionSiteCalculationRepository()
                    .GetLastConstructionSiteCalculation(constructionSiteCalculationViewModel.Company.Id, constructionSiteCalculationViewModel.ConstructionSite.Id);

                if (constructionSiteCalculationViewModel.PlusMinus == "+")
                {
                    decimal enteredValue = constructionSiteCalculationViewModel.NumOfEmployees * constructionSiteCalculationViewModel.NumOfMonths * constructionSiteCalculationViewModel.EmployeePrice;

                    constructionSiteCalculationViewModel.ValueDifference = (lastConstructionSiteCalculation?.ValueDifference ?? 0) - enteredValue;
                    if (constructionSiteCalculationViewModel.ValueDifference < 0)
                        constructionSiteCalculationViewModel.ValueDifference = 0;
                    decimal valueToAdd = enteredValue - (lastConstructionSiteCalculation?.ValueDifference ?? 0);
                    if (valueToAdd < 0)
                        valueToAdd = 0;
                    constructionSiteCalculationViewModel.NewValue = (lastConstructionSiteCalculation?.NewValue ?? 0) + valueToAdd;
                }
                else
                {
                    decimal enteredValue = constructionSiteCalculationViewModel.NumOfEmployees * constructionSiteCalculationViewModel.NumOfMonths * constructionSiteCalculationViewModel.EmployeePrice;

                    constructionSiteCalculationViewModel.ValueDifference = (lastConstructionSiteCalculation?.ValueDifference ?? 0) + enteredValue;
                    constructionSiteCalculationViewModel.NewValue = lastConstructionSiteCalculation?.NewValue ?? 0;
                }

                constructionSiteCalculationViewModel.UpdatedAt = DateTime.Now.AddMilliseconds(1);

                ConstructionSiteCalculation addedConstructionSiteCalculation = unitOfWork.GetConstructionSiteCalculationRepository()
                    .Create(constructionSiteCalculationViewModel.ConvertToConstructionSiteCalculation());
                unitOfWork.Save();
                response.ConstructionSiteCalculation = addedConstructionSiteCalculation.ConvertToConstructionSiteCalculationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculation = new ConstructionSiteCalculationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ConstructionSiteCalculationListResponse Sync(SyncConstructionSiteCalculationRequest request)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ConstructionSiteCalculations.AddRange(unitOfWork.GetConstructionSiteCalculationRepository()
                        .GetConstructionSiteCalculationsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToConstructionSiteCalculationViewModelList() ?? new List<ConstructionSiteCalculationViewModel>());
                }
                else
                {
                    response.ConstructionSiteCalculations.AddRange(unitOfWork.GetConstructionSiteCalculationRepository()
                        .GetConstructionSiteCalculations(request.CompanyId)
                        ?.ConvertToConstructionSiteCalculationViewModelList() ?? new List<ConstructionSiteCalculationViewModel>());
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
