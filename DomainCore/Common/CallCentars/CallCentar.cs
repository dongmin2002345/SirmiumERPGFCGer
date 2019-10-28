using DomainCore.Base;
using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.CallCentars
{
    public class CallCentar : BaseEntity
    {
        public string Code { get; set; }
        public DateTime ReceivingDate { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }

        public DateTime EndingDate { get; set; }


    }
}
