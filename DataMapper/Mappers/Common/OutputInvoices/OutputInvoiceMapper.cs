using DataMapper.Mappers.Common.BusinessPartners;
using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Identity;
using DomainCore.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.OutputInvoices
{
    public static class OutputInvoiceMapper
    {
        public static List<OutputInvoiceViewModel> ConvertToOutputInvoiceViewModelList(this IEnumerable<OutputInvoice> outputInvoices)
        {
            List<OutputInvoiceViewModel> outputInvoiceViewModels = new List<OutputInvoiceViewModel>();
            foreach (OutputInvoice outputInvoice in outputInvoices)
            {
                outputInvoiceViewModels.Add(outputInvoice.ConvertToOutputInvoiceViewModel());
            }
            return outputInvoiceViewModels;
        }

        public static OutputInvoiceViewModel ConvertToOutputInvoiceViewModel(this OutputInvoice outputInvoice)
        {
            OutputInvoiceViewModel outputInvoiceViewModel = new OutputInvoiceViewModel()
            {
                Id = outputInvoice.Id,
                Identifier = outputInvoice.Identifier,
                Code = outputInvoice.Code,

                Supplier = outputInvoice.Supplier,
                Address = outputInvoice.Address,
                InvoiceNumber = outputInvoice.InvoiceNumber,
                InvoiceDate = outputInvoice.InvoiceDate,
                Currency = outputInvoice.Currency,

                AmountNet = outputInvoice.AmountNet,

                PdvPercent = outputInvoice.PdvPercent,

                Pdv = outputInvoice.Pdv,

                AmountGross = outputInvoice.AmountGross,

                DateOfPayment = outputInvoice.DateOfPayment,
                Status = outputInvoice.Status,
                StatusDate = outputInvoice.StatusDate,
                
                Description = outputInvoice.Description,

                Path = outputInvoice.Path,

                IsActive = outputInvoice.Active,

                Company = outputInvoice.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = outputInvoice.CreatedBy?.ConvertToUserViewModelLite(),
                BusinessPartner = outputInvoice.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                UpdatedAt = outputInvoice.UpdatedAt,
                CreatedAt = outputInvoice.CreatedAt,
            };

            return outputInvoiceViewModel;
        }

        public static OutputInvoiceViewModel ConvertToOutputInvoiceViewModelLite(this OutputInvoice outputInvoice)
        {
            OutputInvoiceViewModel outputInvoiceViewModel = new OutputInvoiceViewModel()
            {
                Id = outputInvoice.Id,
                Identifier = outputInvoice.Identifier,
                Code = outputInvoice.Code,

                Supplier = outputInvoice.Supplier,
                Address = outputInvoice.Address,
                InvoiceNumber = outputInvoice.InvoiceNumber,
                InvoiceDate = outputInvoice.InvoiceDate,
                Currency = outputInvoice.Currency,


                AmountNet = outputInvoice.AmountNet,

                PdvPercent = outputInvoice.PdvPercent,

                Pdv = outputInvoice.Pdv,

                AmountGross = outputInvoice.AmountGross,

                DateOfPayment = outputInvoice.DateOfPayment,
                Status = outputInvoice.Status,
                StatusDate = outputInvoice.StatusDate,

                Description = outputInvoice.Description,

                Path = outputInvoice.Path,

                IsActive = outputInvoice.Active,

                UpdatedAt = outputInvoice.UpdatedAt,
                CreatedAt = outputInvoice.CreatedAt,
            };

            return outputInvoiceViewModel;
        }

        public static OutputInvoice ConvertToOutputInvoice(this OutputInvoiceViewModel outputInvoiceViewModel)
        {
            OutputInvoice outputinvoice = new OutputInvoice()
            {
                Id = outputInvoiceViewModel.Id,
                Identifier = outputInvoiceViewModel.Identifier,
                Code = outputInvoiceViewModel.Code,

                Supplier = outputInvoiceViewModel.Supplier,
                Address = outputInvoiceViewModel.Address,
                InvoiceNumber = outputInvoiceViewModel.InvoiceNumber,
                InvoiceDate = outputInvoiceViewModel.InvoiceDate,
                Currency = outputInvoiceViewModel.Currency,

                AmountNet = outputInvoiceViewModel.AmountNet,

                PdvPercent = outputInvoiceViewModel.PdvPercent,

                Pdv = outputInvoiceViewModel.Pdv,

                AmountGross = outputInvoiceViewModel.AmountGross,

                DateOfPayment = outputInvoiceViewModel.DateOfPayment,
                Status = outputInvoiceViewModel.Status,
                StatusDate = outputInvoiceViewModel.StatusDate,

                Description = outputInvoiceViewModel.Description,

                Path = outputInvoiceViewModel.Path,

                Active = outputInvoiceViewModel.IsActive,

                BusinessPartnerId = outputInvoiceViewModel.BusinessPartner?.Id ?? null,
                CreatedById = outputInvoiceViewModel.CreatedBy?.Id ?? null,
                CompanyId = outputInvoiceViewModel.Company?.Id ?? null,

                CreatedAt = outputInvoiceViewModel.CreatedAt,
                UpdatedAt = outputInvoiceViewModel.UpdatedAt
            };

            return outputinvoice;
        }
    }
}

