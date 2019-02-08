using DataMapper.Mappers.Common.OutputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.OutputInvoices
{
    public class OutputInvoiceNoteService : IOutputInvoiceNoteService
    {
        IUnitOfWork unitOfWork;

        public OutputInvoiceNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OutputInvoiceNoteListResponse Sync(SyncOutputInvoiceNoteRequest request)
        {
            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteListResponse();
            try
            {
                response.OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.OutputInvoiceNotes.AddRange(unitOfWork.GetOutputInvoiceNoteRepository()
                        .GetOutputInvoiceNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToOutputInvoiceNoteViewModelList() ?? new List<OutputInvoiceNoteViewModel>());
                }
                else
                {
                    response.OutputInvoiceNotes.AddRange(unitOfWork.GetOutputInvoiceNoteRepository()
                        .GetOutputInvoiceNotes(request.CompanyId)
                        ?.ConvertToOutputInvoiceNoteViewModelList() ?? new List<OutputInvoiceNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
