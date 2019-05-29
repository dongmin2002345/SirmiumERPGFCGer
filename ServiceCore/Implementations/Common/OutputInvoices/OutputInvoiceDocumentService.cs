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
	public class OutputInvoiceDocumentService : IOutputInvoiceDocumentService
	{
		IUnitOfWork unitOfWork;

		public OutputInvoiceDocumentService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public OutputInvoiceDocumentListResponse Sync(SyncOutputInvoiceDocumentRequest request)
		{
			OutputInvoiceDocumentListResponse response = new OutputInvoiceDocumentListResponse();
			try
			{
				response.OutputInvoiceDocuments = new List<OutputInvoiceDocumentViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.OutputInvoiceDocuments.AddRange(unitOfWork.GetOutputInvoiceDocumentRepository()
						.GetOutputInvoiceDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToOutputInvoiceDocumentViewModelList() ?? new List<OutputInvoiceDocumentViewModel>());
				}
				else
				{
					response.OutputInvoiceDocuments.AddRange(unitOfWork.GetOutputInvoiceDocumentRepository()
						.GetOutputInvoiceDocuments(request.CompanyId)
						?.ConvertToOutputInvoiceDocumentViewModelList() ?? new List<OutputInvoiceDocumentViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.OutputInvoiceDocuments = new List<OutputInvoiceDocumentViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
