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
	public class InputInvoiceDocumentService : IInputInvoiceDocumentService
	{
		IUnitOfWork unitOfWork;

		public InputInvoiceDocumentService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public InputInvoiceDocumentListResponse Sync(SyncInputInvoiceDocumentRequest request)
		{
			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentListResponse();
			try
			{
				response.InputInvoiceDocuments = new List<InputInvoiceDocumentViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.InputInvoiceDocuments.AddRange(unitOfWork.GetInputInvoiceDocumentRepository()
						.GetInputInvoiceDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToInputInvoiceDocumentViewModelList() ?? new List<InputInvoiceDocumentViewModel>());
				}
				else
				{
					response.InputInvoiceDocuments.AddRange(unitOfWork.GetInputInvoiceDocumentRepository()
						.GetInputInvoiceDocuments(request.CompanyId)
						?.ConvertToInputInvoiceDocumentViewModelList() ?? new List<InputInvoiceDocumentViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoiceDocuments = new List<InputInvoiceDocumentViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
