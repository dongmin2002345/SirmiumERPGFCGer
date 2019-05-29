using DataMapper.Mappers.Common.InputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.InputInvoices
{
    public class InputInvoiceNoteService : IInputInvoiceNoteService
    {
        IUnitOfWork unitOfWork;

        public InputInvoiceNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public InputInvoiceNoteListResponse Sync(SyncInputInvoiceNoteRequest request)
        {
            InputInvoiceNoteListResponse response = new InputInvoiceNoteListResponse();
            try
            {
                response.InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.InputInvoiceNotes.AddRange(unitOfWork.GetInputInvoiceNoteRepository()
                        .GetInputInvoiceNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToInputInvoiceNoteViewModelList() ?? new List<InputInvoiceNoteViewModel>());
                }
                else
                {
                    response.InputInvoiceNotes.AddRange(unitOfWork.GetInputInvoiceNoteRepository()
                        .GetInputInvoiceNotes(request.CompanyId)
                        ?.ConvertToInputInvoiceNoteViewModelList() ?? new List<InputInvoiceNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
