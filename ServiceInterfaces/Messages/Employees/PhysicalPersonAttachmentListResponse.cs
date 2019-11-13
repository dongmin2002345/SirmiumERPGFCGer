using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Employees
{
    public class PhysicalPersonAttachmentListResponse : BaseResponse
    {
        public List<PhysicalPersonAttachmentViewModel> PhysicalPersonAttachments { get; set; }
        public int TotalItems { get; set; }
    }
}
