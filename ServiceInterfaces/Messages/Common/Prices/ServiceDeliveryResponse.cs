using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Prices
{
    public class ServiceDeliveryResponse : BaseResponse
    {
        public ServiceDeliveryViewModel ServiceDelivery { get; set; }
    }
}
