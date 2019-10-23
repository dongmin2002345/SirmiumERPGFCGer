using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Phonebooks
{
    public class PhonebookNote : BaseEntity
    {
        public int? PhonebookId { get; set; }
        public Phonebook Phonebook { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
        public int ItemStatus { get; set; }
    }
}
