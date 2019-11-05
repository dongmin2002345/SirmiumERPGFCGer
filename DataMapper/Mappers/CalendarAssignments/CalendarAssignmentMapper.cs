using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.CalendarAssignments
{
    public static class CalendarAssignmentMapper
    {
        public static List<CalendarAssignmentViewModel> ConvertToCalendarAssignmentViewModelList(this IEnumerable<CalendarAssignment> CalendarAssignments)
        {
            List<CalendarAssignmentViewModel> CalendarAssignmentViewModels = new List<CalendarAssignmentViewModel>();
            foreach (CalendarAssignment CalendarAssignment in CalendarAssignments)
            {
                CalendarAssignmentViewModels.Add(CalendarAssignment.ConvertToCalendarAssignmentViewModel());
            }
            return CalendarAssignmentViewModels;
        }

        public static CalendarAssignmentViewModel ConvertToCalendarAssignmentViewModel(this CalendarAssignment CalendarAssignment)
        {
            CalendarAssignmentViewModel CalendarAssignmentViewModel = new CalendarAssignmentViewModel()
            {
                Id = CalendarAssignment.Id,
                Identifier = CalendarAssignment.Identifier,

                Name = CalendarAssignment.Name,
                Description = CalendarAssignment.Description,
                Date = CalendarAssignment.Date,

                IsActive = CalendarAssignment.Active,
                
                CreatedBy = CalendarAssignment?.CreatedBy?.ConvertToUserViewModelLite(),
                AssignedTo = CalendarAssignment?.AssignedTo?.ConvertToUserViewModelLite(),
                
                Company = CalendarAssignment?.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = CalendarAssignment.UpdatedAt,
                CreatedAt = CalendarAssignment.CreatedAt
            };

            return CalendarAssignmentViewModel;
        }

        public static CalendarAssignmentViewModel ConvertToCalendarAssignmentViewModelLite(this CalendarAssignment CalendarAssignment)
        {
            CalendarAssignmentViewModel CalendarAssignmentViewModel = new CalendarAssignmentViewModel()
            {
                Id = CalendarAssignment.Id,
                Identifier = CalendarAssignment.Identifier,

                Name = CalendarAssignment.Name,
                Description = CalendarAssignment.Description,
                Date = CalendarAssignment.Date,

                IsActive = CalendarAssignment.Active,

                UpdatedAt = CalendarAssignment.UpdatedAt,
                CreatedAt = CalendarAssignment.CreatedAt
            };

            return CalendarAssignmentViewModel;
        }

        public static CalendarAssignment ConvertToCalendarAssignment(this CalendarAssignmentViewModel CalendarAssignmentViewModel)
        {
            CalendarAssignment CalendarAssignment = new CalendarAssignment()
            {
                Id = CalendarAssignmentViewModel.Id,
                Identifier = CalendarAssignmentViewModel.Identifier,
                Name = CalendarAssignmentViewModel.Name,
                Description = CalendarAssignmentViewModel.Description,
                Date = CalendarAssignmentViewModel.Date,

                AssignedToId = CalendarAssignmentViewModel?.AssignedTo?.Id ?? null,

                Active = CalendarAssignmentViewModel.IsActive,
                CreatedById = CalendarAssignmentViewModel.CreatedBy?.Id ?? null,
                CompanyId = CalendarAssignmentViewModel.Company?.Id ?? null,

                CreatedAt = CalendarAssignmentViewModel.CreatedAt,
                UpdatedAt = CalendarAssignmentViewModel.UpdatedAt
            };

            return CalendarAssignment;
        }
    }
}
