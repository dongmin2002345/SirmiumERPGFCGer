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

                AmountNet = outputInvoice.AmountNet,

                PdvPercent = outputInvoice.PdvPercent,

                Pdv = outputInvoice.Pdv,

                AmountGross = outputInvoice.AmountGross,

                DateOfPayment = outputInvoice.DateOfPayment,
                Status = outputInvoice.Status,
                StatusDate = outputInvoice.StatusDate,
                //Base = outputInvoice.Base,
                //PDV = outputInvoice.PDV,
                //Total = outputInvoice.Total,

                //IsActive = outputInvoice.Active,

                Company = outputInvoice.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = outputInvoice.CreatedBy?.ConvertToUserViewModelLite(),
                BusinessPartner = outputInvoice.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                UpdatedAt = outputInvoice.UpdatedAt,
                CreatedAt = outputInvoice.CreatedAt,
                //IsCancelItem = outputInvoice.IsCancelItem,
            };

            //if (outputInvoice.InvoiceItems != null && outputInvoice.InvoiceItems.Count > 0)
            //{
            //    outputInvoiceViewModel.OutputInvoiceSubItems = new ObservableCollection<OutputInvoiceSubItemViewModel>(outputInvoice.InvoiceItems.ConvertToOutputInvoiceSubItemViewModelListLite());
            //}
            //else
            //{
            //    outputInvoiceViewModel.OutputInvoiceSubItems = new ObservableCollection<OutputInvoiceSubItemViewModel>();
            //}

            return outputInvoiceViewModel;
        }

        public static List<OutputInvoiceViewModel> ConvertToOutputInvoiceViewModelListLite(this IEnumerable<OutputInvoice> outputInvoices)
        {
            List<OutputInvoiceViewModel> outputInvoiceViewModels = new List<OutputInvoiceViewModel>();
            foreach (OutputInvoice outputInvoice in outputInvoices)
            {
                outputInvoiceViewModels.Add(outputInvoice.ConvertToOutputInvoiceViewModelLite());
            }
            return outputInvoiceViewModels;
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

                AmountNet = outputInvoice.AmountNet,

                PdvPercent = outputInvoice.PdvPercent,

                Pdv = outputInvoice.Pdv,

                AmountGross = outputInvoice.AmountGross,

                DateOfPayment = outputInvoice.DateOfPayment,
                Status = outputInvoice.Status,
                StatusDate = outputInvoice.StatusDate,

                //Company = outputInvoice.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = outputInvoice.UpdatedAt,
                CreatedAt = outputInvoice.CreatedAt,
                //IsCancelItem = outputInvoice.IsCancelItem,
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

                AmountNet = outputInvoiceViewModel.AmountNet,

                PdvPercent = outputInvoiceViewModel.PdvPercent,

                Pdv = outputInvoiceViewModel.Pdv,

                AmountGross = outputInvoiceViewModel.AmountGross,

                DateOfPayment = outputInvoiceViewModel.DateOfPayment,
                Status = outputInvoiceViewModel.Status,
                StatusDate = outputInvoiceViewModel.StatusDate,


                BusinessPartnerId = outputInvoiceViewModel.BusinessPartner?.Id ?? null,
                CreatedById = outputInvoiceViewModel.CreatedBy?.Id ?? null,
                CompanyId = outputInvoiceViewModel.Company?.Id ?? null,

                CreatedAt = outputInvoiceViewModel.CreatedAt,
                UpdatedAt = outputInvoiceViewModel.UpdatedAt

                //IsLocked = outputInvoiceViewModel.IsLocked,
                //LockedDate = outputInvoiceViewModel.LockedDate,

                //CreatedBy = new User() { Id = outputInvoiceViewModel.CreatedBy?.Id ?? 0 },

                //InvoiceItems = outputInvoiceViewModel.OutputInvoiceSubItems?.ConvertToOutputInvoiceSubItemList(),

                //Status = 0,

                //CreatedAt = outputInvoiceViewModel.CreatedAt,
                //IsCancelItem = outputInvoiceViewModel.IsCancelItem
            };

            return outputinvoice;
        }
    }
}

