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
    public class PhonebookService : IPhonebookService
    {
        public PhonebookListResponse GetPhonebooks(int companyId)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<PhonebookViewModel>, PhonebookListResponse>("GetPhonebooks", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Phonebooks = new List<PhonebookViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public PhonebookListResponse GetPhonebooksNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<PhonebookViewModel>, PhonebookListResponse>("GetPhonebooksNewerThan", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Phonebooks = new List<PhonebookViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public PhonebookResponse Create(PhonebookViewModel re)
        {
            PhonebookResponse response = new PhonebookResponse();
            try
            {
                response = WpfApiHandler.SendToApi<PhonebookViewModel, PhonebookResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.Phonebook = new PhonebookViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public PhonebookResponse Delete(Guid identifier)
        {
            PhonebookResponse response = new PhonebookResponse();
            try
            {
                PhonebookViewModel re = new PhonebookViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<PhonebookViewModel, PhonebookResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Phonebook = new PhonebookViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public PhonebookListResponse Sync(SyncPhonebookRequest request)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncPhonebookRequest, PhonebookViewModel, PhonebookListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Phonebooks = new List<PhonebookViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
