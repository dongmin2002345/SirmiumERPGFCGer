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
                InvoiceDate = invoice.InvoiceDate,
                DateOfSupplyOfGoods = invoice.DateOfSupplyOfGoods,

                Customer = invoice.Customer,
                PIB = invoice.PIB,
                BPName = invoice.BPName,
                Address = invoice.Address,

                Currency = invoice.Currency,
                IsInPDV = invoice.IsInPDV,
                PdvType = invoice.PdvType,

                IsActive = invoice.Active,

                Company = invoice.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = invoice.CreatedBy?.ConvertToUserViewModelLite(),
                BusinessPartner = invoice.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),
                City = invoice.City?.ConvertToCityViewModelLite(),
                Municipality = invoice.Municipality?.ConvertToMunicipalityViewModelLite(),
                Discount = invoice.Discount?.ConvertToDiscountViewModelLite(),
                Vat = invoice.Vat?.ConvertToVatViewModelLite(),

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
                InvoiceDate = invoice.InvoiceDate,
                DateOfSupplyOfGoods = invoice.DateOfSupplyOfGoods,

                Customer = invoice.Customer,
                PIB = invoice.PIB,
                BPName = invoice.BPName,
                Address = invoice.Address,

                Currency = invoice.Currency,
                IsInPDV = invoice.IsInPDV,
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
                InvoiceDate = invoiceViewModel.InvoiceDate,
                DateOfSupplyOfGoods = invoiceViewModel.DateOfSupplyOfGoods,

                Customer = invoiceViewModel.Customer,
                PIB = invoiceViewModel.PIB,
                BPName = invoiceViewModel.BPName,
                Address = invoiceViewModel.Address,
                IsInPDV = invoiceViewModel.IsInPDV,
                PdvType = invoiceViewModel.PdvType,

                Currency = invoiceViewModel.Currency,

                Active = invoiceViewModel.IsActive,

                BusinessPartnerId = invoiceViewModel.BusinessPartner?.Id ?? null,
                CityId = invoiceViewModel.City?.Id ?? null,
                MunicipalityId = invoiceViewModel.Municipality?.Id ?? null,
                DiscountId = invoiceViewModel.Discount?.Id ?? null,
                VatId = invoiceViewModel.Vat?.Id ?? null,
                CreatedById = invoiceViewModel.CreatedBy?.Id ?? null,
                CompanyId = invoiceViewModel.Company?.Id ?? null,

                CreatedAt = invoiceViewModel.CreatedAt,
                UpdatedAt = invoiceViewModel.UpdatedAt
            };

            return outputinvoice;
        }
    }
}
