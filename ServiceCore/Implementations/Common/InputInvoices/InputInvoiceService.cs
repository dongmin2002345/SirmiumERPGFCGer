using DataMapper.Mappers.Common.InputInvoices;
using DomainCore.Common.InputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.InputInvoices
{
    public class InputInvoiceService : IInputInvoiceService
	{
		IUnitOfWork unitOfWork;

		public InputInvoiceService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public InputInvoiceListResponse GetInputInvoices(int companyId)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response.InputInvoices = unitOfWork.GetInputInvoiceRepository().GetInputInvoices(companyId)
			   .ConvertToInputInvoiceViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse GetInputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.InputInvoices = unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoicesNewerThan(companyId, (DateTime)lastUpdateTime)
						.ConvertToInputInvoiceViewModelList();
				}
				else
				{
					response.InputInvoices = unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoices(companyId)
						.ConvertToInputInvoiceViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Create(InputInvoiceViewModel re)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
            {
                // Backup notes
                List<InputInvoiceNoteViewModel> inputInvoiceNotes = re.InputInvoiceNotes?.ToList();
                re.InputInvoiceNotes = null;

                // Backup documents
                List<InputInvoiceDocumentViewModel> inputInvoiceDocuments = re.InputInvoiceDocuments?.ToList();
                re.InputInvoiceDocuments = null;

                InputInvoice createdInputInvoice = unitOfWork.GetInputInvoiceRepository().Create(re.ConvertToInputInvoice());

                // Update notes
                if (inputInvoiceNotes != null && inputInvoiceNotes.Count > 0)
                {
                    // Items for create or update
                    foreach (var inputInvoiceNote in inputInvoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<InputInvoiceNoteViewModel>())
                    {
                        inputInvoiceNote.InputInvoice = new InputInvoiceViewModel() { Id = createdInputInvoice.Id };
                        inputInvoiceNote.ItemStatus = ItemStatus.Submited;
                        InputInvoiceNote createdInputInvoiceNote = unitOfWork.GetInputInvoiceNoteRepository()
                            .Create(inputInvoiceNote.ConvertToInputInvoiceNote());
                    }

                    foreach (var item in inputInvoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<InputInvoiceNoteViewModel>())
                    {
                        item.InputInvoice = new InputInvoiceViewModel() { Id = createdInputInvoice.Id };
                        unitOfWork.GetInputInvoiceNoteRepository().Create(item.ConvertToInputInvoiceNote());

                        unitOfWork.GetInputInvoiceNoteRepository().Delete(item.Identifier);
                    }
                }

                // Update documents
                if (inputInvoiceDocuments != null && inputInvoiceDocuments.Count > 0)
                {
                    // Items for create or update
                    foreach (var inputInvoiceDocument in inputInvoiceDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<InputInvoiceDocumentViewModel>())
                    {
                        inputInvoiceDocument.InputInvoice = new InputInvoiceViewModel() { Id = createdInputInvoice.Id };
                        inputInvoiceDocument.ItemStatus = ItemStatus.Submited;
                        InputInvoiceDocument createdInputInvoiceDocument = unitOfWork.GetInputInvoiceDocumentRepository()
                            .Create(inputInvoiceDocument.ConvertToInputInvoiceDocument());
                    }

                    foreach (var item in inputInvoiceDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<InputInvoiceDocumentViewModel>())
                    {
                        item.InputInvoice = new InputInvoiceViewModel() { Id = createdInputInvoice.Id };
                        unitOfWork.GetInputInvoiceDocumentRepository().Create(item.ConvertToInputInvoiceDocument());

                        unitOfWork.GetInputInvoiceDocumentRepository().Delete(item.Identifier);
                    }
                }

                unitOfWork.Save();

				response.InputInvoice = createdInputInvoice.ConvertToInputInvoiceViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Delete(Guid identifier)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				DomainCore.Common.InputInvoices.InputInvoice deletedInputInvoice = unitOfWork.GetInputInvoiceRepository().Delete(identifier);

				unitOfWork.Save();

				response.InputInvoice = deletedInputInvoice.ConvertToInputInvoiceViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse Sync(SyncInputInvoiceRequest request)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.InputInvoices.AddRange(unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoicesNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToInputInvoiceViewModelList() ?? new List<InputInvoiceViewModel>());
				}
				else
				{
					response.InputInvoices.AddRange(unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoices(request.CompanyId)
						?.ConvertToInputInvoiceViewModelList() ?? new List<InputInvoiceViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
