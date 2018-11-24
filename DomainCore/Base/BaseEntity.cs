using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using System;

namespace DomainCore.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public Guid Identifier { get; set; }

        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public bool Active { get; set; }

        
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
