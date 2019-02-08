using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.OutputInvoices;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class OutputInvoiceNoteViewRepository : IOutputInvoiceNoteRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public OutputInvoiceNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<OutputInvoiceNote> GetOutputInvoiceNotes(int companyId)
        {
            List<OutputInvoiceNote> OutputInvoiceNotes = new List<OutputInvoiceNote>();

            string queryString =
                "SELECT OutputInvoiceNoteId, OutputInvoiceNoteIdentifier, " +
                "OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, OutputInvoiceName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vOutputInvoiceNotes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    OutputInvoiceNote outputInvoiceNote;
                    while (reader.Read())
                    {
                        outputInvoiceNote = new OutputInvoiceNote();
                        outputInvoiceNote.Id = Int32.Parse(reader["OutputInvoiceNoteId"].ToString());
                        outputInvoiceNote.Identifier = Guid.Parse(reader["OutputInvoiceNoteIdentifier"].ToString());

                        if (reader["OutputInvoiceId"] != DBNull.Value)
                        {
                            outputInvoiceNote.OutputInvoice = new OutputInvoice();
                            outputInvoiceNote.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
                            outputInvoiceNote.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();
                        }

                        if (reader["Note"] != DBNull.Value)
                            outputInvoiceNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            outputInvoiceNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        outputInvoiceNote.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoiceNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            outputInvoiceNote.CreatedBy = new User();
                            outputInvoiceNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            outputInvoiceNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            outputInvoiceNote.Company = new Company();
                            outputInvoiceNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        OutputInvoiceNotes.Add(outputInvoiceNote);
                    }
                }
            }
            return OutputInvoiceNotes;


            //List<OutputInvoiceNote> OutputInvoiceNotes = context.OutputInvoiceNotes
            //    .Include(x => x.OutputInvoice)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return OutputInvoiceNotes;
        }

        public List<OutputInvoiceNote> GetOutputInvoiceNotesByOutputInvoice(int OutputInvoiceId)
        {
            List<OutputInvoiceNote> OutputInvoiceNotes = new List<OutputInvoiceNote>();

            string queryString =
                "SELECT OutputInvoiceNoteId, OutputInvoiceNoteIdentifier, " +
                "OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, OutputInvoiceName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vOutputInvoiceNotes " +
                "WHERE OutputInvoiceId = @OutputInvoiceId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@OutputInvoiceId", OutputInvoiceId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    OutputInvoiceNote outputInvoiceNote;
                    while (reader.Read())
                    {
                        outputInvoiceNote = new OutputInvoiceNote();
                        outputInvoiceNote.Id = Int32.Parse(reader["OutputInvoiceNoteId"].ToString());
                        outputInvoiceNote.Identifier = Guid.Parse(reader["OutputInvoiceNoteIdentifier"].ToString());

                        if (reader["OutputInvoiceId"] != DBNull.Value)
                        {
                            outputInvoiceNote.OutputInvoice = new OutputInvoice();
                            outputInvoiceNote.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
                            outputInvoiceNote.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            outputInvoiceNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            outputInvoiceNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        outputInvoiceNote.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoiceNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            outputInvoiceNote.CreatedBy = new User();
                            outputInvoiceNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            outputInvoiceNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            outputInvoiceNote.Company = new Company();
                            outputInvoiceNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        OutputInvoiceNotes.Add(outputInvoiceNote);
                    }
                }
            }
            return OutputInvoiceNotes;
        }

        public List<OutputInvoiceNote> GetOutputInvoiceNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<OutputInvoiceNote> OutputInvoiceNotes = new List<OutputInvoiceNote>();

            string queryString =
                "SELECT OutputInvoiceNoteId, OutputInvoiceNoteIdentifier, " +
                "OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, OutputInvoiceName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vOutputInvoiceNotes " +
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
                    OutputInvoiceNote outputInvoiceNote;
                    while (reader.Read())
                    {
                        outputInvoiceNote = new OutputInvoiceNote();
                        outputInvoiceNote.Id = Int32.Parse(reader["OutputInvoiceNoteId"].ToString());
                        outputInvoiceNote.Identifier = Guid.Parse(reader["OutputInvoiceNoteIdentifier"].ToString());

                        if (reader["OutputInvoiceId"] != DBNull.Value)
                        {
                            outputInvoiceNote.OutputInvoice = new OutputInvoice();
                            outputInvoiceNote.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
                            outputInvoiceNote.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
                            outputInvoiceNote.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            outputInvoiceNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            outputInvoiceNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        outputInvoiceNote.Active = bool.Parse(reader["Active"].ToString());
                        outputInvoiceNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            outputInvoiceNote.CreatedBy = new User();
                            outputInvoiceNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            outputInvoiceNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            outputInvoiceNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            outputInvoiceNote.Company = new Company();
                            outputInvoiceNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            outputInvoiceNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        OutputInvoiceNotes.Add(outputInvoiceNote);
                    }
                }
            }
            return OutputInvoiceNotes;
        }

        public OutputInvoiceNote Create(OutputInvoiceNote OutputInvoiceNote)
        {
            if (context.OutputInvoiceNotes.Where(x => x.Identifier != null && x.Identifier == OutputInvoiceNote.Identifier).Count() == 0)
            {
                OutputInvoiceNote.Id = 0;

                OutputInvoiceNote.Active = true;

                context.OutputInvoiceNotes.Add(OutputInvoiceNote);
                return OutputInvoiceNote;
            }
            else
            {
                // Load item that will be updated
                OutputInvoiceNote dbEntry = context.OutputInvoiceNotes
                    .FirstOrDefault(x => x.Identifier == OutputInvoiceNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = OutputInvoiceNote.CompanyId ?? null;
                    dbEntry.CreatedById = OutputInvoiceNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = OutputInvoiceNote.Note;
                    dbEntry.NoteDate = OutputInvoiceNote.NoteDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public OutputInvoiceNote Delete(Guid identifier)
        {
            OutputInvoiceNote dbEntry = context.OutputInvoiceNotes
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
