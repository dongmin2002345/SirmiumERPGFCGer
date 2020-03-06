using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.ToDos
{
    public class ToDoStatus : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

    }
}
