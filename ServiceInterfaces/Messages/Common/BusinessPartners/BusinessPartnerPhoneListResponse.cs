using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.BusinessPartners
{
    public class BusinessPartnerPhoneListResponse : BaseResponse
    {
        public List<BusinessPartnerPhoneViewModel> BusinessPartnerPhones { get; set; }
        public int TotalItems { get; set; }
    }
}
