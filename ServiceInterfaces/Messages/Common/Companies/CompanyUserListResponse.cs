using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Companies
{
    public class CompanyUserListResponse : BaseResponse
    {
        public List<CompanyUserViewModel> CompanyUsers { get; set; }
        public int TotalItems { get; set; }
    }
}
