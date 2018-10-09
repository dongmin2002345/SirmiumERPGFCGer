﻿using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
	public interface ILicenceTypeService
	{
		LicenceTypeListResponse GetLicenceTypes(int companyId);
		LicenceTypeListResponse GetLicenceTypesNewerThen(int companyId, DateTime? lastUpdateTime);

		LicenceTypeResponse Create(LicenceTypeViewModel LicenceType);
		LicenceTypeResponse Delete(Guid identifier);

		LicenceTypeListResponse Sync(SyncLicenceTypeRequest request);
	}
}
