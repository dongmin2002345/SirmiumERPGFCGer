using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Employees;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.PhysicalPersons
{
    public class PhysicalPersonLicenceViewRepository : IPhysicalPersonLicenceRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonLicenceViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<PhysicalPersonLicence> GetPhysicalPersonItems(int companyId)  // izmene u IPhysicalPersonLicenceRepository -> GetPhysicalPersonLicences?
        {
            List<PhysicalPersonLicence> PhysicalPersonLicences = new List<PhysicalPersonLicence>();

            string queryString =
                "SELECT PhysicalPersonLicenceId, PhysicalPersonLicenceIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonLicences " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonLicence physicalPersonLicence;
                    while (reader.Read())
                    {
                        physicalPersonLicence = new PhysicalPersonLicence();
                        physicalPersonLicence.Id = Int32.Parse(reader["PhysicalPersonLicenceId"].ToString());
                        physicalPersonLicence.Identifier = Guid.Parse(reader["PhysicalPersonLicenceIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonLicence.PhysicalPerson = new PhysicalPerson();
                            physicalPersonLicence.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonLicence.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonLicence.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Licence = new LicenceType();
                            physicalPersonLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            physicalPersonLicence.Licence.Code = reader["LicenceCode"].ToString();
                            physicalPersonLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            physicalPersonLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Country = new Country();
                            physicalPersonLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonLicence.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            physicalPersonLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            physicalPersonLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        physicalPersonLicence.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonLicence.CreatedBy = new User();
                            physicalPersonLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Company = new Company();
                            physicalPersonLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonLicences.Add(physicalPersonLicence);
                    }
                }
            }
            return PhysicalPersonLicences;

            //List<PhysicalPersonLicence> PhysicalPersonLicences = context.PhysicalPersonLicences
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersonLicences;
        }

        public List<PhysicalPersonLicence> GetPhysicalPersonItemsByPhysicalPerson(int PhysicalPersonId) //GetPhysicalPersonLicencesByPhysicalPerson?
        {
            List<PhysicalPersonLicence> PhysicalPersonLicences = new List<PhysicalPersonLicence>();

            string queryString =
                "SELECT PhysicalPersonLicenceId, PhysicalPersonLicenceIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonLicences " +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonLicence physicalPersonLicence;
                    while (reader.Read())
                    {
                        physicalPersonLicence = new PhysicalPersonLicence();
                        physicalPersonLicence.Id = Int32.Parse(reader["PhysicalPersonLicenceId"].ToString());
                        physicalPersonLicence.Identifier = Guid.Parse(reader["PhysicalPersonLicenceIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonLicence.PhysicalPerson = new PhysicalPerson();
                            physicalPersonLicence.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonLicence.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonLicence.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Licence = new LicenceType();
                            physicalPersonLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            physicalPersonLicence.Licence.Code = reader["LicenceCode"].ToString();
                            physicalPersonLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            physicalPersonLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Country = new Country();
                            physicalPersonLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonLicence.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            physicalPersonLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            physicalPersonLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        physicalPersonLicence.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonLicence.CreatedBy = new User();
                            physicalPersonLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Company = new Company();
                            physicalPersonLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonLicences.Add(physicalPersonLicence);
                    }
                }
            }
            return PhysicalPersonLicences;

            //List<PhysicalPersonLicence> PhysicalPersons = context.PhysicalPersonLicences
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public List<PhysicalPersonLicence> GetPhysicalPersonItemsNewerThan(int companyId, DateTime lastUpdateTime) //GetPhysicalPersonLicencesNewerThan?
        {
            List<PhysicalPersonLicence> PhysicalPersonLicences = new List<PhysicalPersonLicence>();

            string queryString =
                "SELECT PhysicalPersonLicenceId, PhysicalPersonLicenceIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonLicences " +
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
                    PhysicalPersonLicence physicalPersonLicence;
                    while (reader.Read())
                    {
                        physicalPersonLicence = new PhysicalPersonLicence();
                        physicalPersonLicence.Id = Int32.Parse(reader["PhysicalPersonLicenceId"].ToString());
                        physicalPersonLicence.Identifier = Guid.Parse(reader["PhysicalPersonLicenceIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonLicence.PhysicalPerson = new PhysicalPerson();
                            physicalPersonLicence.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonLicence.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonLicence.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonLicence.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Licence = new LicenceType();
                            physicalPersonLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            physicalPersonLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            physicalPersonLicence.Licence.Code = reader["LicenceCode"].ToString();
                            physicalPersonLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            physicalPersonLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Country = new Country();
                            physicalPersonLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonLicence.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            physicalPersonLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            physicalPersonLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        physicalPersonLicence.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonLicence.CreatedBy = new User();
                            physicalPersonLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonLicence.Company = new Company();
                            physicalPersonLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonLicences.Add(physicalPersonLicence);
                    }
                }
            }
            return PhysicalPersonLicences;

            //List<PhysicalPersonLicence> PhysicalPersons = context.PhysicalPersonLicences
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public PhysicalPersonLicence Create(PhysicalPersonLicence PhysicalPersonLicence)
        {
            if (context.PhysicalPersonLicences.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonLicence.Identifier).Count() == 0)
            {
                PhysicalPersonLicence.Id = 0;

                PhysicalPersonLicence.Active = true;

                context.PhysicalPersonLicences.Add(PhysicalPersonLicence);
                return PhysicalPersonLicence;
            }
            else
            {
                // Load item that will be updated
                PhysicalPersonLicence dbEntry = context.PhysicalPersonLicences
                    .FirstOrDefault(x => x.Identifier == PhysicalPersonLicence.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.LicenceId = PhysicalPersonLicence.LicenceId ?? null;
                    dbEntry.CountryId = PhysicalPersonLicence.CountryId ?? null;
                    dbEntry.CompanyId = PhysicalPersonLicence.CompanyId ?? null;
                    dbEntry.CreatedById = PhysicalPersonLicence.CreatedById ?? null;

                    dbEntry.ValidFrom = PhysicalPersonLicence.ValidFrom;
                    dbEntry.ValidTo = PhysicalPersonLicence.ValidTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonLicence Delete(Guid identifier)
        {
            PhysicalPersonLicence dbEntry = context.PhysicalPersonLicences
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
