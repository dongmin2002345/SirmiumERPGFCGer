using DataMapper.Mappers.Common.BusinessPartners;
using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.InputInvoices
{
    public static class InputInvoiceMapper
    {
		public static List<InputInvoiceViewModel> ConvertToInputInvoiceViewModelList(this IEnumerable<InputInvoice> InputInvoices)
		{
			List<InputInvoiceViewModel> viewModels = new List<InputInvoiceViewModel>();
			foreach (InputInvoice InputInvoice in InputInvoices)
			{
				viewModels.Add(InputInvoice.ConvertToInputInvoiceViewModel());
			}
			return viewModels;
		}


		public static InputInvoiceViewModel ConvertToInputInvoiceViewModel(this InputInvoice inputInvoice)
		{
			InputInvoiceViewModel inputInvoiceViewModel = new InputInvoiceViewModel()
			{
				Id = inputInvoice.Id,
				Identifier = inputInvoice.Identifier,

				Code = inputInvoice.Code,
				Supplier = inputInvoice.Supplier,
				Address = inputInvoice.Address,
				InvoiceNumber = inputInvoice.InvoiceNumber,
				InvoiceDate = inputInvoice.InvoiceDate,
				AmountNet = inputInvoice.AmountNet,
				PDVPercent = inputInvoice.PDVPercent,
				PDV = inputInvoice.PDV,
				AmountGross = inputInvoice.AmountGross,
				Currency = inputInvoice.Currency,
				DateOfPaymet = inputInvoice.DateOfPaymet,
				Status = inputInvoice.Status,

				StatusDate = inputInvoice.StatusDate,
				Description = inputInvoice.Description,

                Path = inputInvoice.Path,

				BusinessPartner = inputInvoice.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                IsActive = inputInvoice.Active, 

				CreatedBy = inputInvoice.CreatedBy?.ConvertToUserViewModelLite(),
				Company = inputInvoice.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = inputInvoice.UpdatedAt,
				CreatedAt = inputInvoice.CreatedAt
			};



			return inputInvoiceViewModel;
		}

		public static List<InputInvoiceViewModel> ConvertToInputInvoiceViewModelListLite(this IEnumerable<InputInvoice> inputInvoices)
		{
			List<InputInvoiceViewModel> inputInvoiceViewModels = new List<InputInvoiceViewModel>();
			foreach (InputInvoice inputInvoice in inputInvoices)
			{
				inputInvoiceViewModels.Add(inputInvoice.ConvertToInputInvoiceViewModelLite());
			}
			return inputInvoiceViewModels;
		}

		public static InputInvoiceViewModel ConvertToInputInvoiceViewModelLite(this InputInvoice inputInvoice)
		{
			InputInvoiceViewModel inputInvoiceViewModel = new InputInvoiceViewModel()
			{
				Id = inputInvoice.Id,
				Identifier = inputInvoice.Identifier,

				Code = inputInvoice.Code,
				Supplier = inputInvoice.Supplier,
				Address = inputInvoice.Address,
				InvoiceNumber = inputInvoice.InvoiceNumber,
				InvoiceDate = inputInvoice.InvoiceDate,
				AmountNet = inputInvoice.AmountNet,
				PDVPercent = inputInvoice.PDVPercent,
				PDV = inputInvoice.PDV,
				AmountGross = inputInvoice.AmountGross,
				Currency = inputInvoice.Currency,
				DateOfPaymet = inputInvoice.DateOfPaymet,
				Status = inputInvoice.Status,

				StatusDate = inputInvoice.StatusDate,
				Description = inputInvoice.Description,

                Path = inputInvoice.Path,

                IsActive = inputInvoice.Active,

                UpdatedAt = inputInvoice.UpdatedAt,
				CreatedAt = inputInvoice.CreatedAt
			};


			return inputInvoiceViewModel;
		}

		public static InputInvoice ConvertToInputInvoice(this InputInvoiceViewModel inputInvoiceViewModel)
		{
			InputInvoice inputInvoice = new InputInvoice()
			{
				Id = inputInvoiceViewModel.Id,
				Identifier = inputInvoiceViewModel.Identifier,

				Code = inputInvoiceViewModel.Code,
				Supplier = inputInvoiceViewModel.Supplier,
				Address = inputInvoiceViewModel.Address,
				InvoiceNumber = inputInvoiceViewModel.InvoiceNumber,
				InvoiceDate = inputInvoiceViewModel.InvoiceDate,
				AmountNet = inputInvoiceViewModel.AmountNet,
				PDVPercent = inputInvoiceViewModel.PDVPercent,
				PDV = inputInvoiceViewModel.PDV,
				AmountGross = inputInvoiceViewModel.AmountGross,
				Currency = inputInvoiceViewModel.Currency,
				DateOfPaymet = inputInvoiceViewModel.DateOfPaymet,
				Status = inputInvoiceViewModel.Status,

				StatusDate = inputInvoiceViewModel.StatusDate,
				Description = inputInvoiceViewModel.Description,

                Path = inputInvoiceViewModel.Path,

                BusinessPartnerId = inputInvoiceViewModel.BusinessPartner?.Id ?? null,

                Active = inputInvoiceViewModel.IsActive,

                CreatedById = inputInvoiceViewModel.CreatedBy?.Id ?? null,
				CompanyId = inputInvoiceViewModel.Company?.Id ?? null,

				CreatedAt = inputInvoiceViewModel.CreatedAt,
				UpdatedAt = inputInvoiceViewModel.UpdatedAt
			};

			return inputInvoice;
		}

	}
}
