using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.InputInvoices;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class InputInvoiceNoteViewRepository : IInputInvoiceNoteRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        string selectString =
            "SELECT InputInvoiceNoteId, InputInvoiceNoteIdentifier, " +
                "InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "Note, NoteDate, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vInputInvoiceNotes ";

        public InputInvoiceNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        private static InputInvoiceNote Read(SqlDataReader reader)
        {
            InputInvoiceNote inputInvoiceNote = new InputInvoiceNote();
            inputInvoiceNote.Id = Int32.Parse(reader["InputInvoiceNoteId"].ToString());
            inputInvoiceNote.Identifier = Guid.Parse(reader["InputInvoiceNoteIdentifier"].ToString());

            if (reader["InputInvoiceId"] != DBNull.Value)
            {
                inputInvoiceNote.InputInvoice = new InputInvoice();
                inputInvoiceNote.InputInvoiceId = Int32.Parse(reader["InputInvoiceId"].ToString());
                inputInvoiceNote.InputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
                inputInvoiceNote.InputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
                inputInvoiceNote.InputInvoice.Code = reader["InputInvoiceCode"].ToString();
            }

            if (reader["Note"] != DBNull.Value)
                inputInvoiceNote.Note = reader["Note"].ToString();
            if (reader["NoteDate"] != DBNull.Value)
                inputInvoiceNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());
            if (reader["ItemStatus"] != DBNull.Value)
                inputInvoiceNote.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
            inputInvoiceNote.Active = bool.Parse(reader["Active"].ToString());
            inputInvoiceNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                inputInvoiceNote.CreatedBy = new User();
                inputInvoiceNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                inputInvoiceNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                inputInvoiceNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                inputInvoiceNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                inputInvoiceNote.Company = new Company();
                inputInvoiceNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                inputInvoiceNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                inputInvoiceNote.Company.Name = reader["CompanyName"].ToString();
            }

            return inputInvoiceNote;
        }

        public List<InputInvoiceNote> GetInputInvoiceNotes(int companyId)
        {
            List<InputInvoiceNote> inputInvoiceNotes = new List<InputInvoiceNote>();

            string queryString = selectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InputInvoiceNote inputInvoiceNote;
                    while (reader.Read())
                    {
                        inputInvoiceNote = Read(reader);
                        inputInvoiceNotes.Add(inputInvoiceNote);
                    }
                }
            }

            return inputInvoiceNotes;
        }

        public List<InputInvoiceNote> GetInputInvoiceNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<InputInvoiceNote> inputInvoiceNotes = new List<InputInvoiceNote>();

            string queryString = selectString +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(NVARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(NVARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));


                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InputInvoiceNote inputInvoiceNote;
                    while (reader.Read())
                    {
                        inputInvoiceNote = Read(reader);
                        inputInvoiceNotes.Add(inputInvoiceNote);
                    }
                }
            }

            return inputInvoiceNotes;
        }

        public List<InputInvoiceNote> GetInputInvoiceNotesByInputInvoice(int inputInvoiceId)
        {
            List<InputInvoiceNote> inputInvoiceNotes = new List<InputInvoiceNote>();

            string queryString = selectString +
                "WHERE InputInvoiceId = @InputInvoiceId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@InputInvoiceId", inputInvoiceId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    InputInvoiceNote inputInvoiceNote;
                    while (reader.Read())
                    {
                        inputInvoiceNote = Read(reader);
                        inputInvoiceNotes.Add(inputInvoiceNote);
                    }
                }
            }

            return inputInvoiceNotes;
        }


        public InputInvoiceNote Create(InputInvoiceNote InputInvoiceNote)
        {
            if (context.InputInvoiceNotes.Where(x => x.Identifier != null && x.Identifier == InputInvoiceNote.Identifier).Count() == 0)
            {
                InputInvoiceNote.Id = 0;

                InputInvoiceNote.Active = true;
                InputInvoiceNote.UpdatedAt = DateTime.Now;
                InputInvoiceNote.CreatedAt = DateTime.Now;
                context.InputInvoiceNotes.Add(InputInvoiceNote);
                return InputInvoiceNote;
            }
            else
            {
                // Load item that will be updated
                InputInvoiceNote dbEntry = context.InputInvoiceNotes
                    .FirstOrDefault(x => x.Identifier == InputInvoiceNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = InputInvoiceNote.CompanyId ?? null;
                    dbEntry.CreatedById = InputInvoiceNote.CreatedById ?? null;
                    dbEntry.InputInvoiceId = InputInvoiceNote.InputInvoiceId ?? null;
                    // Set properties
                    dbEntry.Note = InputInvoiceNote.Note;
                    dbEntry.NoteDate = InputInvoiceNote.NoteDate;
                    dbEntry.ItemStatus = InputInvoiceNote.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public InputInvoiceNote Delete(Guid identifier)
        {
            InputInvoiceNote dbEntry = context.InputInvoiceNotes
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InputInvoiceNote))
                    .Select(x => x.Entity as InputInvoiceNote))
                .FirstOrDefault(x => x.Identifier == identifier);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
