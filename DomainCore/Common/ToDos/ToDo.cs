using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.ToDos
{
    public class ToDo : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime ToDoDate { get; set; }
    }
}
