using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Phonebooks
{
    public class PhonebookDocumentListResponse : BaseResponse
    {
        public List<PhonebookDocumentViewModel> PhonebookDocuments { get; set; }
        public int TotalItems { get; set; }
    }
}
