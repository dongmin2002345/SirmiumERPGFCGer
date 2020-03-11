using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.ToDos
{
    public static class ToDoStatusMapper
    {
        public static List<ToDoStatusViewModel> ConvertToToDoStatusViewModelList(this IEnumerable<ToDoStatus> toDoStatuses)
        {
            List<ToDoStatusViewModel> toDoStatusViewModels = new List<ToDoStatusViewModel>();
            foreach (ToDoStatus toDoStatus in toDoStatuses)
            {
                toDoStatusViewModels.Add(toDoStatus.ConvertToToDoStatusViewModel());
            }
            return toDoStatusViewModels;
        }

        public static ToDoStatusViewModel ConvertToToDoStatusViewModel(this ToDoStatus toDoStatus)
        {
            ToDoStatusViewModel toDoStatusViewModel = new ToDoStatusViewModel()
            {
                Id = toDoStatus.Id,
                Identifier = toDoStatus.Identifier,
                Code = toDoStatus.Code,
                Name = toDoStatus.Name,

                IsActive = toDoStatus.Active,

                CreatedBy = toDoStatus.CreatedBy?.ConvertToUserViewModelLite(),
                Company = toDoStatus.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = toDoStatus.UpdatedAt,
                CreatedAt = toDoStatus.CreatedAt
            };



            return toDoStatusViewModel;
        }

        public static ToDoStatusViewModel ConvertToToDoStatusViewModelLite(this ToDoStatus toDoStatus)
        {
            ToDoStatusViewModel toDoStatusViewModel = new ToDoStatusViewModel()
            {
                Id = toDoStatus.Id,
                Identifier = toDoStatus.Identifier,
                Code = toDoStatus.Code,
                Name = toDoStatus.Name,

                IsActive = toDoStatus.Active,

                UpdatedAt = toDoStatus.UpdatedAt,
                CreatedAt = toDoStatus.CreatedAt
            };


            return toDoStatusViewModel;
        }

        public static ToDoStatus ConvertToToDoStatus(this ToDoStatusViewModel toDoStatusViewModel)
        {
            ToDoStatus toDoStatus = new ToDoStatus()
            {
                Id = toDoStatusViewModel.Id,
                Identifier = toDoStatusViewModel.Identifier,
                Code = toDoStatusViewModel.Code,
                Name = toDoStatusViewModel.Name,

                CreatedById = toDoStatusViewModel.CreatedBy?.Id ?? null,
                CompanyId = toDoStatusViewModel.Company?.Id ?? null,

                CreatedAt = toDoStatusViewModel.CreatedAt,
                UpdatedAt = toDoStatusViewModel.UpdatedAt
            };

            return toDoStatus;
        }
    }
}
