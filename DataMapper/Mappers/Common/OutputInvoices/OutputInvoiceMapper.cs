using DataMapper.Mappers.Common.Companies;
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

                Code = outputInvoice.Code,

                Construction = outputInvoice.Construction,

                InvoiceDate = outputInvoice.InvoiceDate,

                BusinessPartner = outputInvoice.BusinessPartner,

                InvoiceType = outputInvoice.InvoiceType,

                Quantity = outputInvoice.Quantity,

                TrafficDate = outputInvoice.TrafficDate,

                Price = outputInvoice.Price,
                Rebate = outputInvoice.Rebate,
                RebateValue = outputInvoice.RebateValue,
                Base = outputInvoice.Base,
                PDV = outputInvoice.PDV,
                Total = outputInvoice.Total,

                IsActive = outputInvoice.Active,

                Company = outputInvoice.Company?.ConvertToCompanyViewModelLite(),

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

                Code = outputInvoice.Code,

                Construction = outputInvoice.Construction,

                InvoiceDate = outputInvoice.InvoiceDate,

                BusinessPartner = outputInvoice.BusinessPartner,

                InvoiceType = outputInvoice.InvoiceType,

                Quantity = outputInvoice.Quantity,

                TrafficDate = outputInvoice.TrafficDate,

                Price = outputInvoice.Price,
                Rebate = outputInvoice.Rebate,
                RebateValue = outputInvoice.RebateValue,
                Base = outputInvoice.Base,
                PDV = outputInvoice.PDV,
                Total = outputInvoice.Total,

                IsActive = outputInvoice.Active,

                Company = outputInvoice.Company?.ConvertToCompanyViewModelLite(),

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

                Code = outputInvoiceViewModel.Code,

                Construction = outputInvoiceViewModel.Construction,

                InvoiceDate = outputInvoiceViewModel.InvoiceDate,

                BusinessPartner = outputInvoiceViewModel.BusinessPartner,

                InvoiceType = outputInvoiceViewModel.InvoiceType,

                Quantity = outputInvoiceViewModel.Quantity,

                TrafficDate = outputInvoiceViewModel.TrafficDate,

                Price = outputInvoiceViewModel.Price,
                Rebate = outputInvoiceViewModel.Rebate,
                RebateValue = outputInvoiceViewModel.RebateValue,
                Base = outputInvoiceViewModel.Base,
                PDV = outputInvoiceViewModel.PDV,
                Total = outputInvoiceViewModel.Total,

                //IsLocked = outputInvoiceViewModel.IsLocked,
                //LockedDate = outputInvoiceViewModel.LockedDate,

                CreatedBy = new User() { Id = outputInvoiceViewModel.CreatedBy?.Id ?? 0 },

                //InvoiceItems = outputInvoiceViewModel.OutputInvoiceSubItems?.ConvertToOutputInvoiceSubItemList(),

                //Status = 0,

                CreatedAt = outputInvoiceViewModel.CreatedAt,
                //IsCancelItem = outputInvoiceViewModel.IsCancelItem
            };

            return outputinvoice;
        }
    }
}

