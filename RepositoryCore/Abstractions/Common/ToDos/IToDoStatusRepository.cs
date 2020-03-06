using DomainCore.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.ToDos
{
    public interface IToDoStatusRepository
    {
        List<ToDoStatus> GetToDoStatuses(int companyId);
        List<ToDoStatus> GetToDoStatusesNewerThen(int companyId, DateTime lastUpdateTime);
        ToDoStatus GetToDoStatus(int toDoStatusId);

        ToDoStatus Create(ToDoStatus toDoStatus);
        ToDoStatus Delete(Guid identifier);
    }
}
