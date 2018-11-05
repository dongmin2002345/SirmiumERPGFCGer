using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.ToDos
{
    public interface IToDoService
    {
        ToDoListResponse GetToDos(int companyId);
        ToDoListResponse GetToDosNewerThen(int companyId, DateTime? lastUpdateTime);

        ToDoResponse Create(ToDoViewModel toDo);
        ToDoResponse Delete(Guid identifier);

        ToDoListResponse Sync(SyncToDoRequest request);
    }
}
