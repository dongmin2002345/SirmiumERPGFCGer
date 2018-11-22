using DataMapper.Mappers.Common.TaxAdministrations;
using DomainCore.Common.TaxAdministrations;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.TaxAdministrations
{
    public class TaxAdministrationService : ITaxAdministrationService
    {
        IUnitOfWork unitOfWork;

        public TaxAdministrationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public TaxAdministrationListResponse GetTaxAdministrations(int companyId)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response.TaxAdministrations = unitOfWork.GetTaxAdministrationRepository().GetTaxAdministrations(companyId)
               .ConvertToTaxAdministrationViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationListResponse GetTaxAdministrationsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.TaxAdministrations = unitOfWork.GetTaxAdministrationRepository()
                        .GetTaxAdministrationsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToTaxAdministrationViewModelList();
                }
                else
                {
                    response.TaxAdministrations = unitOfWork.GetTaxAdministrationRepository()
                        .GetTaxAdministrations(companyId)
                        .ConvertToTaxAdministrationViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationResponse Create(TaxAdministrationViewModel re)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                TaxAdministration addedTaxAdministration = unitOfWork.GetTaxAdministrationRepository().Create(re.ConvertToTaxAdministration());
                unitOfWork.Save();
                response.TaxAdministration = addedTaxAdministration.ConvertToTaxAdministrationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.TaxAdministration = new TaxAdministrationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationResponse Delete(Guid identifier)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                TaxAdministration deletedTaxAdministration = unitOfWork.GetTaxAdministrationRepository().Delete(identifier);

                unitOfWork.Save();

                response.TaxAdministration = deletedTaxAdministration.ConvertToTaxAdministrationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.TaxAdministration = new TaxAdministrationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public TaxAdministrationListResponse Sync(SyncTaxAdministrationRequest request)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.TaxAdministrations.AddRange(unitOfWork.GetTaxAdministrationRepository()
                        .GetTaxAdministrationsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToTaxAdministrationViewModelList() ?? new List<TaxAdministrationViewModel>());
                }
                else
                {
                    response.TaxAdministrations.AddRange(unitOfWork.GetTaxAdministrationRepository()
                        .GetTaxAdministrations(request.CompanyId)
                        ?.ConvertToTaxAdministrationViewModelList() ?? new List<TaxAdministrationViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
