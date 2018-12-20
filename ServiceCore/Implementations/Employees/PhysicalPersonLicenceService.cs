using DataMapper.Mappers.PhysicalPersons;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class PhysicalPersonLicenceService : IPhysicalPersonLicenceService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonLicenceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonLicenceListResponse GetPhysicalPersonItems(int companyId)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                response.PhysicalPersonLicences = unitOfWork.GetPhysicalPersonLicenceRepository()
                    .GetPhysicalPersonItems(companyId)
                    .ConvertToPhysicalPersonLicenceViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonLicenceListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.PhysicalPersonLicences = unitOfWork.GetPhysicalPersonLicenceRepository()
                        .GetPhysicalPersonItemsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToPhysicalPersonLicenceViewModelList();
                }
                else
                {
                    response.PhysicalPersonLicences = unitOfWork.GetPhysicalPersonLicenceRepository()
                        .GetPhysicalPersonItems(companyId)
                        .ConvertToPhysicalPersonLicenceViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonLicenceResponse Create(PhysicalPersonLicenceViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();
            try
            {
                var addedPhysicalPersonItem = unitOfWork.GetPhysicalPersonLicenceRepository().Create(PhysicalPersonItemViewModel.ConvertToPhysicalPersonLicence());
                unitOfWork.Save();
                response.PhysicalPersonLicence = addedPhysicalPersonItem.ConvertToPhysicalPersonLicenceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicence = new PhysicalPersonLicenceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonLicenceListResponse Sync(SyncPhysicalPersonLicenceRequest request)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            try
            {
                response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonLicences.AddRange(unitOfWork.GetPhysicalPersonLicenceRepository()
                        .GetPhysicalPersonItemsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonLicenceViewModelList() ?? new List<PhysicalPersonLicenceViewModel>());
                }
                else
                {
                    response.PhysicalPersonLicences.AddRange(unitOfWork.GetPhysicalPersonLicenceRepository()
                        .GetPhysicalPersonItems(request.CompanyId)
                        ?.ConvertToPhysicalPersonLicenceViewModelList() ?? new List<PhysicalPersonLicenceViewModel>());
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
