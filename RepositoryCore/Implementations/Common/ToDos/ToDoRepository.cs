using DomainCore.Common.ToDos;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.ToDos
{
    public class ToDoRepository : IToDoRepository
    {
        private ApplicationDbContext context;

        public ToDoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        #region GET methods

        public List<ToDo> GetToDos(int companyId)
        {
            List<ToDo> toDos = context.ToDos
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return toDos;
        }

        public List<ToDo> GetToDosNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ToDo> ToDos = context.ToDos
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return ToDos;
        }

        #endregion

        #region CREATE methods

        public ToDo Create(ToDo toDo)
        {
            if (context.ToDos.Where(x => x.Identifier != null && x.Identifier == toDo.Identifier).Count() == 0)
            {
                toDo.Id = 0;

                toDo.Active = true;

                toDo.UpdatedAt = DateTime.Now;
                toDo.CreatedAt = DateTime.Now;

                context.ToDos.Add(toDo);
                return toDo;
            }
            else
            {
                // Load remedy that will be updated
                ToDo dbEntry = context.ToDos
                .FirstOrDefault(x => x.Identifier == toDo.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = toDo.CompanyId ?? null;
                    dbEntry.CreatedById = toDo.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = toDo.Name;
                    dbEntry.Description = toDo.Description;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        #endregion

        #region DELETE methods

        public ToDo Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            ToDo dbEntry = context.ToDos
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }

        #endregion
    }
}
