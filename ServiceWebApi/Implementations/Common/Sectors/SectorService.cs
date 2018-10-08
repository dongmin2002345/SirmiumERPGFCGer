using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Sectors
{
	public class SectorService : ISectorService
	{
		public SectorListResponse GetSectors(int companyId)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<SectorViewModel>, SectorListResponse>("GetSectors", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() }
				});
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
				response = WpfApiHandler.GetFromApi<List<SectorViewModel>, SectorListResponse>("GetSectorsNewerThen", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() },
					{ "LastUpdateTime", lastUpdateTime.ToString() }
				});
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
				response = WpfApiHandler.SendToApi<SectorViewModel, SectorResponse>(sector, "Create");
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
				SectorViewModel re = new SectorViewModel();
				re.Identifier = identifier;
				response = WpfApiHandler.SendToApi<SectorViewModel, SectorResponse>(re, "Delete");
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
				response = WpfApiHandler.SendToApi<SyncSectorRequest, SectorViewModel, SectorListResponse>(request, "Sync");
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
