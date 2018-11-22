using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.TaxAdministrations
{
    public class TaxAdministrationListResponse : BaseResponse
    {
        public List<TaxAdministrationViewModel> TaxAdministrations { get; set; }
        public int TotalItems { get; set; }
    }
}
