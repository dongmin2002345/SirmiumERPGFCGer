using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Individuals
{
    public class IndividualResponse : BaseResponse
    {
        public IndividualViewModel Individual { get; set; }
    }
}
