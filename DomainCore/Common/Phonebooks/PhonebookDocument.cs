using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Phonebooks
{
    public class PhonebookDocument : BaseEntity
    {
        public int? PhonebookId { get; set; }
        public Phonebook Phonebook { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }
        public int ItemStatus { get; set; }
    }
}
