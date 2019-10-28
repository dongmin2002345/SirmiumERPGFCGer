using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.CallCentars
{
    public class CallCentarListResponse : BaseResponse
    {
        public List<CallCentarViewModel> CallCentars { get; set; }
        public int TotalItems { get; set; }
    }
}
