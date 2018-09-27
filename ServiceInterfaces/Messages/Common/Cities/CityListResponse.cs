using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Cities
{
    public class CityListResponse : BaseResponse
    {
        public List<CityViewModel> Cities { get; set; }
        public int TotalItems { get; set; }
    }
}
