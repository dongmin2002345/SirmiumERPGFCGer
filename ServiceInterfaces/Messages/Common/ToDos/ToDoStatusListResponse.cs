using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.ToDos
{
    public class ToDoStatusListResponse :BaseResponse
    {
        public List<ToDoStatusViewModel> ToDoStatuses { get; set; }

        public int TotalItems { get; set; }
    }
}
