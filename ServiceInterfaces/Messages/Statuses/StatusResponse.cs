using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Statuses
{
    public class StatusResponse : BaseResponse
    {
        public StatusViewModel Status { get; set; }
    }
}
