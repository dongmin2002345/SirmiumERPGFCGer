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
    public class PhysicalPersonAttachmentService : IPhysicalPersonAttachmentService
    {
        public PhysicalPersonAttachmentResponse Create(PhysicalPersonAttachmentViewModel PhysicalPersonAttachment)
        {
            PhysicalPersonAttachmentResponse response = new PhysicalPersonAttachmentResponse();
            try
            {
                response = WpfApiHandler.SendToApi<PhysicalPersonAttachmentViewModel, PhysicalPersonAttachmentResponse>(PhysicalPersonAttachment, "Create");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.SendToApi<List<PhysicalPersonAttachmentViewModel>, PhysicalPersonAttachmentListResponse>(PhysicalPersonAttachment, "CreateList");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.SendToApi<Guid, PhysicalPersonAttachmentViewModel, PhysicalPersonAttachmentResponse>(identifier, "Delete");
            }
            catch (Exception ex)
            {
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
                response = WpfApiHandler.GetFromApi<PhysicalPersonAttachmentViewModel, PhysicalPersonAttachmentListResponse>("GetPhysicalPersonAttachments", new Dictionary<string, string>() {
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

        public PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<PhysicalPersonAttachmentViewModel, PhysicalPersonAttachmentListResponse>("GetPhysicalPersonAttachmentsNewerThen", new Dictionary<string, string>() {
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

        public PhysicalPersonAttachmentListResponse Sync(SyncPhysicalPersonAttachmentRequest request)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhysicalPersonAttachmentRequest, PhysicalPersonAttachmentViewModel, PhysicalPersonAttachmentListResponse>(request, "Sync");
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
