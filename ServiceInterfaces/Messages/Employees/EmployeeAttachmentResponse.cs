﻿using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Employees
{
    public class EmployeeAttachmentResponse : BaseResponse
    {
        public EmployeeAttachmentViewModel EmployeeAttachment { get; set; }
    }
}
