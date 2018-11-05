using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.ToDos
{
    public class ToDoListResponse : BaseResponse
    {
        public List<ToDoViewModel> ToDos { get; set; }
    }
}
