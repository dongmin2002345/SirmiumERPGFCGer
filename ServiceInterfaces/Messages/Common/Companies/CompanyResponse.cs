using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Companies
{
    public class CompanyResponse : BaseResponse
    {
        public CompanyViewModel Company { get; set; }
    }
}
