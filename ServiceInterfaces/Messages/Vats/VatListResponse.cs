using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Vats
{
    public class VatListResponse : BaseResponse
    {
        public List<VatViewModel> Vats { get; set; }
        public int TotalItems { get; set; }
    }
}
