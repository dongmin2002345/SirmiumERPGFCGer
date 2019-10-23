using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Phonebooks
{
    public class PhonebookDocumentService : IPhonebookDocumentService
    {
        public PhonebookDocumentListResponse Sync(SyncPhonebookDocumentRequest request)
        {
            PhonebookDocumentListResponse response = new PhonebookDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhonebookDocumentRequest, PhonebookDocumentViewModel, PhonebookDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhonebookDocuments = new List<PhonebookDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
