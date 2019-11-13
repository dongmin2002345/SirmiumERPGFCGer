using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Employees
{
    public class EmployeeAttachmentService : IEmployeeAttachmentService
    {
        public EmployeeAttachmentResponse Create(EmployeeAttachmentViewModel EmployeeAttachment)
        {
            EmployeeAttachmentResponse response = new EmployeeAttachmentResponse();
            try
            {
                response = WpfApiHandler.SendToApi<EmployeeAttachmentViewModel, EmployeeAttachmentResponse>(EmployeeAttachment, "Create");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.SendToApi<List<EmployeeAttachmentViewModel>, EmployeeAttachmentListResponse>(EmployeeAttachment, "CreateList");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.SendToApi<Guid, EmployeeAttachmentViewModel, EmployeeAttachmentResponse>(identifier, "Delete");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.GetFromApi<EmployeeAttachmentViewModel, EmployeeAttachmentListResponse>("GetEmployeeAttachments", new Dictionary<string, string>() {
                       { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.GetFromApi<EmployeeAttachmentViewModel, EmployeeAttachmentListResponse>("GetEmployeeAttachmentsNewerThen", new Dictionary<string, string>() {
                       { "CompanyId", companyId.ToString() },
                       { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.SendToApi<SyncEmployeeAttachmentRequest, EmployeeAttachmentViewModel, EmployeeAttachmentListResponse>(request, "Sync");
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
