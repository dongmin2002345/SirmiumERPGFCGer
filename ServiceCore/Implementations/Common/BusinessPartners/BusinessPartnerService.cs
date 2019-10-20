using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerListResponse GetBusinessPartners(int companyId)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                List<BusinessPartner> businessPartners = unitOfWork.GetBusinessPartnerRepository().GetBusinessPartners(companyId);
                response.BusinessPartners = businessPartners.ConvertToBusinessPartnerViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartnersNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerViewModelList();
                }
                else
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners(companyId)
                        .ConvertToBusinessPartnerViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerResponse Create(BusinessPartnerViewModel re)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                // Backup notes
                List<BusinessPartnerNoteViewModel> businessPartnerNotes = re.BusinessPartnerNotes?.ToList();
                re.BusinessPartnerNotes = null;

                // Backup documents
                List<BusinessPartnerDocumentViewModel> businessPartnerDocuments = re.BusinessPartnerDocuments?.ToList();
                re.BusinessPartnerDocuments = null;

                //Phone
                List<BusinessPartnerPhoneViewModel> businessPartnerPhones = re.Phones?.ToList();
                re.Phones = null;

                //Location
                List<BusinessPartnerLocationViewModel> businessPartnerLocations = re.Locations?.ToList();
                re.Locations = null;

                //Institution
                List<BusinessPartnerInstitutionViewModel> businessPartnerInstitutions = re.Institutions?.ToList();
                re.Institutions = null;

                //Bank
                List<BusinessPartnerBankViewModel> businessPartnerBanks = re.Banks?.ToList();
                re.Banks = null;

                //Type
                List<BusinessPartnerTypeViewModel> businessPartnerTypes = re.BusinessPartnerTypes?.ToList();
                re.BusinessPartnerTypes = null;

                BusinessPartner createdBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Create(re.ConvertToBusinessPartner());

                // Update notes
                if (businessPartnerNotes != null && businessPartnerNotes.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerNote in businessPartnerNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerNoteViewModel>())
                    {
                        businessPartnerNote.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerNote.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerNote createdBusinessPartnerNote = unitOfWork.GetBusinessPartnerNoteRepository()
                            .Create(businessPartnerNote.ConvertToBusinessPartnerNote());
                    }

                    foreach (var item in businessPartnerNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerNoteViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerNoteRepository().Create(item.ConvertToBusinessPartnerNote());

                        unitOfWork.GetBusinessPartnerNoteRepository().Delete(item.Identifier);
                    }
                }

                // Update documents
                if (businessPartnerDocuments != null && businessPartnerDocuments.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerDocument in businessPartnerDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerDocumentViewModel>())
                    {
                        businessPartnerDocument.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerDocument.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerDocument createdBusinessPartnerDocument = unitOfWork.GetBusinessPartnerDocumentRepository()
                            .Create(businessPartnerDocument.ConvertToBusinessPartnerDocument());
                    }

                    foreach (var item in businessPartnerDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerDocumentViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerDocumentRepository().Create(item.ConvertToBusinessPartnerDocument());

                        unitOfWork.GetBusinessPartnerDocumentRepository().Delete(item.Identifier);
                    }
                }

                // Update Phone
                if (businessPartnerPhones != null && businessPartnerPhones.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerPhone in businessPartnerPhones
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerPhoneViewModel>())
                    {
                        businessPartnerPhone.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerPhone.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerPhone createdBusinessPartnerPhone = unitOfWork.GetBusinessPartnerPhoneRepository()
                            .Create(businessPartnerPhone.ConvertToBusinessPartnerPhone());
                    }

                    foreach (var item in businessPartnerPhones
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerPhoneViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerPhoneRepository().Create(item.ConvertToBusinessPartnerPhone());

                        unitOfWork.GetBusinessPartnerPhoneRepository().Delete(item.Identifier);
                    }
                }

                // Update Location
                if (businessPartnerLocations != null && businessPartnerLocations.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerLocation in businessPartnerLocations
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerLocationViewModel>())
                    {
                        businessPartnerLocation.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerLocation.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerLocation createdBusinessPartnerLocation = unitOfWork.GetBusinessPartnerLocationRepository()
                            .Create(businessPartnerLocation.ConvertToBusinessPartnerLocation());
                    }

                    foreach (var item in businessPartnerLocations
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerLocationViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerLocationRepository().Create(item.ConvertToBusinessPartnerLocation());

                        unitOfWork.GetBusinessPartnerLocationRepository().Delete(item.Identifier);
                    }
                }

                // Update Institution
                if (businessPartnerInstitutions != null && businessPartnerInstitutions.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerInstitution in businessPartnerInstitutions
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerInstitutionViewModel>())
                    {
                        businessPartnerInstitution.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerInstitution.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerInstitution createdBusinessPartnerInstitution = unitOfWork.GetBusinessPartnerInstitutionRepository()
                            .Create(businessPartnerInstitution.ConvertToBusinessPartnerInstitution());
                    }

                    foreach (var item in businessPartnerInstitutions
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerInstitutionViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerInstitutionRepository().Create(item.ConvertToBusinessPartnerInstitution());

                        unitOfWork.GetBusinessPartnerInstitutionRepository().Delete(item.Identifier);
                    }
                }

                // Update Bank
                if (businessPartnerBanks != null && businessPartnerBanks.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerBank in businessPartnerBanks
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerBankViewModel>())
                    {
                        businessPartnerBank.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerBank.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerBank createdBusinessPartnerBank = unitOfWork.GetBusinessPartnerBankRepository()
                            .Create(businessPartnerBank.ConvertToBusinessPartnerBank());
                    }

                    foreach (var item in businessPartnerBanks
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerBankViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerBankRepository().Create(item.ConvertToBusinessPartnerBank());

                        unitOfWork.GetBusinessPartnerBankRepository().Delete(item.Identifier);
                    }
                }

                // Update Type
                //unitOfWork.GetBusinessPartnerBusinessPartnerTypeRepository().Delete(createdBusinessPartner.Id);
                //foreach (var item in businessPartnerTypes)
                //{
                //    unitOfWork.GetBusinessPartnerBusinessPartnerTypeRepository().Create(createdBusinessPartner.Id, item.Id);
                //}
                if (businessPartnerTypes != null && businessPartnerTypes.Count > 0)
                {
                    // Items for create or update
                    foreach (var businessPartnerType in businessPartnerTypes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<BusinessPartnerTypeViewModel>())
                    {
                        businessPartnerType.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        businessPartnerType.ItemStatus = ItemStatus.Submited;
                        BusinessPartnerType createdBusinessPartnerType = unitOfWork.GetBusinessPartnerTypeRepository()
                            .Create(businessPartnerType.ConvertToBusinessPartnerType());
                    }

                    foreach (var item in businessPartnerTypes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<BusinessPartnerTypeViewModel>())
                    {
                        item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                        unitOfWork.GetBusinessPartnerTypeRepository().Create(item.ConvertToBusinessPartnerType());

                        unitOfWork.GetBusinessPartnerTypeRepository().Delete(item.Identifier);
                    }
                }

                unitOfWork.Save();

                response.BusinessPartner = createdBusinessPartner.ConvertToBusinessPartnerViewModel();
                response.Success = true;
                
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerResponse Delete(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                BusinessPartner deletedBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Delete(identifier);

                unitOfWork.Save();

                response.BusinessPartner = deletedBusinessPartner.ConvertToBusinessPartnerViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse Sync(SyncBusinessPartnerRequest request)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartners.AddRange(unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartnersNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
                }
                else
                {
                    response.BusinessPartners.AddRange(unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners(request.CompanyId)
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
