using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.ToDos
{
    public interface IToDoStatusService
    {
        ToDoStatusListResponse GetToDoStatuses(int companyId);

        ToDoStatusResponse Create(ToDoStatusViewModel toDoStatus);
        ToDoStatusResponse Delete(Guid identifier);

        ToDoStatusListResponse Sync(SyncToDoStatusRequest request);
        
    }
}
