using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.InputInvoices
{
	public static class InputInvoiceDocumentMapper
	{
		public static List<InputInvoiceDocumentViewModel> ConvertToInputInvoiceDocumentViewModelList(this IEnumerable<InputInvoiceDocument> inputInvoiceDocuments)
		{
			List<InputInvoiceDocumentViewModel> InputInvoiceDocumentViewModels = new List<InputInvoiceDocumentViewModel>();
			foreach (InputInvoiceDocument InputInvoiceDocument in inputInvoiceDocuments)
			{
				InputInvoiceDocumentViewModels.Add(InputInvoiceDocument.ConvertToInputInvoiceDocumentViewModel());
			}
			return InputInvoiceDocumentViewModels;
		}

		public static InputInvoiceDocumentViewModel ConvertToInputInvoiceDocumentViewModel(this InputInvoiceDocument inputInvoiceDocument)
		{
			InputInvoiceDocumentViewModel InputInvoiceDocumentViewModel = new InputInvoiceDocumentViewModel()
			{
				Id = inputInvoiceDocument.Id,
				Identifier = inputInvoiceDocument.Identifier,

				InputInvoice = inputInvoiceDocument.InputInvoice?.ConvertToInputInvoiceViewModelLite(),

				Name = inputInvoiceDocument.Name,
				CreateDate = inputInvoiceDocument.CreateDate,
				Path = inputInvoiceDocument.Path,
                ItemStatus = inputInvoiceDocument.ItemStatus,
                IsActive = inputInvoiceDocument.Active,

				CreatedBy = inputInvoiceDocument.CreatedBy?.ConvertToUserViewModelLite(),
				Company = inputInvoiceDocument.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = inputInvoiceDocument.UpdatedAt,
				CreatedAt = inputInvoiceDocument.CreatedAt
			};

			return InputInvoiceDocumentViewModel;
		}

		public static InputInvoiceDocumentViewModel ConvertToInputInvoiceDocumentViewModelLite(this InputInvoiceDocument inputInvoiceDocument)
		{
			InputInvoiceDocumentViewModel InputInvoiceDocumentViewModel = new InputInvoiceDocumentViewModel()
			{
				Id = inputInvoiceDocument.Id,
				Identifier = inputInvoiceDocument.Identifier,

				Name = inputInvoiceDocument.Name,
				CreateDate = inputInvoiceDocument.CreateDate,
				Path = inputInvoiceDocument.Path,
                ItemStatus = inputInvoiceDocument.ItemStatus,
                IsActive = inputInvoiceDocument.Active,

				UpdatedAt = inputInvoiceDocument.UpdatedAt,
				CreatedAt = inputInvoiceDocument.CreatedAt
			};

			return InputInvoiceDocumentViewModel;
		}

		public static InputInvoiceDocument ConvertToInputInvoiceDocument(this InputInvoiceDocumentViewModel inputInvoiceDocumentViewModel)
		{
			InputInvoiceDocument InputInvoiceDocument = new InputInvoiceDocument()
			{
				Id = inputInvoiceDocumentViewModel.Id,
				Identifier = inputInvoiceDocumentViewModel.Identifier,

				InputInvoiceId = inputInvoiceDocumentViewModel.InputInvoice?.Id ?? null,

				Name = inputInvoiceDocumentViewModel.Name,
				CreateDate = inputInvoiceDocumentViewModel.CreateDate,
				Path = inputInvoiceDocumentViewModel.Path,
                ItemStatus = inputInvoiceDocumentViewModel.ItemStatus,
                Active = inputInvoiceDocumentViewModel.IsActive,

				CreatedById = inputInvoiceDocumentViewModel.CreatedBy?.Id ?? null,
				CompanyId = inputInvoiceDocumentViewModel.Company?.Id ?? null,

				CreatedAt = inputInvoiceDocumentViewModel.CreatedAt,
				UpdatedAt = inputInvoiceDocumentViewModel.UpdatedAt
			};

			return InputInvoiceDocument;
		}
	}
}
