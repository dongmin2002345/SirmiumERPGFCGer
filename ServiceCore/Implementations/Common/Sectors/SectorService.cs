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
    public class SectorService : ISectorService
    {
		private IUnitOfWork unitOfWork;

		public SectorService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public SectorListResponse GetSectors(int companyId)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				response.Sectors = unitOfWork.GetSectorRepository().GetSectors(companyId)
					.ConvertToSectorViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Sectors = new List<SectorViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public SectorListResponse GetSectorsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.Sectors = unitOfWork.GetSectorRepository()
						.GetSectorsNewerThen(companyId, (DateTime)lastUpdateTime)
						.ConvertToSectorViewModelList();
				}
				else
				{
					response.Sectors = unitOfWork.GetSectorRepository()
						.GetSectors(companyId)
						.ConvertToSectorViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Sectors = new List<SectorViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public SectorResponse Create(SectorViewModel sector)
		{
			SectorResponse response = new SectorResponse();
			try
			{
				Sector addedSector = unitOfWork.GetSectorRepository().Create(sector.ConvertToSector());
				unitOfWork.Save();

				response.Sector = addedSector.ConvertToSectorViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Sector = new SectorViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public SectorResponse Delete(Guid identifier)
		{
			SectorResponse response = new SectorResponse();
			try
			{
				Sector deletedSector = unitOfWork.GetSectorRepository().Delete(identifier);

				unitOfWork.Save();

				response.Sector = deletedSector.ConvertToSectorViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Sector = new SectorViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public SectorListResponse Sync(SyncSectorRequest request)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				response.Sectors = new List<SectorViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.Sectors.AddRange(unitOfWork.GetSectorRepository()
						.GetSectorsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToSectorViewModelList() ?? new List<SectorViewModel>());
				}
				else
				{
					response.Sectors.AddRange(unitOfWork.GetSectorRepository()
						.GetSectors(request.CompanyId)
						?.ConvertToSectorViewModelList() ?? new List<SectorViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Sectors = new List<SectorViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
