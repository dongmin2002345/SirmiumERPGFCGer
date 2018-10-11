using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Employees
{
    public class EmployeeListResponse : BaseResponse
    {
        public List<EmployeeViewModel> Employees { get; set; }
        public int TotalItems { get; set; }
    }
}
