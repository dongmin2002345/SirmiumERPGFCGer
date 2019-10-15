using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Prices
{
    public static class DiscountMapper
    { 
   public static List<DiscountViewModel> ConvertToDiscountViewModelList(this IEnumerable<Discount> discounts)
    {
        List<DiscountViewModel> discountViewModels = new List<DiscountViewModel>();
        foreach (Discount discount in discounts)
        {
            discountViewModels.Add(discount.ConvertToDiscountViewModel());
        }
        return discountViewModels;
    }

    public static DiscountViewModel ConvertToDiscountViewModel(this Discount discount)
    {
        DiscountViewModel discountViewModel = new DiscountViewModel()
        {
            Id = discount.Id,
            Identifier = discount.Identifier,

            Code = discount.Code,
            Name = discount.Name,
            Amount = discount.Amount,

            IsActive = discount.Active,

            CreatedBy = discount.CreatedBy?.ConvertToUserViewModelLite(),
            Company = discount.Company?.ConvertToCompanyViewModelLite(),

            UpdatedAt = discount.UpdatedAt,
            CreatedAt = discount.CreatedAt,
        };

        return discountViewModel;
    }

    public static DiscountViewModel ConvertToDiscountViewModelLite(this Discount discount)
    {
        DiscountViewModel discountViewModel = new DiscountViewModel()
        {
            Id = discount.Id,
            Identifier = discount.Identifier,

            Code = discount.Code,
            Name = discount.Name,
            Amount = discount.Amount,

            IsActive = discount.Active,

            UpdatedAt = discount.UpdatedAt,
            CreatedAt = discount.CreatedAt
        };

        return discountViewModel;
    }

    public static Discount ConvertToDiscount(this DiscountViewModel discountViewModel)
    {
        Discount discount = new Discount()
        {
            Id = discountViewModel.Id,
            Identifier = discountViewModel.Identifier,

            Code = discountViewModel.Code,
            Name = discountViewModel.Name,
            Amount = discountViewModel.Amount,

            Active = discountViewModel.IsActive,

            CreatedById = discountViewModel.CreatedBy?.Id ?? null,
            CompanyId = discountViewModel.Company?.Id ?? null,

            CreatedAt = discountViewModel.CreatedAt,
            UpdatedAt = discountViewModel.UpdatedAt,
        };

        return discount;
    }
}
}


