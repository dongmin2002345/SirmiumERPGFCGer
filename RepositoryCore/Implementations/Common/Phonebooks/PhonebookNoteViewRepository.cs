using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Phonebooks;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Phonebooks;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Phonebooks
{
    public class PhonebookNoteViewRepository: IPhonebookNoteRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public PhonebookNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public string selectString =
            "SELECT PhonebookNoteId, PhonebookNoteIdentifier, " +
                            "PhonebookId, PhonebookIdentifier, PhonebookCode, PhonebookName, " +
                            "Note, NoteDate, ItemStatus, " +
                            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vPhonebookNotes ";

        private static PhonebookNote Read(SqlDataReader reader)
        {
            PhonebookNote PhonebookNote = new PhonebookNote();
            PhonebookNote.Id = Int32.Parse(reader["PhonebookNoteId"].ToString());
            PhonebookNote.Identifier = Guid.Parse(reader["PhonebookNoteIdentifier"].ToString());

            if (reader["PhonebookId"] != DBNull.Value)
            {
                PhonebookNote.Phonebook = new Phonebook();
                PhonebookNote.PhonebookId = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookNote.Phonebook.Id = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookNote.Phonebook.Identifier = Guid.Parse(reader["PhonebookIdentifier"].ToString());
                PhonebookNote.Phonebook.Code = reader["PhonebookCode"].ToString();
                PhonebookNote.Phonebook.Name = reader["PhonebookName"].ToString();
            }


            if (reader["Note"] != DBNull.Value)
                PhonebookNote.Note = reader["Note"].ToString();
            if (reader["NoteDate"] != DBNull.Value)
                PhonebookNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());
            if (reader["ItemStatus"] != DBNull.Value)
                PhonebookNote.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
            PhonebookNote.Active = bool.Parse(reader["Active"].ToString());
            PhonebookNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                PhonebookNote.CreatedBy = new User();
                PhonebookNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                PhonebookNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                PhonebookNote.Company = new Company();
                PhonebookNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookNote.Company.Name = reader["CompanyName"].ToString();
            }

            return PhonebookNote;
        }

        public List<PhonebookNote> GetPhonebookNotes(int companyId)
        {
            List<PhonebookNote> PhonebookNotes = new List<PhonebookNote>();

            string queryString =
                selectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhonebookNote PhonebookNote;
                    while (reader.Read())
                    {
                        PhonebookNote = Read(reader);
                        PhonebookNotes.Add(PhonebookNote);
                    }
                }
            }

            return PhonebookNotes;
        }

        public List<PhonebookNote> GetPhonebookNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhonebookNote> PhonebookNotes = new List<PhonebookNote>();

            string queryString =
                selectString +
                "WHERE CompanyId = @CompanyId AND " +
                "CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhonebookNote PhonebookNote;
                    while (reader.Read())
                    {
                        PhonebookNote = Read(reader);
                        PhonebookNotes.Add(PhonebookNote);
                    }
                }
            }

            return PhonebookNotes;
        }


        public List<PhonebookNote> GetPhonebookNotesByPhonebook(int PhonebookId)
        {
            List<PhonebookNote> PhonebookNotes = new List<PhonebookNote>();

            string queryString =
                selectString +
                "WHERE PhonebookId = @PhonebookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhonebookId", PhonebookId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhonebookNote PhonebookNote;
                    while (reader.Read())
                    {
                        PhonebookNote = Read(reader);
                        PhonebookNotes.Add(PhonebookNote);
                    }
                }
            }

            return PhonebookNotes;
        }

        public PhonebookNote GetPhonebookNote(int id)
        {
            PhonebookNote PhonebookNote = null;

            string queryString =
                selectString +
                "WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@Id", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PhonebookNote = Read(reader);
                    }
                }
            }
            return PhonebookNote;
        }

        public PhonebookNote Create(PhonebookNote PhonebookNote)
        {
            if (context.PhonebookNotes.Where(x => x.Identifier != null && x.Identifier == PhonebookNote.Identifier).Count() == 0)
            {
                PhonebookNote.Id = 0;

                PhonebookNote.Active = true;
                PhonebookNote.UpdatedAt = DateTime.Now;
                PhonebookNote.CreatedAt = DateTime.Now;

                context.PhonebookNotes.Add(PhonebookNote);
                return PhonebookNote;
            }
            else
            {
                // Load PhonebookNote that will be updated
                PhonebookNote dbEntry = context.PhonebookNotes
                    .FirstOrDefault(x => x.Identifier == PhonebookNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.PhonebookId = PhonebookNote.PhonebookId ?? null;
                    dbEntry.CompanyId = PhonebookNote.CompanyId ?? null;
                    dbEntry.CreatedById = PhonebookNote.CreatedById ?? null;

                    // Set properties

                    dbEntry.Note = PhonebookNote.Note;
                    dbEntry.NoteDate = PhonebookNote.NoteDate;

                    dbEntry.ItemStatus = PhonebookNote.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhonebookNote Delete(Guid identifier)
        {
            PhonebookNote dbEntry = context.PhonebookNotes
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhonebookNote))
                    .Select(x => x.Entity as PhonebookNote))
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
