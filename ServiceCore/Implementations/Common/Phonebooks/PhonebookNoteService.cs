using DataMapper.Mappers.Common.Phonebooks;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Phonebooks
{
    public class PhonebookNoteService : IPhonebookNoteService
    {
        IUnitOfWork unitOfWork;

        public PhonebookNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhonebookNoteListResponse Sync(SyncPhonebookNoteRequest request)
        {
            PhonebookNoteListResponse response = new PhonebookNoteListResponse();
            try
            {
                response.PhonebookNotes = new List<PhonebookNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhonebookNotes.AddRange(unitOfWork.GetPhonebookNoteRepository()
                        .GetPhonebookNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhonebookNoteViewModelList() ?? new List<PhonebookNoteViewModel>());
                }
                else
                {
                    response.PhonebookNotes.AddRange(unitOfWork.GetPhonebookNoteRepository()
                        .GetPhonebookNotes(request.CompanyId)
                        ?.ConvertToPhonebookNoteViewModelList() ?? new List<PhonebookNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhonebookNotes = new List<PhonebookNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
