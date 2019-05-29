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
    public class LimitationEmailService : ILimitationEmailService
    {
        IUnitOfWork unitOfWork;

        public LimitationEmailService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public LimitationEmailListResponse GetLimitationEmails(int companyId)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response.LimitationEmails = unitOfWork.GetLimitationEmailRepository().GetLimitationEmails(companyId)
               .ConvertToLimitationEmailViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailListResponse GetLimitationEmailsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.LimitationEmails = unitOfWork.GetLimitationEmailRepository()
                        .GetLimitationEmailsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToLimitationEmailViewModelList();
                }
                else
                {
                    response.LimitationEmails = unitOfWork.GetLimitationEmailRepository()
                        .GetLimitationEmails(companyId)
                        .ConvertToLimitationEmailViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailResponse Create(LimitationEmailViewModel re)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                LimitationEmail addedLimitationEmail = unitOfWork.GetLimitationEmailRepository().Create(re.ConvertToLimitationEmail());
                unitOfWork.Save();
                response.LimitationEmail = addedLimitationEmail.ConvertToLimitationEmailViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.LimitationEmail = new LimitationEmailViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailResponse Delete(Guid identifier)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                LimitationEmail deletedLimitationEmail = unitOfWork.GetLimitationEmailRepository().Delete(identifier);

                unitOfWork.Save();

                response.LimitationEmail = deletedLimitationEmail?.ConvertToLimitationEmailViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.LimitationEmail = new LimitationEmailViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public LimitationEmailListResponse Sync(SyncLimitationEmailRequest request)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.LimitationEmails.AddRange(unitOfWork.GetLimitationEmailRepository()
                        .GetLimitationEmailsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToLimitationEmailViewModelList() ?? new List<LimitationEmailViewModel>());
                }
                else
                {
                    response.LimitationEmails.AddRange(unitOfWork.GetLimitationEmailRepository()
                        .GetLimitationEmails(request.CompanyId)
                        ?.ConvertToLimitationEmailViewModelList() ?? new List<LimitationEmailViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.LimitationEmails = new List<LimitationEmailViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
