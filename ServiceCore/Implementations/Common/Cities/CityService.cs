using DataMapper.Mappers.Common.Cities;
using DomainCore.Common.Cities;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Cities;
using ServiceInterfaces.Messages.Common.Cities;
using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Cities
{
    public class CityService : ICityService
    {
        IUnitOfWork unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public CityListResponse GetCities()
        {
            CityListResponse response = new CityListResponse();
            try
            {
                List<City> Cities = unitOfWork.GetCityRepository().GetCities();
                response.Cities = Cities.ConvertToCityViewModelList();
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

        public CityListResponse GetCitiesNewerThen(DateTime? lastUpdateTime)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Cities = unitOfWork.GetCityRepository()
                        .GetCitiesNewerThen((DateTime)lastUpdateTime)
                        .ConvertToCityViewModelList();
                }
                else
                {
                    response.Cities = unitOfWork.GetCityRepository()
                        .GetCities()
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

        public CityResponse Create(CityViewModel CityViewModel)
        {
            CityResponse response = new CityResponse();
            try
            {
                City createdCity = unitOfWork.GetCityRepository()
                    .Create(CityViewModel.ConvertToCity());
                unitOfWork.Save();

                response.City = createdCity.ConvertToCityViewModel();
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
                        .GetCitiesNewerThen((DateTime)request.LastUpdatedAt)
                        ?.ConvertToCityViewModelList() ?? new List<CityViewModel>());
                }
                else
                {
                    response.Cities.AddRange(unitOfWork.GetCityRepository()
                        .GetCities()
                        ?.ConvertToCityViewModelList() ?? new List<CityViewModel>());
                }

                List<City> addedCities = new List<City>();
                foreach (var City in request.UnSyncedCities)
                {
                    addedCities.Add(unitOfWork.GetCityRepository().Create(City.ConvertToCity()));
                }

                unitOfWork.Save();

                foreach (var item in addedCities)
                {
                    response.Cities.Add(unitOfWork.GetCityRepository()
                        .GetCity(item.Id)
                        .ConvertToCityViewModel());
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

