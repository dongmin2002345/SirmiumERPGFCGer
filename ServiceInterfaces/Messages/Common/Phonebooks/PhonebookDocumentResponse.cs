using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Phonebooks
{
    public class PhonebookDocumentResponse : BaseResponse
    {
        public PhonebookDocumentViewModel PhonebookDocument { get; set; }
    }
}
