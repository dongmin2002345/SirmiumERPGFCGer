using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IEmployeeByBusinessPartnerRepository
    {
        List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartners(int companyId);
        List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime lastUpdateTime);

        EmployeeByBusinessPartner Create(EmployeeByBusinessPartner employeeByBusinessPartner);
        EmployeeByBusinessPartner Delete(EmployeeByBusinessPartner employeeByBusinessPartner);
    }
}
