using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeNoteViewRepository : IEmployeeNoteRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeNote> GetEmployeeNotes(int companyId)
        {
            List<EmployeeNote> EmployeeNotes = new List<EmployeeNote>();

            string queryString =
                "SELECT EmployeeNoteId, EmployeeNoteIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeNotes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeNote employeeNote;
                    while (reader.Read())
                    {
                        employeeNote = new EmployeeNote();
                        employeeNote.Id = Int32.Parse(reader["EmployeeNoteId"].ToString());
                        employeeNote.Identifier = Guid.Parse(reader["EmployeeNoteIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeNote.Employee = new Employee();
                            employeeNote.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeNote.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeNote.Employee.Name = reader["EmployeeName"].ToString();
                        }
                       
                        if (reader["Note"] != DBNull.Value)
                            employeeNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            employeeNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        employeeNote.Active = bool.Parse(reader["Active"].ToString());
                        employeeNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeNote.CreatedBy = new User();
                            employeeNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeNote.Company = new Company();
                            employeeNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeNotes.Add(employeeNote);
                    }
                }
            }
            return EmployeeNotes;


            //List<EmployeeNote> EmployeeNotes = context.EmployeeNotes
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return EmployeeNotes;
        }

        public List<EmployeeNote> GetEmployeeNotesByEmployee(int EmployeeId)
        {
            List<EmployeeNote> EmployeeNotes = new List<EmployeeNote>();

            string queryString =
                "SELECT EmployeeNoteId, EmployeeNoteIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeNotes " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeNote employeeNote;
                    while (reader.Read())
                    {
                        employeeNote = new EmployeeNote();
                        employeeNote.Id = Int32.Parse(reader["EmployeeNoteId"].ToString());
                        employeeNote.Identifier = Guid.Parse(reader["EmployeeNoteIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeNote.Employee = new Employee();
                            employeeNote.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeNote.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeNote.Employee.Name = reader["EmployeeName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            employeeNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            employeeNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        employeeNote.Active = bool.Parse(reader["Active"].ToString());
                        employeeNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeNote.CreatedBy = new User();
                            employeeNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeNote.Company = new Company();
                            employeeNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeNotes.Add(employeeNote);
                    }
                }
            }
            return EmployeeNotes;

            //List<EmployeeNote> Employees = context.EmployeeNotes
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeNote> GetEmployeeNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeNote> EmployeeNotes = new List<EmployeeNote>();

            string queryString =
                "SELECT EmployeeNoteId, EmployeeNoteIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeNotes " +
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
                    EmployeeNote employeeNote;
                    while (reader.Read())
                    {
                        employeeNote = new EmployeeNote();
                        employeeNote.Id = Int32.Parse(reader["EmployeeNoteId"].ToString());
                        employeeNote.Identifier = Guid.Parse(reader["EmployeeNoteIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeNote.Employee = new Employee();
                            employeeNote.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeNote.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeNote.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeNote.Employee.Name = reader["EmployeeName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            employeeNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            employeeNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        employeeNote.Active = bool.Parse(reader["Active"].ToString());
                        employeeNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeNote.CreatedBy = new User();
                            employeeNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeNote.Company = new Company();
                            employeeNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeNotes.Add(employeeNote);
                    }
                }
            }
            return EmployeeNotes;

            //List<EmployeeNote> Employees = context.EmployeeNotes
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeNote Create(EmployeeNote EmployeeNote)
        {
            if (context.EmployeeNotes.Where(x => x.Identifier != null && x.Identifier == EmployeeNote.Identifier).Count() == 0)
            {
                EmployeeNote.Id = 0;

                EmployeeNote.Active = true;

                context.EmployeeNotes.Add(EmployeeNote);
                return EmployeeNote;
            }
            else
            {
                // Load item that will be updated
                EmployeeNote dbEntry = context.EmployeeNotes
                    .FirstOrDefault(x => x.Identifier == EmployeeNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeNote.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = EmployeeNote.Note;
                    dbEntry.NoteDate = EmployeeNote.NoteDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeNote Delete(Guid identifier)
        {
            EmployeeNote dbEntry = context.EmployeeNotes
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
