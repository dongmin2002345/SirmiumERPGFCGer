using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Employees
{
    public class EmployeeDocumentListResponse : BaseResponse
    {
        public List<EmployeeDocumentViewModel> EmployeeDocuments { get; set; }
        public int TotalItems { get; set; }
    }
}
