using DataMapper.Mappers.Common.Phonebooks;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Phonebooks
{
    public class PhonebookPhoneService : IPhonebookPhoneService
    {
        IUnitOfWork unitOfWork;

        public PhonebookPhoneService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhonebookPhoneListResponse Sync(SyncPhonebookPhoneRequest request)
        {
            PhonebookPhoneListResponse response = new PhonebookPhoneListResponse();
            try
            {
                response.PhonebookPhones = new List<PhonebookPhoneViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhonebookPhones.AddRange(unitOfWork.GetPhonebookPhoneRepository()
                        .GetPhonebookPhonesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhonebookPhoneViewModelList() ?? new List<PhonebookPhoneViewModel>());
                }
                else
                {
                    response.PhonebookPhones.AddRange(unitOfWork.GetPhonebookPhoneRepository()
                        .GetPhonebookPhones(request.CompanyId)
                        ?.ConvertToPhonebookPhoneViewModelList() ?? new List<PhonebookPhoneViewModel>());
                }

                response.Success = true;
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
