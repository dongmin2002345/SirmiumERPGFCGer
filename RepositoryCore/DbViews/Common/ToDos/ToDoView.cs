﻿using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.ToDos
{
    public class ToDoView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vToDos";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vToDos AS " +
                "SELECT toDo.Id AS ToDoId, toDo.Identifier AS ToDoIdentifier, toDo.Name AS ToDoName, toDo.Description AS ToDoDescription, toDo.ToDoDate, " +
                "toDo.Active, toDo.UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM ToDos toDo " +
                "LEFT JOIN Users createdBy ON toDo.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON toDo.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}