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
	public class PhysicalPersonService : IPhysicalPersonService
	{
		public PhysicalPersonListResponse GetPhysicalPersons(int companyId)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<PhysicalPersonViewModel, PhysicalPersonListResponse>("GetPhysicalPersons",
					new Dictionary<string, string>() { { "CompanyID", companyId.ToString() }
				});
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonListResponse GetPhysicalPersonsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response = WpfApiHandler.GetFromApi<List<PhysicalPersonViewModel>, PhysicalPersonListResponse>("GetPhysicalPersonsNewerThen",
				   new Dictionary<string, string>() {
					   { "CompanyId", companyId.ToString() },
					   { "LastUpdateTime", lastUpdateTime.ToString() }
				   });
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonResponse Create(PhysicalPersonViewModel PhysicalPerson)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			try
			{
				response = WpfApiHandler.SendToApi<PhysicalPersonViewModel, PhysicalPersonResponse>(PhysicalPerson, "Create");
			}
			catch (Exception ex)
			{
				response.PhysicalPerson = new PhysicalPersonViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;

		}

		public PhysicalPersonResponse Delete(Guid identifier)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			try
			{
				response = WpfApiHandler.SendToApi<Guid, PhysicalPersonViewModel, PhysicalPersonResponse>(identifier, "Delete");
			}
			catch (Exception ex)
			{
				response.PhysicalPerson = new PhysicalPersonViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;

		}

		public PhysicalPersonListResponse Sync(SyncPhysicalPersonRequest request)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncPhysicalPersonRequest, PhysicalPersonViewModel, PhysicalPersonListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
