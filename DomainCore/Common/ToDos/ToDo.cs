using DomainCore.Base;
using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.ToDos
{
    public class ToDo : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Path { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime ToDoDate { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? ToDoStatusId { get; set; }
        public ToDoStatus ToDoStatus { get; set; }
    }
}
