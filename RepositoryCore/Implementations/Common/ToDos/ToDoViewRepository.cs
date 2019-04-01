using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.ToDos;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.ToDos
{
    public class ToDoViewRepository : IToDoRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public ToDoViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        #region GET methods

        public List<ToDo> GetToDos(int companyId)
        {
            List<ToDo> ToDos = new List<ToDo>();

            string queryString =
                "SELECT ToDoId, ToDoIdentifier, ToDoName, ToDoDescription, Path, ToDoDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vToDos " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ToDo toDo;
                    while (reader.Read())
                    {
                        toDo = new ToDo();
                        toDo.Id = Int32.Parse(reader["ToDoId"].ToString());
                        toDo.Identifier = Guid.Parse(reader["ToDoIdentifier"].ToString());
                        toDo.Name = reader["ToDoName"].ToString();

                        if (reader["ToDoDescription"] != DBNull.Value)
                            toDo.Description = reader["ToDoDescription"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            toDo.Path = reader["Path"].ToString();
                        if (reader["ToDoDate"] != DBNull.Value)
                            toDo.ToDoDate = DateTime.Parse(reader["ToDoDate"].ToString());

                        toDo.Active = bool.Parse(reader["Active"].ToString());
                        toDo.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            toDo.CreatedBy = new User();
                            toDo.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            toDo.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            toDo.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            toDo.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            toDo.Company = new Company();
                            toDo.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            toDo.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            toDo.Company.Name = reader["CompanyName"].ToString();
                        }

                        ToDos.Add(toDo);
                    }
                }
            }
            return ToDos;
        }

        public List<ToDo> GetToDosNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ToDo> ToDos = new List<ToDo>();

            string queryString =
                "SELECT ToDoId, ToDoIdentifier, ToDoName, ToDoDescription, Path, ToDoDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vToDos " +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ToDo toDo;
                    while (reader.Read())
                    {
                        toDo = new ToDo();
                        toDo.Id = Int32.Parse(reader["ToDoId"].ToString());
                        toDo.Identifier = Guid.Parse(reader["ToDoIdentifier"].ToString());
                        toDo.Name = reader["ToDoName"].ToString();

                        if (reader["ToDoDescription"] != DBNull.Value)
                            toDo.Description = reader["ToDoDescription"].ToString();
                        if (reader["Path"] != DBNull.Value)
                            toDo.Path = reader["Path"].ToString();
                        if (reader["ToDoDate"] != DBNull.Value)
                            toDo.ToDoDate = DateTime.Parse(reader["ToDoDate"].ToString());

                        toDo.Active = bool.Parse(reader["Active"].ToString());
                        toDo.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            toDo.CreatedBy = new User();
                            toDo.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            toDo.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            toDo.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            toDo.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            toDo.Company = new Company();
                            toDo.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            toDo.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            toDo.Company.Name = reader["CompanyName"].ToString();
                        }

                        ToDos.Add(toDo);
                    }
                }
            }
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
                    dbEntry.Path = toDo.Path;
                    dbEntry.ToDoDate = toDo.ToDoDate;

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
                .FirstOrDefault(x => x.Identifier == identifier);

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
