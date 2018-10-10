using DataMapper.Mappers.Common.Sectors;
using DomainCore.Common.Sectors;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Sectors
{
    public class AgencyService : IAgencyService
    {
        private IUnitOfWork unitOfWork;

        public AgencyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public AgencyListResponse GetAgencies(int companyId)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response.Agencies = unitOfWork.GetAgencyRepository().GetAgencies(companyId)
                    .ConvertToAgencyViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyListResponse GetAgenciesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Agencies = unitOfWork.GetAgencyRepository()
                        .GetAgenciesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToAgencyViewModelList();
                }
                else
                {
                    response.Agencies = unitOfWork.GetAgencyRepository()
                        .GetAgencies(companyId)
                        .ConvertToAgencyViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public AgencyResponse Create(AgencyViewModel Agency)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                Agency addedAgency = unitOfWork.GetAgencyRepository().Create(Agency.ConvertToAgency());
                unitOfWork.Save();

                response.Agency = addedAgency.ConvertToAgencyViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Agency = new AgencyViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public AgencyResponse Delete(Guid identifier)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                Agency deletedAgency = unitOfWork.GetAgencyRepository().Delete(identifier);

                unitOfWork.Save();

                response.Agency = deletedAgency.ConvertToAgencyViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Agency = new AgencyViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public AgencyListResponse Sync(SyncAgencyRequest request)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response.Agencies = new List<AgencyViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Agencies.AddRange(unitOfWork.GetAgencyRepository()
                        .GetAgenciesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToAgencyViewModelList() ?? new List<AgencyViewModel>());
                }
                else
                {
                    response.Agencies.AddRange(unitOfWork.GetAgencyRepository()
                        .GetAgencies(request.CompanyId)
                        ?.ConvertToAgencyViewModelList() ?? new List<AgencyViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Agencies = new List<AgencyViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
