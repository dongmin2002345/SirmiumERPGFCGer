using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.ToDos
{
    public static class ToDoMapper
    {
        public static List<ToDoViewModel> ConvertToToDoViewModelList(this IEnumerable<ToDo> toDos)
        {
            List<ToDoViewModel> toDoViewModels = new List<ToDoViewModel>();
            foreach (ToDo toDo in toDos)
            {
                toDoViewModels.Add(toDo.ConvertToToDoViewModel());
            }
            return toDoViewModels;
        }

        public static ToDoViewModel ConvertToToDoViewModel(this ToDo toDo)
        {
            ToDoViewModel toDoViewModel = new ToDoViewModel()
            {
                Id = toDo.Id,
                Identifier = toDo.Identifier,

                Name = toDo.Name,
                Description = toDo.Description,
                ToDoDate = toDo.ToDoDate,

                IsActive = toDo.Active,

                CreatedBy = toDo.CreatedBy?.ConvertToUserViewModelLite(),
                Company = toDo.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = toDo.UpdatedAt,
                CreatedAt = toDo.CreatedAt
            };



            return toDoViewModel;
        }

        public static List<ToDoViewModel> ConvertToToDoViewModelListLite(this IEnumerable<ToDo> toDos)
        {
            List<ToDoViewModel> toDoViewModels = new List<ToDoViewModel>();
            foreach (ToDo toDo in toDos)
            {
                toDoViewModels.Add(toDo.ConvertToToDoViewModelLite());
            }
            return toDoViewModels;
        }

        public static ToDoViewModel ConvertToToDoViewModelLite(this ToDo toDo)
        {
            ToDoViewModel toDoViewModel = new ToDoViewModel()
            {
                Id = toDo.Id,
                Identifier = toDo.Identifier,

                Name = toDo.Name,
                Description = toDo.Description,
                ToDoDate = toDo.ToDoDate,

                IsActive = toDo.Active,

                UpdatedAt = toDo.UpdatedAt,
                CreatedAt = toDo.CreatedAt
            };


            return toDoViewModel;
        }

        public static ToDo ConvertToToDo(this ToDoViewModel toDoViewModel)
        {
            ToDo toDo = new ToDo()
            {
                Id = toDoViewModel.Id,
                Identifier = toDoViewModel.Identifier,

                Name = toDoViewModel.Name,
                Description = toDoViewModel.Description,
                ToDoDate = toDoViewModel.ToDoDate,

                CreatedById = toDoViewModel.CreatedBy?.Id ?? null,
                CompanyId = toDoViewModel.Company?.Id ?? null,

                CreatedAt = toDoViewModel.CreatedAt,
                UpdatedAt = toDoViewModel.UpdatedAt
            };

            return toDo;
        }
    }
}
