using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Vats
{
    public class VatResponse : BaseResponse
    {
        public VatViewModel Vat { get; set; }
    }
}
