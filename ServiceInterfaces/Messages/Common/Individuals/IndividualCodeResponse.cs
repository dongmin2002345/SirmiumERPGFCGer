using ServiceInterfaces.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Individuals
{
    public class IndividualCodeResponse : BaseResponse
    {
        public int Code { get; set; }
    }
}
