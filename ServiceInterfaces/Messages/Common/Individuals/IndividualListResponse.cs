using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Individuals
{
    public class IndividualListResponse : BaseResponse
    {
        public List<IndividualViewModel> Individuals { get; set; }
        public List<IndividualViewModel> IndividualsByPage { get; set; }
        public int TotalItems { get; set; }
    }
}
