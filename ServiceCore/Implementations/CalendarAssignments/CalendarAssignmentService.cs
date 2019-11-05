using DataMapper.Mappers.CalendarAssignments;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.CalendarAssignments
{
    public class CalendarAssignmentService : ICalendarAssignmentService
    {
        private IUnitOfWork unitOfWork;

        public CalendarAssignmentService(IUnitOfWork uow)
        {
            this.unitOfWork = uow;
        }
        public CalendarAssignmentResponse Create(CalendarAssignmentViewModel assignment)
        {
            throw new NotImplementedException();
        }

        public CalendarAssignmentResponse Delete(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public CalendarAssignmentListResponse GetCalendarAssignments(int companyId)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            try
            {
                response.CalendarAssignments = unitOfWork.GetCalendarAssignmentRepository()
                    .GetCalendarAssignments(companyId)
                    .ConvertToCalendarAssignmentViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
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
                if (lastUpdateTime != null)
                {
                    response.CalendarAssignments = unitOfWork.GetCalendarAssignmentRepository()
                        .GetCalendarAssignmentsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToCalendarAssignmentViewModelList();
                }
                else
                {
                    response.CalendarAssignments = unitOfWork.GetCalendarAssignmentRepository()
                        .GetCalendarAssignments(companyId)
                        .ConvertToCalendarAssignmentViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
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
                response.CalendarAssignments = new List<CalendarAssignmentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.CalendarAssignments.AddRange(unitOfWork.GetCalendarAssignmentRepository()
                        .GetCalendarAssignmentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToCalendarAssignmentViewModelList() ?? new List<CalendarAssignmentViewModel>());
                }
                else
                {
                    response.CalendarAssignments = unitOfWork.GetCalendarAssignmentRepository()
                        .GetCalendarAssignments(request.CompanyId)
                        .ConvertToCalendarAssignmentViewModelList();
                }

                response.Success = true;
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
