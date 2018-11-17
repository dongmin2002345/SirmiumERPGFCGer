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
    public class EmployeeNoteService : IEmployeeNoteService
    {
        IUnitOfWork unitOfWork;

        public EmployeeNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeNoteListResponse Sync(SyncEmployeeNoteRequest request)
        {
            EmployeeNoteListResponse response = new EmployeeNoteListResponse();
            try
            {
                response.EmployeeNotes = new List<EmployeeNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeNotes.AddRange(unitOfWork.GetEmployeeNoteRepository()
                        .GetEmployeeNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeNoteViewModelList() ?? new List<EmployeeNoteViewModel>());
                }
                else
                {
                    response.EmployeeNotes.AddRange(unitOfWork.GetEmployeeNoteRepository()
                        .GetEmployeeNotes(request.CompanyId)
                        ?.ConvertToEmployeeNoteViewModelList() ?? new List<EmployeeNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeNotes = new List<EmployeeNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
