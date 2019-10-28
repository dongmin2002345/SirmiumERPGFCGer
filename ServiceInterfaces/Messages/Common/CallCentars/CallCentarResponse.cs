using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.CallCentars
{
    public class CallCentarResponse : BaseResponse
    {
        public CallCentarViewModel CallCentar { get; set; }
    }
}
