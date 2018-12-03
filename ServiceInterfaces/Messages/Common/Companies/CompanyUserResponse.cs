using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Companies
{
    public class CompanyUserResponse : BaseResponse
    {
        public CompanyUserViewModel CompanyUser { get; set; }
    }
}
