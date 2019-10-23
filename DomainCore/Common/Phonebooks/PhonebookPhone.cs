using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Phonebooks
{
    public class PhonebookPhone : BaseEntity
    {
        public int? PhonebookId { get; set; }
        public Phonebook Phonebook { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ItemStatus { get; set; }
    }
}
