using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IEmployeeByBusinessPartnerHistoryService
    {
        EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistories(int companyId);
        EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime? lastUpdateTime);

        EmployeeByBusinessPartnerHistoryResponse Create(EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistory);
        EmployeeByBusinessPartnerHistoryResponse Delete(Guid identifier);

        EmployeeByBusinessPartnerHistoryListResponse Sync(SyncEmployeeByBusinessPartnerHistoryRequest request);
    }
}
