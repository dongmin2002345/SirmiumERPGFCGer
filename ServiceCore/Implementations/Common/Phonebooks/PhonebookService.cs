using DataMapper.Mappers.Common.Phonebooks;
using DomainCore.Common.Phonebooks;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.Phonebooks
{
    public class PhonebookService : IPhonebookService
    {
        IUnitOfWork unitOfWork;

        public PhonebookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhonebookListResponse GetPhonebooks(int companyId)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response.Phonebooks = unitOfWork.GetPhonebookRepository().GetPhonebooks(companyId)
               .ConvertToPhonebookViewModelList();
                response.Success = true;
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
                if (lastUpdateTime != null)
                {
                    response.Phonebooks = unitOfWork.GetPhonebookRepository()
                        .GetPhonebooksNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToPhonebookViewModelList();
                }
                else
                {
                    response.Phonebooks = unitOfWork.GetPhonebookRepository()
                        .GetPhonebooks(companyId)
                        .ConvertToPhonebookViewModelList();
                }
                response.Success = true;
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
                // Backup notes
                List<PhonebookNoteViewModel> PhonebookNotes = re.PhonebookNotes?.ToList();
                re.PhonebookNotes = null;

                // Backup documents
                List<PhonebookDocumentViewModel> PhonebookDocuments = re.PhonebookDocuments?.ToList();
                re.PhonebookDocuments = null;

                // Backup phone
                List<PhonebookPhoneViewModel> PhonebookPhones = re.PhonebookPhones?.ToList();
                re.PhonebookPhones = null;

                Phonebook createdPhonebook = unitOfWork.GetPhonebookRepository().Create(re.ConvertToPhonebook());

                // Update notes
                if (PhonebookNotes != null && PhonebookNotes.Count > 0)
                {
                    // Items for create or update
                    foreach (var PhonebookNote in PhonebookNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhonebookNoteViewModel>())
                    {
                        PhonebookNote.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        PhonebookNote.ItemStatus = ItemStatus.Submited;
                        PhonebookNote createdPhonebookNote = unitOfWork.GetPhonebookNoteRepository()
                            .Create(PhonebookNote.ConvertToPhonebookNote());
                    }

                    foreach (var item in PhonebookNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhonebookNoteViewModel>())
                    {
                        item.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        unitOfWork.GetPhonebookNoteRepository().Create(item.ConvertToPhonebookNote());

                        unitOfWork.GetPhonebookNoteRepository().Delete(item.Identifier);
                    }
                }

                // Update documents
                if (PhonebookDocuments != null && PhonebookDocuments.Count > 0)
                {
                    // Items for create or update
                    foreach (var PhonebookDocument in PhonebookDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhonebookDocumentViewModel>())
                    {
                        PhonebookDocument.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        PhonebookDocument.ItemStatus = ItemStatus.Submited;
                        PhonebookDocument createdPhonebookDocument = unitOfWork.GetPhonebookDocumentRepository()
                            .Create(PhonebookDocument.ConvertToPhonebookDocument());
                    }

                    foreach (var item in PhonebookDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhonebookDocumentViewModel>())
                    {
                        item.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        unitOfWork.GetPhonebookDocumentRepository().Create(item.ConvertToPhonebookDocument());

                        unitOfWork.GetPhonebookDocumentRepository().Delete(item.Identifier);
                    }
                }

                // Update Phones
                if (PhonebookPhones != null && PhonebookPhones.Count > 0)
                {
                    // Items for create or update
                    foreach (var PhonebookPhone in PhonebookPhones
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhonebookPhoneViewModel>())
                    {
                        PhonebookPhone.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        PhonebookPhone.ItemStatus = ItemStatus.Submited;
                        PhonebookPhone createdPhonebookPhone = unitOfWork.GetPhonebookPhoneRepository()
                            .Create(PhonebookPhone.ConvertToPhonebookPhone());
                    }

                    foreach (var item in PhonebookPhones
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhonebookPhoneViewModel>())
                    {
                        item.Phonebook = new PhonebookViewModel() { Id = createdPhonebook.Id };
                        unitOfWork.GetPhonebookPhoneRepository().Create(item.ConvertToPhonebookPhone());

                        unitOfWork.GetPhonebookPhoneRepository().Delete(item.Identifier);
                    }
                }

                unitOfWork.Save();

                response.Phonebook = createdPhonebook.ConvertToPhonebookViewModel();
                response.Success = true;
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
                DomainCore.Common.Phonebooks.Phonebook deletedPhonebook = unitOfWork.GetPhonebookRepository().Delete(identifier);

                unitOfWork.Save();

                response.Phonebook = deletedPhonebook.ConvertToPhonebookViewModel();
                response.Success = true;
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
                response.Phonebooks = new List<PhonebookViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Phonebooks.AddRange(unitOfWork.GetPhonebookRepository()
                        .GetPhonebooksNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhonebookViewModelList() ?? new List<PhonebookViewModel>());
                }
                else
                {
                    response.Phonebooks.AddRange(unitOfWork.GetPhonebookRepository()
                        .GetPhonebooks(request.CompanyId)
                        ?.ConvertToPhonebookViewModelList() ?? new List<PhonebookViewModel>());
                }

                response.Success = true;
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
