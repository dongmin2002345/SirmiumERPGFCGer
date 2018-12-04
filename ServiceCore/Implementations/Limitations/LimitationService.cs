using DataMapper.Mappers.Limitations;
using DomainCore.Limitations;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Limitations
{
    public class LimitationService : ILimitationService
    {
        private IUnitOfWork unitOfWork;

        public LimitationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public LimitationListResponse GetLimitations(int companyId)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response.Limitations = unitOfWork.GetLimitationRepository().GetLimitations(companyId)
                    .ConvertToLimitationViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationListResponse GetLimitationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Limitations = unitOfWork.GetLimitationRepository()
                        .GetLimitationsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToLimitationViewModelList();
                }
                else
                {
                    response.Limitations = unitOfWork.GetLimitationRepository()
                        .GetLimitations(companyId)
                        .ConvertToLimitationViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public LimitationResponse Create(LimitationViewModel limitation)
        {
            LimitationResponse response = new LimitationResponse();
            try
            {
                Limitation addedLimitation = unitOfWork.GetLimitationRepository().Create(limitation.ConvertToLimitation());
                unitOfWork.Save();

                response.Limitation = addedLimitation.ConvertToLimitationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Limitation = new LimitationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public LimitationListResponse Sync(SyncLimitationRequest request)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response.Limitations = new List<LimitationViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Limitations.AddRange(unitOfWork.GetLimitationRepository()
                        .GetLimitationsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToLimitationViewModelList() ?? new List<LimitationViewModel>());
                }
                else
                {
                    response.Limitations.AddRange(unitOfWork.GetLimitationRepository()
                        .GetLimitations(request.CompanyId)
                        ?.ConvertToLimitationViewModelList() ?? new List<LimitationViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Limitations = new List<LimitationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
