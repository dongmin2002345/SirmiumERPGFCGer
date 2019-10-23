using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Phonebooks
{
    public class PhonebookNoteResponse : BaseResponse
    {
        public PhonebookNoteViewModel PhonebookNote { get; set; }
    }
}
