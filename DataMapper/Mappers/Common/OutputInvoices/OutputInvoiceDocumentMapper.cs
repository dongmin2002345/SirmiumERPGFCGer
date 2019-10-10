using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.OutputInvoices
{
	public static class OutputInvoiceDocumentMapper
	{
		public static List<OutputInvoiceDocumentViewModel> ConvertToOutputInvoiceDocumentViewModelList(this IEnumerable<OutputInvoiceDocument> outputInvoiceDocuments)
		{
			List<OutputInvoiceDocumentViewModel> OutputInvoiceDocumentViewModels = new List<OutputInvoiceDocumentViewModel>();
			foreach (OutputInvoiceDocument OutputInvoiceDocument in outputInvoiceDocuments)
			{
				OutputInvoiceDocumentViewModels.Add(OutputInvoiceDocument.ConvertToOutputInvoiceDocumentViewModel());
			}
			return OutputInvoiceDocumentViewModels;
		}

		public static OutputInvoiceDocumentViewModel ConvertToOutputInvoiceDocumentViewModel(this OutputInvoiceDocument outputInvoiceDocument)
		{
			OutputInvoiceDocumentViewModel OutputInvoiceDocumentViewModel = new OutputInvoiceDocumentViewModel()
			{
				Id = outputInvoiceDocument.Id,
				Identifier = outputInvoiceDocument.Identifier,

				OutputInvoice = outputInvoiceDocument.OutputInvoice?.ConvertToOutputInvoiceViewModelLite(),

				Name = outputInvoiceDocument.Name,
				CreateDate = outputInvoiceDocument.CreateDate,
				Path = outputInvoiceDocument.Path,
                ItemStatus = outputInvoiceDocument.ItemStatus,

                IsActive = outputInvoiceDocument.Active,

				CreatedBy = outputInvoiceDocument.CreatedBy?.ConvertToUserViewModelLite(),
				Company = outputInvoiceDocument.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = outputInvoiceDocument.UpdatedAt,
				CreatedAt = outputInvoiceDocument.CreatedAt
			};

			return OutputInvoiceDocumentViewModel;
		}

		public static OutputInvoiceDocumentViewModel ConvertToOutputInvoiceDocumentViewModelLite(this OutputInvoiceDocument outputInvoiceDocument)
		{
			OutputInvoiceDocumentViewModel OutputInvoiceDocumentViewModel = new OutputInvoiceDocumentViewModel()
			{
				Id = outputInvoiceDocument.Id,
				Identifier = outputInvoiceDocument.Identifier,

				Name = outputInvoiceDocument.Name,
				CreateDate = outputInvoiceDocument.CreateDate,
				Path = outputInvoiceDocument.Path,
                ItemStatus = outputInvoiceDocument.ItemStatus,

                IsActive = outputInvoiceDocument.Active,

				UpdatedAt = outputInvoiceDocument.UpdatedAt,
				CreatedAt = outputInvoiceDocument.CreatedAt
			};

			return OutputInvoiceDocumentViewModel;
		}

		public static OutputInvoiceDocument ConvertToOutputInvoiceDocument(this OutputInvoiceDocumentViewModel outputInvoiceDocumentViewModel)
		{
			OutputInvoiceDocument OutputInvoiceDocument = new OutputInvoiceDocument()
			{
				Id = outputInvoiceDocumentViewModel.Id,
				Identifier = outputInvoiceDocumentViewModel.Identifier,

				OutputInvoiceId = outputInvoiceDocumentViewModel.OutputInvoice?.Id ?? null,

				Name = outputInvoiceDocumentViewModel.Name,
				CreateDate = outputInvoiceDocumentViewModel.CreateDate,
				Path = outputInvoiceDocumentViewModel.Path,
                ItemStatus = outputInvoiceDocumentViewModel.ItemStatus,

                Active = outputInvoiceDocumentViewModel.IsActive,

				CreatedById = outputInvoiceDocumentViewModel.CreatedBy?.Id ?? null,
				CompanyId = outputInvoiceDocumentViewModel.Company?.Id ?? null,

				CreatedAt = outputInvoiceDocumentViewModel.CreatedAt,
				UpdatedAt = outputInvoiceDocumentViewModel.UpdatedAt
			};

			return OutputInvoiceDocument;
		}
	}
}
