using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Vats;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Vats
{
    public static class VatMapper
    {
        public static List<VatViewModel> ConvertToVatViewModelList(this IEnumerable<Vat> Vats)
        {
            List<VatViewModel> VatViewModels = new List<VatViewModel>();
            foreach (Vat Vat in Vats)
            {
                VatViewModels.Add(Vat.ConvertToVatViewModel());
            }
            return VatViewModels;
        }

        public static VatViewModel ConvertToVatViewModel(this Vat Vat)
        {
            VatViewModel VatViewModel = new VatViewModel()
            {
                Id = Vat.Id,
                Identifier = Vat.Identifier,

                Code = Vat.Code,
                Amount = Vat.Amount,
                Description = Vat.Description,

                IsActive = Vat.Active,

                CreatedBy = Vat.CreatedBy?.ConvertToUserViewModelLite(),
                Company = Vat.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = Vat.UpdatedAt,
                CreatedAt = Vat.CreatedAt,
            };

            return VatViewModel;
        }

        public static VatViewModel ConvertToVatViewModelLite(this Vat Vat)
        {
            VatViewModel VatViewModel = new VatViewModel()
            {
                Id = Vat.Id,
                Identifier = Vat.Identifier,

                Code = Vat.Code,
                Amount = Vat.Amount,
                Description = Vat.Description,

                IsActive = Vat.Active,

                UpdatedAt = Vat.UpdatedAt,
                CreatedAt = Vat.CreatedAt
            };

            return VatViewModel;
        }

        public static Vat ConvertToVat(this VatViewModel VatViewModel)
        {
            Vat Vat = new Vat()
            {
                Id = VatViewModel.Id,
                Identifier = VatViewModel.Identifier,

                Code = VatViewModel.Code,
                Amount = VatViewModel.Amount,
                Description = VatViewModel.Description,

                Active = VatViewModel.IsActive,

                CreatedById = VatViewModel.CreatedBy?.Id ?? null,
                CompanyId = VatViewModel.Company?.Id ?? null,

                CreatedAt = VatViewModel.CreatedAt,
                UpdatedAt = VatViewModel.UpdatedAt,
            };

            return Vat;
        }
    }
}
