using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteNoteViewRepository : IConstructionSiteNoteRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public ConstructionSiteNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ConstructionSiteNote> GetConstructionSiteNotes(int companyId)
        {
            List<ConstructionSiteNote> ConstructionSiteNotes = new List<ConstructionSiteNote>();

            string queryString =
                "SELECT ConstructionSiteNoteId, ConstructionSiteNoteIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteNotes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteNote constructionSiteNote;
                    while (reader.Read())
                    {
                        constructionSiteNote = new ConstructionSiteNote();
                        constructionSiteNote.Id = Int32.Parse(reader["ConstructionSiteNoteId"].ToString());
                        constructionSiteNote.Identifier = Guid.Parse(reader["ConstructionSiteNoteIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteNote.ConstructionSite = new ConstructionSite();
                            constructionSiteNote.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteNote.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteNote.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["Note"] != null)
                            constructionSiteNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != null)
                            constructionSiteNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        constructionSiteNote.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteNote.CreatedBy = new User();
                            constructionSiteNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            constructionSiteNote.Company = new Company();
                            constructionSiteNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteNotes.Add(constructionSiteNote);
                    }
                }
            }
            return ConstructionSiteNotes;


            //List<ConstructionSiteNote> ConstructionSiteNotes = context.ConstructionSiteNotes
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSiteNotes;
        }

        public List<ConstructionSiteNote> GetConstructionSiteNotesByConstructionSite(int ConstructionSiteId)
        {
            List<ConstructionSiteNote> ConstructionSiteNotes = new List<ConstructionSiteNote>();

            string queryString =
                "SELECT ConstructionSiteNoteId, ConstructionSiteNoteIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteNotes " +
                "WHERE ConstructionSiteId = @ConstructionSiteId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", ConstructionSiteId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteNote constructionSiteNote;
                    while (reader.Read())
                    {
                        constructionSiteNote = new ConstructionSiteNote();
                        constructionSiteNote.Id = Int32.Parse(reader["ConstructionSiteNoteId"].ToString());
                        constructionSiteNote.Identifier = Guid.Parse(reader["ConstructionSiteNoteIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteNote.ConstructionSite = new ConstructionSite();
                            constructionSiteNote.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteNote.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteNote.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }


                        if (reader["Note"] != null)
                            constructionSiteNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != null)
                            constructionSiteNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        constructionSiteNote.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteNote.CreatedBy = new User();
                            constructionSiteNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            constructionSiteNote.Company = new Company();
                            constructionSiteNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteNotes.Add(constructionSiteNote);
                    }
                }
            }
            return ConstructionSiteNotes;

            //List<ConstructionSiteNote> ConstructionSites = context.ConstructionSiteNotes
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.ConstructionSiteId == ConstructionSiteId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public List<ConstructionSiteNote> GetConstructionSiteNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSiteNote> ConstructionSiteNotes = new List<ConstructionSiteNote>();

            string queryString =
                "SELECT ConstructionSiteNoteId, ConstructionSiteNoteIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteNotes " +
                "WHERE CompanyId = @CompanyId AND UpdatedAt > @LastUpdateTime;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteNote constructionSiteNote;
                    while (reader.Read())
                    {
                        constructionSiteNote = new ConstructionSiteNote();
                        constructionSiteNote.Id = Int32.Parse(reader["ConstructionSiteNoteId"].ToString());
                        constructionSiteNote.Identifier = Guid.Parse(reader["ConstructionSiteNoteIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteNote.ConstructionSite = new ConstructionSite();
                            constructionSiteNote.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteNote.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteNote.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteNote.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }


                        if (reader["Note"] != null)
                            constructionSiteNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != null)
                            constructionSiteNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        constructionSiteNote.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteNote.CreatedBy = new User();
                            constructionSiteNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            constructionSiteNote.Company = new Company();
                            constructionSiteNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteNotes.Add(constructionSiteNote);
                    }
                }
            }
            return ConstructionSiteNotes;

            //List<ConstructionSiteNote> ConstructionSites = context.ConstructionSiteNotes
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public ConstructionSiteNote Create(ConstructionSiteNote ConstructionSiteNote)
        {
            if (context.ConstructionSiteNotes.Where(x => x.Identifier != null && x.Identifier == ConstructionSiteNote.Identifier).Count() == 0)
            {
                ConstructionSiteNote.Id = 0;

                ConstructionSiteNote.Active = true;

                context.ConstructionSiteNotes.Add(ConstructionSiteNote);
                return ConstructionSiteNote;
            }
            else
            {
                // Load item that will be updated
                ConstructionSiteNote dbEntry = context.ConstructionSiteNotes
                    .FirstOrDefault(x => x.Identifier == ConstructionSiteNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = ConstructionSiteNote.CompanyId ?? null;
                    dbEntry.CreatedById = ConstructionSiteNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = ConstructionSiteNote.Note;
                    dbEntry.NoteDate = ConstructionSiteNote.NoteDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSiteNote Delete(Guid identifier)
        {
            ConstructionSiteNote dbEntry = context.ConstructionSiteNotes
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
