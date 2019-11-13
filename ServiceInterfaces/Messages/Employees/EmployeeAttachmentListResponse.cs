using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Employees
{
    public class EmployeeAttachmentListResponse : BaseResponse
    {
        public List<EmployeeAttachmentViewModel> EmployeeAttachments { get; set; }
        public int TotalItems { get; set; }
    }
}
