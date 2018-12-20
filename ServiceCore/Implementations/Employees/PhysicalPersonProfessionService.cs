using DataMapper.Mappers.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class PhysicalPersonProfessionService : IPhysicalPersonProfessionService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonProfessionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonProfessionListResponse GetPhysicalPersonItems(int companyId)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response.PhysicalPersonProfessions = unitOfWork.GetPhysicalPersonProfessionRepository()
                    .GetPhysicalPersonItems(companyId)
                    .ConvertToPhysicalPersonProfessionViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionListResponse GetPhysicalPersonItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.PhysicalPersonProfessions = unitOfWork.GetPhysicalPersonProfessionRepository()
                        .GetPhysicalPersonItemsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToPhysicalPersonProfessionViewModelList();
                }
                else
                {
                    response.PhysicalPersonProfessions = unitOfWork.GetPhysicalPersonProfessionRepository()
                        .GetPhysicalPersonItems(companyId)
                        .ConvertToPhysicalPersonProfessionViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionResponse Create(PhysicalPersonProfessionViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();
            try
            {
                var addedPhysicalPersonItem = unitOfWork.GetPhysicalPersonProfessionRepository().Create(PhysicalPersonItemViewModel.ConvertToPhysicalPersonProfession());
                unitOfWork.Save();
                response.PhysicalPersonProfession = addedPhysicalPersonItem.ConvertToPhysicalPersonProfessionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfession = new PhysicalPersonProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public PhysicalPersonProfessionListResponse Sync(SyncPhysicalPersonProfessionRequest request)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonProfessions.AddRange(unitOfWork.GetPhysicalPersonProfessionRepository()
                        .GetPhysicalPersonItemsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonProfessionViewModelList() ?? new List<PhysicalPersonProfessionViewModel>());
                }
                else
                {
                    response.PhysicalPersonProfessions.AddRange(unitOfWork.GetPhysicalPersonProfessionRepository()
                        .GetPhysicalPersonItems(request.CompanyId)
                        ?.ConvertToPhysicalPersonProfessionViewModelList() ?? new List<PhysicalPersonProfessionViewModel>());
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
