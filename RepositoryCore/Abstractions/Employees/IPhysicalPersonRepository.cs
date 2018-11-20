using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
	public interface IPhysicalPersonRepository
	{
		List<PhysicalPerson> GetPhysicalPersons(int companyId);
		PhysicalPerson GetPhysicalPerson(int physicalPersonId);
		List<PhysicalPerson> GetPhysicalPersonsNewerThen(int companyId, DateTime lastUpdateTime);

		PhysicalPerson Create(PhysicalPerson PhysicalPerson);
		PhysicalPerson Delete(Guid identifier);
	}
}
