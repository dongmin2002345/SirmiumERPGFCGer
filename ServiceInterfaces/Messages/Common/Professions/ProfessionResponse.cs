using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Professions
{
    public class ProfessionResponse : BaseResponse
    {
        public ProfessionViewModel Profession { get; set; }
    }
}
