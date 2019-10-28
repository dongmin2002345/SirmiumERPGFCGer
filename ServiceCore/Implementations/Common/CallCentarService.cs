using DataMapper.Mappers.Common.CallCentars;
using DomainCore.Common.CallCentars;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common
{
    public class CallCentarService : ICallCentarService
    {
        private IUnitOfWork unitOfWork;

        public CallCentarService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public CallCentarListResponse GetCallCentars(int companyId)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response.CallCentars = unitOfWork.GetCallCentarRepository().GetCallCentars(companyId)
                    .ConvertToCallCentarViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CallCentars = new List<CallCentarViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarResponse Create(CallCentarViewModel re)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                CallCentar addedCallCentar = unitOfWork.GetCallCentarRepository().Create(re.ConvertToCallCentar());

                unitOfWork.Save();

                response.CallCentar = addedCallCentar.ConvertToCallCentarViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CallCentar = new CallCentarViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarResponse Delete(Guid identifier)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                CallCentar deletedCallCentar = unitOfWork.GetCallCentarRepository().Delete(identifier);

                unitOfWork.Save();

                response.CallCentar = deletedCallCentar.ConvertToCallCentarViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CallCentar = new CallCentarViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CallCentarListResponse Sync(SyncCallCentarRequest request)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response.CallCentars = new List<CallCentarViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.CallCentars.AddRange(unitOfWork.GetCallCentarRepository()
                        .GetCallCentarsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToCallCentarViewModelList() ?? new List<CallCentarViewModel>());
                }
                else
                {
                    response.CallCentars.AddRange(unitOfWork.GetCallCentarRepository()
                        .GetCallCentars(request.CompanyId)
                        ?.ConvertToCallCentarViewModelList() ?? new List<CallCentarViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CallCentars = new List<CallCentarViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
