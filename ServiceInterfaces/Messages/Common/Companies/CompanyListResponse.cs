using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Companies
{
    public class CompanyListResponse : BaseResponse
    {
        public List<CompanyViewModel> Companies { get; set; }
    }
}
