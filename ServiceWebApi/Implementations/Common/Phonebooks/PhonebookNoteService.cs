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
    public class PhonebookNoteService : IPhonebookNoteService
    {
        public PhonebookNoteListResponse Sync(SyncPhonebookNoteRequest request)
        {
            PhonebookNoteListResponse response = new PhonebookNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhonebookNoteRequest, PhonebookNoteViewModel, PhonebookNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhonebookNotes = new List<PhonebookNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
