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
    public class PhysicalPersonAttachmentService : IPhysicalPersonAttachmentService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonAttachmentService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public PhysicalPersonAttachmentResponse Create(PhysicalPersonAttachmentViewModel PhysicalPersonAttachment)
        {
            PhysicalPersonAttachmentResponse response = new PhysicalPersonAttachmentResponse();
            try
            {
                response.PhysicalPersonAttachment = unitOfWork.GetPhysicalPersonAttachmentRepository().Create(PhysicalPersonAttachment.ConvertToPhysicalPersonAttachment())
                    .ConvertToPhysicalPersonAttachmentViewModel();

                unitOfWork.Save();

                response.Success = true;
            } catch(Exception ex)
            {
                response = new PhysicalPersonAttachmentResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonAttachmentListResponse CreateList(List<PhysicalPersonAttachmentViewModel> PhysicalPersonAttachment)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response.PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();
                foreach (var item in PhysicalPersonAttachment)
                {
                    var attachment = unitOfWork.GetPhysicalPersonAttachmentRepository().Create(item.ConvertToPhysicalPersonAttachment())
                        .ConvertToPhysicalPersonAttachmentViewModel();

                    response.PhysicalPersonAttachments.Add(attachment);
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new PhysicalPersonAttachmentListResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonAttachmentResponse Delete(Guid identifier)
        {
            PhysicalPersonAttachmentResponse response = new PhysicalPersonAttachmentResponse();
            try
            {
                response.PhysicalPersonAttachment = unitOfWork.GetPhysicalPersonAttachmentRepository().Delete(identifier)
                    .ConvertToPhysicalPersonAttachmentViewModel();

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new PhysicalPersonAttachmentResponse();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachments(int companyId)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response.PhysicalPersonAttachments = unitOfWork.GetPhysicalPersonAttachmentRepository()
                    .GetPhysicalPersonAttachments(companyId)
                    .ConvertToPhysicalPersonAttachmentViewModelList();

                response.Success = true;
            } catch(Exception ex)
            {
                response = new PhysicalPersonAttachmentListResponse();
                response.PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response.PhysicalPersonAttachments = unitOfWork.GetPhysicalPersonAttachmentRepository()
                    .GetPhysicalPersonAttachmentsNewerThen(companyId, (DateTime)lastUpdateTime)
                    .ConvertToPhysicalPersonAttachmentViewModelList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new PhysicalPersonAttachmentListResponse();
                response.PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonAttachmentListResponse Sync(SyncPhysicalPersonAttachmentRequest request)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                if(request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonAttachments = unitOfWork.GetPhysicalPersonAttachmentRepository()
                        .GetPhysicalPersonAttachmentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        .ConvertToPhysicalPersonAttachmentViewModelList();
                } else
                {
                    response.PhysicalPersonAttachments = unitOfWork.GetPhysicalPersonAttachmentRepository()
                        .GetPhysicalPersonAttachments(request.CompanyId)
                        .ConvertToPhysicalPersonAttachmentViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response = new PhysicalPersonAttachmentListResponse();
                response.PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
