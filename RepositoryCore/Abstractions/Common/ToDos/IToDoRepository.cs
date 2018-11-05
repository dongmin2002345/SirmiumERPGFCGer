using DomainCore.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.ToDos
{
    public interface IToDoRepository
    {
        List<ToDo> GetToDos(int companyId);
        List<ToDo> GetToDosNewerThen(int companyId, DateTime lastUpdateTime);

        ToDo Create(ToDo toDo);
        ToDo Delete(Guid identifier);
    }
}
