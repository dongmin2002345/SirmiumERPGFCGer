using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
	public interface IPhysicalPersonService
	{
		PhysicalPersonListResponse GetPhysicalPersons(int companyId);
		PhysicalPersonListResponse GetPhysicalPersonsNewerThen(int companyId, DateTime? lastUpdateTime);

		PhysicalPersonResponse Create(PhysicalPersonViewModel PhysicalPerson);
		PhysicalPersonResponse Delete(Guid identifier);

		PhysicalPersonListResponse Sync(SyncPhysicalPersonRequest request);
	}
}
