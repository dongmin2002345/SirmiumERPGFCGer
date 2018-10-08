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
    public class RegionService : IRegionService
    {
        IUnitOfWork unitOfWork;

        public RegionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public RegionListResponse GetRegions(int companyId)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response.Regions = unitOfWork.GetRegionRepository().GetRegions(companyId)
               .ConvertToRegionViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionListResponse GetRegionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Regions = unitOfWork.GetRegionRepository()
                        .GetRegionsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToRegionViewModelList();
                }
                else
                {
                    response.Regions = unitOfWork.GetRegionRepository()
                        .GetRegions(companyId)
                        .ConvertToRegionViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionResponse Create(RegionViewModel re)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                Region addedRegion = unitOfWork.GetRegionRepository().Create(re.ConvertToRegion());
                unitOfWork.Save();
                response.Region = addedRegion.ConvertToRegionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Region = new RegionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionResponse Delete(Guid identifier)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                Region deletedRegion = unitOfWork.GetRegionRepository().Delete(identifier);

                unitOfWork.Save();

                response.Region = deletedRegion.ConvertToRegionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Region = new RegionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public RegionListResponse Sync(SyncRegionRequest request)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response.Regions = new List<RegionViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Regions.AddRange(unitOfWork.GetRegionRepository()
                        .GetRegionsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToRegionViewModelList() ?? new List<RegionViewModel>());
                }
                else
                {
                    response.Regions.AddRange(unitOfWork.GetRegionRepository()
                        .GetRegions(request.CompanyId)
                        ?.ConvertToRegionViewModelList() ?? new List<RegionViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Regions = new List<RegionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
