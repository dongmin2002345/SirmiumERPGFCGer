﻿using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Prices
{
    public class DiscountListResponse : BaseResponse
    {
        public List<DiscountViewModel> Discounts { get; set; }
        public int TotalItems { get; set; }
    }
}
