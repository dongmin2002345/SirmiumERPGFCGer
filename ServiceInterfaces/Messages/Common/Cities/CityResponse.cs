using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Cities
{
    public class CityResponse : BaseResponse
    {
        public CityViewModel City { get; set; }
    }
}
