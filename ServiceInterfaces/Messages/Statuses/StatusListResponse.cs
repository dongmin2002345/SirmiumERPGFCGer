using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Statuses
{
    public class StatusListResponse : BaseResponse
    {
        public List<StatusViewModel> Statuses { get; set; }
        public int TotalItems { get; set; }
    }
}
