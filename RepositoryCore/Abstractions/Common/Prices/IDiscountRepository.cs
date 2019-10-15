using DomainCore.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Prices
{
    public interface IDiscountRepository
    {
        List<Discount> GetDiscounts(int companyId);
        List<Discount> GetDiscountsNewerThen(int companyId, DateTime lastUpdateTime);
        Discount GetDiscount(int discountId);

        Discount Create(Discount discount);
        Discount Delete(Guid identifier);
    }
}
