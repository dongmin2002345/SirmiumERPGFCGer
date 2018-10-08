using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.Locations;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Locations
{
    public class MunicipalityService : IMunicipalityService
    {
        IUnitOfWork unitOfWork;

        public MunicipalityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public MunicipalityListResponse GetMunicipalities(int companyId)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response.Municipalities = unitOfWork.GetMunicipalityRepository().GetMunicipalities(companyId)
                .ConvertToMunicipalityViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityListResponse GetMunicipalitiesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Municipalities = unitOfWork.GetMunicipalityRepository()
                        .GetMunicipalitiesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToMunicipalityViewModelList();
                }
                else
                {
                    response.Municipalities = unitOfWork.GetMunicipalityRepository()
                        .GetMunicipalities(companyId)
                        .ConvertToMunicipalityViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityResponse Create(MunicipalityViewModel re)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                Municipality addedMunicipality = unitOfWork.GetMunicipalityRepository().Create(re.ConvertToMunicipality());
                unitOfWork.Save();
                response.Municipality = addedMunicipality.ConvertToMunicipalityViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Municipality = new MunicipalityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityResponse Delete(Guid identifier)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                Municipality deletedMunicipality = unitOfWork.GetMunicipalityRepository().Delete(identifier);

                unitOfWork.Save();

                response.Municipality = deletedMunicipality.ConvertToMunicipalityViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Municipality = new MunicipalityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public MunicipalityListResponse Sync(SyncMunicipalityRequest request)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response.Municipalities = new List<MunicipalityViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Municipalities.AddRange(unitOfWork.GetMunicipalityRepository()
                        .GetMunicipalitiesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToMunicipalityViewModelList() ?? new List<MunicipalityViewModel>());
                }
                else
                {
                    response.Municipalities.AddRange(unitOfWork.GetMunicipalityRepository()
                        .GetMunicipalities(request.CompanyId)
                        ?.ConvertToMunicipalityViewModelList() ?? new List<MunicipalityViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Municipalities = new List<MunicipalityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
