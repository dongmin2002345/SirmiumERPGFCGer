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
    public class PhysicalPersonNoteService : IPhysicalPersonNoteService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonNoteListResponse Sync(SyncPhysicalPersonNoteRequest request)
        {
            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteListResponse();
            try
            {
                response.PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonNotes.AddRange(unitOfWork.GetPhysicalPersonNoteRepository()
                        .GetPhysicalPersonNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonNoteViewModelList() ?? new List<PhysicalPersonNoteViewModel>());
                }
                else
                {
                    response.PhysicalPersonNotes.AddRange(unitOfWork.GetPhysicalPersonNoteRepository()
                        .GetPhysicalPersonNotes(request.CompanyId)
                        ?.ConvertToPhysicalPersonNoteViewModelList() ?? new List<PhysicalPersonNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
