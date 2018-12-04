using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Limitations
{
    public class LimitationResponse : BaseResponse
    {
        public LimitationViewModel Limitation { get; set; }
    }
}
