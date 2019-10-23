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
    public class PhonebookPhoneService : IPhonebookPhoneService
    {
        public PhonebookPhoneListResponse Sync(SyncPhonebookPhoneRequest request)
        {
            PhonebookPhoneListResponse response = new PhonebookPhoneListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhonebookPhoneRequest, PhonebookPhoneViewModel, PhonebookPhoneListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.PhonebookPhones = new List<PhonebookPhoneViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
