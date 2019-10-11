using DataMapper.Mappers.Employees;
using DataMapper.Mappers.PhysicalPersons;
using DomainCore.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
	public class PhysicalPersonService : IPhysicalPersonService
	{
		IUnitOfWork unitOfWork;

		public PhysicalPersonService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public PhysicalPersonListResponse GetPhysicalPersons(int companyId)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
					.GetPhysicalPersons(companyId)
					.ConvertToPhysicalPersonViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonListResponse GetPhysicalPersonsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersonsNewerThen(companyId, (DateTime)lastUpdateTime)
						.ConvertToPhysicalPersonViewModelList();
				}
				else
				{
					response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersons(companyId)
						.ConvertToPhysicalPersonViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonResponse Create(PhysicalPersonViewModel physicalPerson)
		{
            PhysicalPersonResponse response = new PhysicalPersonResponse();
            try
            {
                //Backup items
                List<PhysicalPersonCardViewModel> physicalPersonCards = physicalPerson
                    .PhysicalPersonCards?.ToList() ?? new List<PhysicalPersonCardViewModel>();
                physicalPerson.PhysicalPersonCards = null;

                List<PhysicalPersonDocumentViewModel> physicalPersonDocuments = physicalPerson
                    .PhysicalPersonDocuments?.ToList() ?? new List<PhysicalPersonDocumentViewModel>();
                physicalPerson.PhysicalPersonDocuments = null;

                List<PhysicalPersonItemViewModel> physicalPersonItems = physicalPerson
                    .PhysicalPersonItems?.ToList() ?? new List<PhysicalPersonItemViewModel>();
                physicalPerson.PhysicalPersonItems = null;

                List<PhysicalPersonLicenceViewModel> physicalPersonLicences = physicalPerson
                    .PhysicalPersonLicences?.ToList() ?? new List<PhysicalPersonLicenceViewModel>();
                physicalPerson.PhysicalPersonLicences = null;

                List<PhysicalPersonNoteViewModel> physicalPersonNotes = physicalPerson
                    .PhysicalPersonNotes?.ToList() ?? new List<PhysicalPersonNoteViewModel>();
                physicalPerson.PhysicalPersonNotes = null;

                List<PhysicalPersonProfessionViewModel> physicalPersonProfessions = physicalPerson
                    .PhysicalPersonProfessions?.ToList() ?? new List<PhysicalPersonProfessionViewModel>();
                physicalPerson.PhysicalPersonProfessions = null;

                PhysicalPerson createdOutputInvoice = unitOfWork.GetPhysicalPersonRepository()
                    .Create(physicalPerson.ConvertToPhysicalPerson());

                // Update items
                if (physicalPersonCards != null && physicalPersonCards.Count > 0)
                {
                    foreach (PhysicalPersonCardViewModel item in physicalPersonCards
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonCardViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonCardRepository().Create(item.ConvertToPhysicalPersonCard());
                    }

                    foreach (PhysicalPersonCardViewModel item in physicalPersonCards
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonCardViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonCardRepository().Create(item.ConvertToPhysicalPersonCard());

                        unitOfWork.GetPhysicalPersonCardRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (physicalPersonDocuments != null && physicalPersonDocuments.Count > 0)
                {
                    foreach (PhysicalPersonDocumentViewModel item in physicalPersonDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonDocumentViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonDocumentRepository().Create(item.ConvertToPhysicalPersonDocument());
                    }

                    foreach (PhysicalPersonDocumentViewModel item in physicalPersonDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonDocumentViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonDocumentRepository().Create(item.ConvertToPhysicalPersonDocument());

                        unitOfWork.GetPhysicalPersonDocumentRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (physicalPersonItems != null && physicalPersonItems.Count > 0)
                {
                    foreach (PhysicalPersonItemViewModel item in physicalPersonItems
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonItemViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonItemRepository().Create(item.ConvertToPhysicalPersonItem());
                    }

                    foreach (PhysicalPersonItemViewModel item in physicalPersonItems
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonItemViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonItemRepository().Create(item.ConvertToPhysicalPersonItem());

                        unitOfWork.GetPhysicalPersonItemRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (physicalPersonLicences != null && physicalPersonLicences.Count > 0)
                {
                    foreach (PhysicalPersonLicenceViewModel item in physicalPersonLicences
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonLicenceViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonLicenceRepository().Create(item.ConvertToPhysicalPersonLicence());
                    }

                    foreach (PhysicalPersonLicenceViewModel item in physicalPersonLicences
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonLicenceViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonLicenceRepository().Create(item.ConvertToPhysicalPersonLicence());

                        unitOfWork.GetPhysicalPersonLicenceRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (physicalPersonNotes != null && physicalPersonNotes.Count > 0)
                {
                    foreach (PhysicalPersonNoteViewModel item in physicalPersonNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonNoteViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonNoteRepository().Create(item.ConvertToPhysicalPersonNote());
                    }

                    foreach (PhysicalPersonNoteViewModel item in physicalPersonNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonNoteViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonNoteRepository().Create(item.ConvertToPhysicalPersonNote());

                        unitOfWork.GetPhysicalPersonNoteRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (physicalPersonProfessions != null && physicalPersonProfessions.Count > 0)
                {
                    foreach (PhysicalPersonProfessionViewModel item in physicalPersonProfessions
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<PhysicalPersonProfessionViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetPhysicalPersonProfessionRepository().Create(item.ConvertToPhysicalPersonProfession());
                    }

                    foreach (PhysicalPersonProfessionViewModel item in physicalPersonProfessions
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<PhysicalPersonProfessionViewModel>())
                    {
                        item.PhysicalPerson = new PhysicalPersonViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetPhysicalPersonProfessionRepository().Create(item.ConvertToPhysicalPersonProfession());

                        unitOfWork.GetPhysicalPersonProfessionRepository().Delete(item.Identifier);
                    }

                }

                unitOfWork.Save();

                response.PhysicalPerson = createdOutputInvoice.ConvertToPhysicalPersonViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPerson = new PhysicalPersonViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public PhysicalPersonResponse Delete(Guid identifier)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			try
			{
				response.PhysicalPerson = unitOfWork.GetPhysicalPersonRepository().Delete(identifier)?.ConvertToPhysicalPersonViewModel();
				unitOfWork.Save();

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPerson = new PhysicalPersonViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonListResponse Sync(SyncPhysicalPersonRequest request)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.PhysicalPersons.AddRange(unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersonsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToPhysicalPersonViewModelList() ?? new List<PhysicalPersonViewModel>());
				}
				else
				{
					response.PhysicalPersons.AddRange(unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersons(request.CompanyId)
						?.ConvertToPhysicalPersonViewModelList() ?? new List<PhysicalPersonViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
