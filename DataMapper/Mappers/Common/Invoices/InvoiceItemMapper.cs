using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Invoices
{
    public static class InvoiceItemMapper
    {
        public static List<InvoiceItemViewModel> ConvertToInvoiceItemViewModelList(this IEnumerable<InvoiceItem> InvoiceItems)
        {
            List<InvoiceItemViewModel> InvoiceItemViewModels = new List<InvoiceItemViewModel>();
            foreach (InvoiceItem InvoiceItem in InvoiceItems)
            {
                InvoiceItemViewModels.Add(InvoiceItem.ConvertToInvoiceItemViewModel());
            }
            return InvoiceItemViewModels;
        }

        public static InvoiceItemViewModel ConvertToInvoiceItemViewModel(this InvoiceItem invoiceItem)
        {
            InvoiceItemViewModel invoiceItemViewModel = new InvoiceItemViewModel()
            {
                Id = invoiceItem.Id,
                Identifier = invoiceItem.Identifier,

                Invoice = invoiceItem.Invoice?.ConvertToInvoiceViewModelLite(),

                Code = invoiceItem.Code,
                Name = invoiceItem.Name,
                UnitOfMeasure = invoiceItem.UnitOfMeasure,
                Quantity = invoiceItem.Quantity,
                PriceWithPDV = invoiceItem.PriceWithPDV,
                PriceWithoutPDV = invoiceItem.PriceWithoutPDV,
                Discount = invoiceItem.Discount,
                PDVPercent = invoiceItem.PDVPercent,
                PDV = invoiceItem.PDV,
                Amount = invoiceItem.Amount,
                ItemStatus = invoiceItem.ItemStatus,
                CurrencyCode = invoiceItem.CurrencyCode,
                ExchangeRate = invoiceItem.ExchangeRate,
                CurrencyPriceWithPDV = invoiceItem.CurrencyPriceWithPDV,

                IsActive = invoiceItem.Active,

                CreatedBy = invoiceItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = invoiceItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = invoiceItem.UpdatedAt,
                CreatedAt = invoiceItem.CreatedAt
            };

            return invoiceItemViewModel;
        }

        public static InvoiceItemViewModel ConvertToInvoiceItemViewModelLite(this InvoiceItem invoiceItem)
        {
            InvoiceItemViewModel invoiceItemViewModel = new InvoiceItemViewModel()
            {
                Id = invoiceItem.Id,
                Identifier = invoiceItem.Identifier,

                Code = invoiceItem.Code,
                Name = invoiceItem.Name,
                UnitOfMeasure = invoiceItem.UnitOfMeasure,
                Quantity = invoiceItem.Quantity,
                PriceWithPDV = invoiceItem.PriceWithPDV,
                PriceWithoutPDV = invoiceItem.PriceWithoutPDV,
                Discount = invoiceItem.Discount,
                PDVPercent = invoiceItem.PDVPercent,
                PDV = invoiceItem.PDV,
                Amount = invoiceItem.Amount,
                ItemStatus = invoiceItem.ItemStatus,
                CurrencyCode = invoiceItem.CurrencyCode,
                ExchangeRate = invoiceItem.ExchangeRate,
                CurrencyPriceWithPDV = invoiceItem.CurrencyPriceWithPDV,

                IsActive = invoiceItem.Active,

                UpdatedAt = invoiceItem.UpdatedAt,
                CreatedAt = invoiceItem.CreatedAt
            };

            return invoiceItemViewModel;
        }

        public static InvoiceItem ConvertToInvoiceItem(this InvoiceItemViewModel invoiceItemViewModel)
        {
            InvoiceItem invoiceItem = new InvoiceItem()
            {
                Id = invoiceItemViewModel.Id,
                Identifier = invoiceItemViewModel.Identifier,

                InvoiceId = invoiceItemViewModel.Invoice?.Id ?? null,

                Code = invoiceItemViewModel.Code,
                Name = invoiceItemViewModel.Name,
                UnitOfMeasure = invoiceItemViewModel.UnitOfMeasure,
                Quantity = invoiceItemViewModel.Quantity,
                PriceWithPDV = invoiceItemViewModel.PriceWithPDV,
                PriceWithoutPDV = invoiceItemViewModel.PriceWithoutPDV,
                Discount = invoiceItemViewModel.Discount,
                PDVPercent = invoiceItemViewModel.PDVPercent,
                PDV = invoiceItemViewModel.PDV,
                Amount = invoiceItemViewModel.Amount,
                ItemStatus = invoiceItemViewModel.ItemStatus,
                CurrencyCode = invoiceItemViewModel.CurrencyCode,
                ExchangeRate = invoiceItemViewModel.ExchangeRate,
                CurrencyPriceWithPDV = invoiceItemViewModel.CurrencyPriceWithPDV,

                CreatedById = invoiceItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = invoiceItemViewModel.Company?.Id ?? null,

                CreatedAt = invoiceItemViewModel.CreatedAt,
                UpdatedAt = invoiceItemViewModel.UpdatedAt
            };

            return invoiceItem;
        }
    }
}