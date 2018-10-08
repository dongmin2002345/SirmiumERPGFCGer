using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Professions
{
    public class ProfessionListResponse : BaseResponse
    {
        public List<ProfessionViewModel> Professions { get; set; }
        public int TotalItems { get; set; }
    }
}
