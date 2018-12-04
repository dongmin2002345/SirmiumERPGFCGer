using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerNoteViewRepository : IBusinessPartnerNoteRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerNoteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerNote> GetBusinessPartnerNotes(int companyId)
        {
            List<BusinessPartnerNote> BusinessPartnerNotes = new List<BusinessPartnerNote>();

            string queryString =
                "SELECT BusinessPartnerNoteId, BusinessPartnerNoteIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerNotes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerNote businessPartnerNote;
                    while (reader.Read())
                    {
                        businessPartnerNote = new BusinessPartnerNote();
                        businessPartnerNote.Id = Int32.Parse(reader["BusinessPartnerNoteId"].ToString());
                        businessPartnerNote.Identifier = Guid.Parse(reader["BusinessPartnerNoteIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerNote.BusinessPartner = new BusinessPartner();
                            businessPartnerNote.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerNote.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerNote.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Note"] != DBNull.Value)
                            businessPartnerNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            businessPartnerNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        businessPartnerNote.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerNote.CreatedBy = new User();
                            businessPartnerNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerNote.Company = new Company();
                            businessPartnerNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerNotes.Add(businessPartnerNote);
                    }
                }
            }
            return BusinessPartnerNotes;


            //List<BusinessPartnerNote> BusinessPartnerNotes = context.BusinessPartnerNotes
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerNotes;
        }

        public List<BusinessPartnerNote> GetBusinessPartnerNotesByBusinessPartner(int BusinessPartnerId)
        {
            List<BusinessPartnerNote> BusinessPartnerNotes = new List<BusinessPartnerNote>();

            string queryString =
                "SELECT BusinessPartnerNoteId, BusinessPartnerNoteIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerNotes " +
                "WHERE BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", BusinessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerNote businessPartnerNote;
                    while (reader.Read())
                    {
                        businessPartnerNote = new BusinessPartnerNote();
                        businessPartnerNote.Id = Int32.Parse(reader["BusinessPartnerNoteId"].ToString());
                        businessPartnerNote.Identifier = Guid.Parse(reader["BusinessPartnerNoteIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerNote.BusinessPartner = new BusinessPartner();
                            businessPartnerNote.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerNote.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerNote.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            businessPartnerNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            businessPartnerNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        businessPartnerNote.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerNote.CreatedBy = new User();
                            businessPartnerNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerNote.Company = new Company();
                            businessPartnerNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerNotes.Add(businessPartnerNote);
                    }
                }
            }
            return BusinessPartnerNotes;

            //List<BusinessPartnerNote> BusinessPartners = context.BusinessPartnerNotes
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == BusinessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartners;
        }

        public List<BusinessPartnerNote> GetBusinessPartnerNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerNote> BusinessPartnerNotes = new List<BusinessPartnerNote>();

            string queryString =
                "SELECT BusinessPartnerNoteId, BusinessPartnerNoteIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Note, NoteDate, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerNotes " +
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
                    BusinessPartnerNote businessPartnerNote;
                    while (reader.Read())
                    {
                        businessPartnerNote = new BusinessPartnerNote();
                        businessPartnerNote.Id = Int32.Parse(reader["BusinessPartnerNoteId"].ToString());
                        businessPartnerNote.Identifier = Guid.Parse(reader["BusinessPartnerNoteIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerNote.BusinessPartner = new BusinessPartner();
                            businessPartnerNote.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerNote.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerNote.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerNote.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }


                        if (reader["Note"] != DBNull.Value)
                            businessPartnerNote.Note = reader["Note"].ToString();
                        if (reader["NoteDate"] != DBNull.Value)
                            businessPartnerNote.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

                        businessPartnerNote.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerNote.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerNote.CreatedBy = new User();
                            businessPartnerNote.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerNote.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerNote.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerNote.Company = new Company();
                            businessPartnerNote.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerNote.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerNotes.Add(businessPartnerNote);
                    }
                }
            }
            return BusinessPartnerNotes;

            //List<BusinessPartnerNote> BusinessPartners = context.BusinessPartnerNotes
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartners;
        }

        public BusinessPartnerNote Create(BusinessPartnerNote BusinessPartnerNote)
        {
            if (context.BusinessPartnerNotes.Where(x => x.Identifier != null && x.Identifier == BusinessPartnerNote.Identifier).Count() == 0)
            {
                BusinessPartnerNote.Id = 0;

                BusinessPartnerNote.Active = true;

                context.BusinessPartnerNotes.Add(BusinessPartnerNote);
                return BusinessPartnerNote;
            }
            else
            {
                // Load item that will be updated
                BusinessPartnerNote dbEntry = context.BusinessPartnerNotes
                    .FirstOrDefault(x => x.Identifier == BusinessPartnerNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = BusinessPartnerNote.CompanyId ?? null;
                    dbEntry.CreatedById = BusinessPartnerNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = BusinessPartnerNote.Note;
                    dbEntry.NoteDate = BusinessPartnerNote.NoteDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerNote Delete(Guid identifier)
        {
            BusinessPartnerNote dbEntry = context.BusinessPartnerNotes
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
