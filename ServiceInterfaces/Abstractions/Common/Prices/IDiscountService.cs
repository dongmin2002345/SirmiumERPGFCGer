using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Prices
{
    public interface IDiscountService
    {
        DiscountListResponse GetDiscounts(int companyId);

        DiscountResponse Create(DiscountViewModel discount);
        DiscountResponse Delete(Guid identifier);

        DiscountListResponse Sync(SyncDiscountRequest request);
    }
}
