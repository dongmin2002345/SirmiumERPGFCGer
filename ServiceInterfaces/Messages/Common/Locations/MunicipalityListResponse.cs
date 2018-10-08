using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Locations
{
    public class MunicipalityListResponse : BaseResponse
    {
        public List<MunicipalityViewModel> Municipalities { get; set; }
        public int TotalItems { get; set; }
    }
}
