using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Phonebooks
{
    public class PhonebookPhoneListResponse : BaseResponse
    {
        public List<PhonebookPhoneViewModel> PhonebookPhones { get; set; }
        public int TotalItems { get; set; }
    }
}
