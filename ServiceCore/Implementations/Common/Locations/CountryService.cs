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
    public class CountryService : ICountryService
    {
        private IUnitOfWork unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public CountryListResponse GetCountries(int companyId)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                response.Countries = unitOfWork.GetCountryRepository().GetCountries(companyId)
               .ConvertToCountryViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryListResponse GetCountriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Countries = unitOfWork.GetCountryRepository()
                        .GetCountriesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToCountryViewModelList();
                }
                else
                {
                    response.Countries = unitOfWork.GetCountryRepository()
                        .GetCountries(companyId)
                        .ConvertToCountryViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryResponse Create(CountryViewModel re)
        {
            CountryResponse response = new CountryResponse();
            try
            {
                Country addedCountry = unitOfWork.GetCountryRepository().Create(re.ConvertToCountry());
                unitOfWork.Save();
                response.Country = addedCountry.ConvertToCountryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Country = new CountryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryResponse Delete(Guid identifier)
        {
            CountryResponse response = new CountryResponse();
            try
            {
                Country deletedCountry = unitOfWork.GetCountryRepository().Delete(identifier);

                unitOfWork.Save();

                response.Country = deletedCountry.ConvertToCountryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Country = new CountryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public CountryListResponse Sync(SyncCountryRequest request)
        {
            CountryListResponse response = new CountryListResponse();
            try
            {
                response.Countries = new List<CountryViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Countries.AddRange(unitOfWork.GetCountryRepository()
                        .GetCountriesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToCountryViewModelList() ?? new List<CountryViewModel>());
                }
                else
                {
                    response.Countries.AddRange(unitOfWork.GetCountryRepository()
                        .GetCountries(request.CompanyId)
                        ?.ConvertToCountryViewModelList() ?? new List<CountryViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Countries = new List<CountryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
