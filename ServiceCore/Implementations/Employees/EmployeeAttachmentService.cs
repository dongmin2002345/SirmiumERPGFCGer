using DataMapper.Mappers.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class EmployeeAttachmentService : IEmployeeAttachmentService
    {
        IUnitOfWork unitOfWork;

        public EmployeeAttachmentService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public EmployeeAttachmentResponse Create(EmployeeAttachmentViewModel EmployeeAttachment)
        {
            EmployeeAttachmentResponse response = new EmployeeAttachmentResponse();
            try
            {
                response.EmployeeAttachment = unitOfWork.GetEmployeeAttachmentRepository().Create(EmployeeAttachment.ConvertToEmployeeAttachment())
                    .ConvertToEmployeeAttachmentViewModel();

                unitOfWork.Save();

                response.Success = true;
            } catch(Exception ex)
            {
                response = new EmployeeAttachmentResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeAttachmentListResponse CreateList(List<EmployeeAttachmentViewModel> EmployeeAttachment)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            try
            {
                response.EmployeeAttachments = new List<EmployeeAttachmentViewModel>();
                foreach (var item in EmployeeAttachment)
                {
                    var attachment = unitOfWork.GetEmployeeAttachmentRepository().Create(item.ConvertToEmployeeAttachment())
                        .ConvertToEmployeeAttachmentViewModel();

                    response.EmployeeAttachments.Add(attachment);
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new EmployeeAttachmentListResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeAttachmentResponse Delete(Guid identifier)
        {
            EmployeeAttachmentResponse response = new EmployeeAttachmentResponse();
            try
            {
                response.EmployeeAttachment = unitOfWork.GetEmployeeAttachmentRepository().Delete(identifier)
                    .ConvertToEmployeeAttachmentViewModel();

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new EmployeeAttachmentResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeAttachmentListResponse GetEmployeeAttachments(int companyId)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            try
            {
                response.EmployeeAttachments = unitOfWork.GetEmployeeAttachmentRepository()
                    .GetEmployeeAttachments(companyId)
                    .ConvertToEmployeeAttachmentViewModelList();

                response.Success = true;
            } catch(Exception ex)
            {
                response = new EmployeeAttachmentListResponse();
                response.EmployeeAttachments = new List<EmployeeAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeAttachmentListResponse GetEmployeeAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            try
            {
                response.EmployeeAttachments = unitOfWork.GetEmployeeAttachmentRepository()
                    .GetEmployeeAttachmentsNewerThen(companyId, (DateTime)lastUpdateTime)
                    .ConvertToEmployeeAttachmentViewModelList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new EmployeeAttachmentListResponse();
                response.EmployeeAttachments = new List<EmployeeAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeAttachmentListResponse Sync(SyncEmployeeAttachmentRequest request)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            try
            {
                if(request.LastUpdatedAt != null)
                {
                    response.EmployeeAttachments = unitOfWork.GetEmployeeAttachmentRepository()
                        .GetEmployeeAttachmentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        .ConvertToEmployeeAttachmentViewModelList();
                } else
                {
                    response.EmployeeAttachments = unitOfWork.GetEmployeeAttachmentRepository()
                        .GetEmployeeAttachments(request.CompanyId)
                        .ConvertToEmployeeAttachmentViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new EmployeeAttachmentListResponse();
                response.EmployeeAttachments = new List<EmployeeAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
