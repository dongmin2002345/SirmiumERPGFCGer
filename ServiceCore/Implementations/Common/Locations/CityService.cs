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
    public class CityService : ICityService
    {
        private IUnitOfWork unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public CityListResponse GetCities(int companyId)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response.Cities = unitOfWork.GetCityRepository().GetCities(companyId)
                    .ConvertToCityViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Cities = new List<CityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CityListResponse GetCitiesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Cities = unitOfWork.GetCityRepository()
                        .GetCitiesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToCityViewModelList();
                }
                else
                {
                    response.Cities = unitOfWork.GetCityRepository()
                        .GetCities(companyId)
                        .ConvertToCityViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Cities = new List<CityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public CityResponse Create(CityViewModel city)
        {
            CityResponse response = new CityResponse();
            try
            {
                City addedCity = unitOfWork.GetCityRepository().Create(city.ConvertToCity());
                unitOfWork.Save();

                response.City = addedCity.ConvertToCityViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.City = new CityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public CityResponse Delete(Guid identifier)
        {
            CityResponse response = new CityResponse();
            try
            {
                City deletedCity = unitOfWork.GetCityRepository().Delete(identifier);

                unitOfWork.Save();

                response.City = deletedCity.ConvertToCityViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.City = new CityViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CityListResponse Sync(SyncCityRequest request)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response.Cities = new List<CityViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Cities.AddRange(unitOfWork.GetCityRepository()
                        .GetCitiesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToCityViewModelList() ?? new List<CityViewModel>());
                }
                else
                {
                    response.Cities.AddRange(unitOfWork.GetCityRepository()
                        .GetCities(request.CompanyId)
                        ?.ConvertToCityViewModelList() ?? new List<CityViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Cities = new List<CityViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

