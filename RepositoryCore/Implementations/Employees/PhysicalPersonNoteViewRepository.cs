using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.PhysicalPersons
{
    public class PhysicalPersonNoteViewRepository : IPhysicalPersonNoteRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<PhysicalPersonNote> GetPhysicalPersonNotes(int companyId)
        {
            List<PhysicalPersonNote> PhysicalPersonNotes = new List<PhysicalPersonNote>();

            string queryString =
                "SELECT PhysicalPersonNoteId, PhysicalPersonNoteIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "Note, NoteDate, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonNotes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonNote physicalPersonNote;
                    while (reader.Read())
                    {
                        physicalPersonNote = new PhysicalPersonNote();
                        physicalPersonNote.Id = Int32.Parse(reader["PhysicalPersonNoteId"].ToString());
                        physicalPersonNote.Identifier = Guid.Parse(reader["PhysicalPersonNoteIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonNote.PhysicalPerson = new PhysicalPerson();
                            physicalPersonNote.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonNote.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonNote.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["Note"] != DBNull.Value)
                            physicalPersonNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            physicalPersonNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonNote.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonNote.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonNote.CreatedBy = new User();
                            physicalPersonNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonNote.Company = new Company();
                            physicalPersonNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonNotes.Add(physicalPersonNote);
                    }
                }
            }
            return PhysicalPersonNotes;


            //List<PhysicalPersonNote> PhysicalPersonNotes = context.PhysicalPersonNotes
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersonNotes;
        }

        public List<PhysicalPersonNote> GetPhysicalPersonNotesByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonNote> PhysicalPersonNotes = new List<PhysicalPersonNote>();

            string queryString =
                "SELECT PhysicalPersonNoteId, PhysicalPersonNoteIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "Note, NoteDate, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonNotes " +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonNote physicalPersonNote;
                    while (reader.Read())
                    {
                        physicalPersonNote = new PhysicalPersonNote();
                        physicalPersonNote.Id = Int32.Parse(reader["PhysicalPersonNoteId"].ToString());
                        physicalPersonNote.Identifier = Guid.Parse(reader["PhysicalPersonNoteIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonNote.PhysicalPerson = new PhysicalPerson();
                            physicalPersonNote.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonNote.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonNote.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            physicalPersonNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            physicalPersonNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonNote.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonNote.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonNote.CreatedBy = new User();
                            physicalPersonNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonNote.Company = new Company();
                            physicalPersonNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonNotes.Add(physicalPersonNote);
                    }
                }
            }
            return PhysicalPersonNotes;

            //List<PhysicalPersonNote> PhysicalPersons = context.PhysicalPersonNotes
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public List<PhysicalPersonNote> GetPhysicalPersonNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonNote> PhysicalPersonNotes = new List<PhysicalPersonNote>();

            string queryString =
                "SELECT PhysicalPersonNoteId, PhysicalPersonNoteIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "Note, NoteDate, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonNotes " +
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
                    PhysicalPersonNote physicalPersonNote;
                    while (reader.Read())
                    {
                        physicalPersonNote = new PhysicalPersonNote();
                        physicalPersonNote.Id = Int32.Parse(reader["PhysicalPersonNoteId"].ToString());
                        physicalPersonNote.Identifier = Guid.Parse(reader["PhysicalPersonNoteIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonNote.PhysicalPerson = new PhysicalPerson();
                            physicalPersonNote.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonNote.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonNote.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonNote.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            physicalPersonNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            physicalPersonNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonNote.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonNote.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonNote.CreatedBy = new User();
                            physicalPersonNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonNote.Company = new Company();
                            physicalPersonNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonNotes.Add(physicalPersonNote);
                    }
                }
            }
            return PhysicalPersonNotes;

            //List<PhysicalPersonNote> PhysicalPersons = context.PhysicalPersonNotes
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public PhysicalPersonNote Create(PhysicalPersonNote PhysicalPersonNote)
        {
            if (context.PhysicalPersonNotes.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonNote.Identifier).Count() == 0)
            {
                PhysicalPersonNote.Id = 0;

                PhysicalPersonNote.Active = true;

                PhysicalPersonNote.UpdatedAt = DateTime.Now;
                PhysicalPersonNote.CreatedAt = DateTime.Now;

                context.PhysicalPersonNotes.Add(PhysicalPersonNote);
                return PhysicalPersonNote;
            }
            else
            {
                // Load item that will be updated
                PhysicalPersonNote dbEntry = context.PhysicalPersonNotes
                    .FirstOrDefault(x => x.Identifier == PhysicalPersonNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = PhysicalPersonNote.CompanyId ?? null;
                    dbEntry.CreatedById = PhysicalPersonNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = PhysicalPersonNote.Note;
                    dbEntry.NoteDate = PhysicalPersonNote.NoteDate;
                    dbEntry.ItemStatus = PhysicalPersonNote.ItemStatus;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonNote Delete(Guid identifier)
        {
            PhysicalPersonNote dbEntry = context.PhysicalPersonNotes
                 .Union(context.ChangeTracker.Entries()
                     .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonNote))
                     .Select(x => x.Entity as PhysicalPersonNote))
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
