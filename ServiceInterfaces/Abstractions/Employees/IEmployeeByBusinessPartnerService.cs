using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeByBusinessPartnerService
    {
        EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartners(int companyId);
        EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeByBusinessPartnerResponse Create(EmployeeByBusinessPartnerViewModel employeeByBusinessPartner);
        EmployeeByBusinessPartnerResponse Delete(EmployeeByBusinessPartnerViewModel employeeByBusinessPartner);

        EmployeeByBusinessPartnerListResponse Sync(SyncEmployeeByBusinessPartnerRequest request);
    }
}
