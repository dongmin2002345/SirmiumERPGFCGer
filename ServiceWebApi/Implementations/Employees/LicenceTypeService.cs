using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Employees
{
	public class LicenceTypeService : ILicenceTypeService
	{
		public LicenceTypeListResponse GetLicenceTypes(int companyId)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<LicenceTypeViewModel>, LicenceTypeListResponse>("GetLicenceTypes", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeListResponse GetLicenceTypesNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<LicenceTypeViewModel>, LicenceTypeListResponse>("GetLicenceTypesNewerThen", new Dictionary<string, string>()
				{
					{ "CompanyId", companyId.ToString() },
					{ "LastUpdateTime", lastUpdateTime.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeResponse Create(LicenceTypeViewModel licenceType)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				response = WpfApiHandler.SendToApi<LicenceTypeViewModel, LicenceTypeResponse>(licenceType, "Create");
			}
			catch (Exception ex)
			{
				response.LicenceType = new LicenceTypeViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeResponse Delete(Guid identifier)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				LicenceTypeViewModel re = new LicenceTypeViewModel();
				re.Identifier = identifier;
				response = WpfApiHandler.SendToApi<LicenceTypeViewModel, LicenceTypeResponse>(re, "Delete");
			}
			catch (Exception ex)
			{
				response.LicenceType = new LicenceTypeViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeListResponse Sync(SyncLicenceTypeRequest request)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncLicenceTypeRequest, LicenceTypeViewModel, LicenceTypeListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
