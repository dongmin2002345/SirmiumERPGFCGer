using DataMapper.Mappers.Common.BusinessPartners;
using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DataMapper.Mappers.Common.Prices;
using DataMapper.Mappers.Vats;
using DomainCore.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Invoices
{
    public static class InvoiceMapper
    {
        public static List<InvoiceViewModel> ConvertToInvoiceViewModelList(this IEnumerable<Invoice> invoices)
        {
            List<InvoiceViewModel> invoiceViewModels = new List<InvoiceViewModel>();
            foreach (Invoice invoice in invoices)
            {
                invoiceViewModels.Add(invoice.ConvertToInvoiceViewModel());
            }
            return invoiceViewModels;
        }

        public static InvoiceViewModel ConvertToInvoiceViewModel(this Invoice invoice)
        {
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel()
            {
                Id = invoice.Id,
                Identifier = invoice.Identifier,
                Code = invoice.Code,

                InvoiceNumber = invoice.InvoiceNumber,
                Buyer = invoice?.Buyer?.ConvertToBusinessPartnerViewModelLite(),
                BuyerName = invoice.BuyerName,
                Address = invoice.Address,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                DateOfPayment = invoice.DateOfPayment,
                Status = invoice.Status,
                StatusDate = invoice.StatusDate,
                Description = invoice.Description,
                CurrencyCode = invoice.CurrencyCode,
                CurrencyExchangeRate = invoice.CurrencyExchangeRate,                
                
                City = invoice.City?.ConvertToCityViewModelLite(),
                Municipality = invoice.Municipality?.ConvertToMunicipalityViewModelLite(),
                Discount = invoice.Discount?.ConvertToDiscountViewModelLite(),
                Vat = invoice.Vat?.ConvertToVatViewModelLite(),

                PdvType = invoice.PdvType,
                IsActive = invoice.Active,

                Company = invoice.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = invoice.CreatedBy?.ConvertToUserViewModelLite(),

                UpdatedAt = invoice.UpdatedAt,
                CreatedAt = invoice.CreatedAt,
            };

            return invoiceViewModel;
        }

        public static InvoiceViewModel ConvertToInvoiceViewModelLite(this Invoice invoice)
        {
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel()
            {
                Id = invoice.Id,
                Identifier = invoice.Identifier,
                Code = invoice.Code,

                InvoiceNumber = invoice.InvoiceNumber,
                BuyerName = invoice.BuyerName,
                Address = invoice.Address,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                DateOfPayment = invoice.DateOfPayment,
                Status = invoice.Status,
                StatusDate = invoice.StatusDate,
                Description = invoice.Description,
                CurrencyCode = invoice.CurrencyCode,
                CurrencyExchangeRate = invoice.CurrencyExchangeRate,

                PdvType = invoice.PdvType,
                IsActive = invoice.Active,

                UpdatedAt = invoice.UpdatedAt,
                CreatedAt = invoice.CreatedAt,
            };

            return invoiceViewModel;
        }

        public static Invoice ConvertToInvoice(this InvoiceViewModel invoiceViewModel)
        {
            Invoice outputinvoice = new Invoice()
            {
                Id = invoiceViewModel.Id,
                Identifier = invoiceViewModel.Identifier,
                Code = invoiceViewModel.Code,

                InvoiceNumber = invoiceViewModel.InvoiceNumber,

                BuyerId = invoiceViewModel.Buyer?.Id ?? null,
                BuyerName = invoiceViewModel.BuyerName,
                Address = invoiceViewModel.Address,
                InvoiceDate = invoiceViewModel.InvoiceDate,
                DueDate = invoiceViewModel.DueDate,
                DateOfPayment = invoiceViewModel.DateOfPayment,
                Status = invoiceViewModel.Status,
                StatusDate = invoiceViewModel.StatusDate,
                Description = invoiceViewModel.Description,
                CurrencyCode = invoiceViewModel.CurrencyCode,
                CurrencyExchangeRate = invoiceViewModel.CurrencyExchangeRate,

                CityId = invoiceViewModel.City?.Id ?? null,
                MunicipalityId = invoiceViewModel.Municipality?.Id ?? null,
                DiscountId = invoiceViewModel.Discount?.Id ?? null,
                VatId = invoiceViewModel.Vat?.Id ?? null,
                CreatedById = invoiceViewModel.CreatedBy?.Id ?? null,
                CompanyId = invoiceViewModel.Company?.Id ?? null,

                PdvType = invoiceViewModel.PdvType,
                Active = invoiceViewModel.IsActive,
                
                UpdatedAt = invoiceViewModel.UpdatedAt,
                CreatedAt = invoiceViewModel.CreatedAt,
            };

            return outputinvoice;
        }
    }
}
