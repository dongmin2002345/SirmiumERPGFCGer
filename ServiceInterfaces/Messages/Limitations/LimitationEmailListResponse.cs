using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Limitations
{
    public class LimitationEmailListResponse : BaseResponse
    {
        public List<LimitationEmailViewModel> LimitationEmails { get; set; }
        public int TotalItems { get; set; }
    }
}
