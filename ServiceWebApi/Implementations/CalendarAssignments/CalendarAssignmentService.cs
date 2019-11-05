using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.CalendarAssignments
{
    public class CalendarAssignmentService : ICalendarAssignmentService
    {
        public CalendarAssignmentListResponse GetCalendarAssignments(int companyId)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<CalendarAssignmentViewModel, CalendarAssignmentListResponse>("GetCalendarAssignments",
                    new Dictionary<string, string>() { { "CompanyID", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CalendarAssignmentListResponse GetCalendarAssignmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<CalendarAssignmentViewModel>, CalendarAssignmentListResponse>("GetCalendarAssignmentsNewerThen",
                   new Dictionary<string, string>() {
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

        public CalendarAssignmentResponse Create(CalendarAssignmentViewModel CalendarAssignment)
        {
            CalendarAssignmentResponse response = new CalendarAssignmentResponse();
            try
            {
                response = WpfApiHandler.SendToApi<CalendarAssignmentViewModel, CalendarAssignmentResponse>(CalendarAssignment, "Create");
            }
            catch (Exception ex)
            {
                response.CalendarAssignment = new CalendarAssignmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public CalendarAssignmentResponse Delete(Guid identifier)
        {
            CalendarAssignmentResponse response = new CalendarAssignmentResponse();
            try
            {
                response = WpfApiHandler.SendToApi<Guid, CalendarAssignmentViewModel, CalendarAssignmentResponse>(identifier, "Delete");
            }
            catch (Exception ex)
            {
                response.CalendarAssignment = new CalendarAssignmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public CalendarAssignmentListResponse Sync(SyncCalendarAssignmentRequest request)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncCalendarAssignmentRequest, CalendarAssignmentViewModel, CalendarAssignmentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
