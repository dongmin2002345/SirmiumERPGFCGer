using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
   public interface ILicenceTypeRepository
    {
		List<LicenceType> GetLicenceTypes(int companyId);
		List<LicenceType> GetLicenceTypesNewerThen(int companyId, DateTime lastUpdateTime);

		LicenceType Create(LicenceType licenceType);
		LicenceType Delete(Guid identifier);
	}
}
