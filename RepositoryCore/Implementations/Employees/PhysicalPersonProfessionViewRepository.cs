using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Professions;
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
    public class PhysicalPersonProfessionViewRepository : IPhysicalPersonProfessionRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonProfessionViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<PhysicalPersonProfession> GetPhysicalPersonItems(int companyId)
        {
            List<PhysicalPersonProfession> PhysicalPersonProfessions = new List<PhysicalPersonProfession>();

            string queryString =
                "SELECT PhysicalPersonProfessionId, PhysicalPersonProfessionIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonProfessions " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonProfession physicalPersonProfession;
                    while (reader.Read())
                    {
                        physicalPersonProfession = new PhysicalPersonProfession();
                        physicalPersonProfession.Id = Int32.Parse(reader["PhysicalPersonProfessionId"].ToString());
                        physicalPersonProfession.Identifier = Guid.Parse(reader["PhysicalPersonProfessionIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonProfession.PhysicalPerson = new PhysicalPerson();
                            physicalPersonProfession.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonProfession.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonProfession.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Profession = new Profession();
                            physicalPersonProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            physicalPersonProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            physicalPersonProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Country = new Country();
                            physicalPersonProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonProfession.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonProfession.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonProfession.CreatedBy = new User();
                            physicalPersonProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Company = new Company();
                            physicalPersonProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonProfessions.Add(physicalPersonProfession);
                    }
                }
            }
            return PhysicalPersonProfessions;


            //List<PhysicalPersonProfession> PhysicalPersonProfessions = context.PhysicalPersonProfessions
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersonProfessions;
        }

        public List<PhysicalPersonProfession> GetPhysicalPersonItemsByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonProfession> PhysicalPersonProfessions = new List<PhysicalPersonProfession>();

            string queryString =
                "SELECT PhysicalPersonProfessionId, PhysicalPersonProfessionIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonProfessions " +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonProfession physicalPersonProfession;
                    while (reader.Read())
                    {
                        physicalPersonProfession = new PhysicalPersonProfession();
                        physicalPersonProfession.Id = Int32.Parse(reader["PhysicalPersonProfessionId"].ToString());
                        physicalPersonProfession.Identifier = Guid.Parse(reader["PhysicalPersonProfessionIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonProfession.PhysicalPerson = new PhysicalPerson();
                            physicalPersonProfession.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonProfession.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonProfession.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Profession = new Profession();
                            physicalPersonProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            physicalPersonProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            physicalPersonProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Country = new Country();
                            physicalPersonProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonProfession.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonProfession.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonProfession.CreatedBy = new User();
                            physicalPersonProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Company = new Company();
                            physicalPersonProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonProfessions.Add(physicalPersonProfession);
                    }
                }
            }
            return PhysicalPersonProfessions;

            //List<PhysicalPersonProfession> PhysicalPersons = context.PhysicalPersonProfessions
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public List<PhysicalPersonProfession> GetPhysicalPersonItemsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonProfession> PhysicalPersonProfessions = new List<PhysicalPersonProfession>();

            string queryString =
                "SELECT PhysicalPersonProfessionId, PhysicalPersonProfessionIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersonProfessions " +
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
                    PhysicalPersonProfession physicalPersonProfession;
                    while (reader.Read())
                    {
                        physicalPersonProfession = new PhysicalPersonProfession();
                        physicalPersonProfession.Id = Int32.Parse(reader["PhysicalPersonProfessionId"].ToString());
                        physicalPersonProfession.Identifier = Guid.Parse(reader["PhysicalPersonProfessionIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonProfession.PhysicalPerson = new PhysicalPerson();
                            physicalPersonProfession.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonProfession.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonProfession.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonProfession.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Profession = new Profession();
                            physicalPersonProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            physicalPersonProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            physicalPersonProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            physicalPersonProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Country = new Country();
                            physicalPersonProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPersonProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPersonProfession.Country.Code = reader["CountryCode"].ToString();
                            physicalPersonProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonProfession.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonProfession.CreatedBy = new User();
                            physicalPersonProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonProfession.Company = new Company();
                            physicalPersonProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonProfessions.Add(physicalPersonProfession);
                    }
                }
            }
            return PhysicalPersonProfessions;

            //List<PhysicalPersonProfession> PhysicalPersons = context.PhysicalPersonProfessions
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public PhysicalPersonProfession Create(PhysicalPersonProfession PhysicalPersonProfession)
        {
            if (context.PhysicalPersonProfessions.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonProfession.Identifier).Count() == 0)
            {
                PhysicalPersonProfession.Id = 0;

                PhysicalPersonProfession.Active = true;
                PhysicalPersonProfession.UpdatedAt = DateTime.Now;
                PhysicalPersonProfession.CreatedAt = DateTime.Now;

                context.PhysicalPersonProfessions.Add(PhysicalPersonProfession);
                return PhysicalPersonProfession;
            }
            else
            {
                // Load item that will be updated
                PhysicalPersonProfession dbEntry = context.PhysicalPersonProfessions
                    .FirstOrDefault(x => x.Identifier == PhysicalPersonProfession.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.ProfessionId = PhysicalPersonProfession.ProfessionId ?? null;
                    dbEntry.CountryId = PhysicalPersonProfession.CountryId ?? null;
                    dbEntry.CompanyId = PhysicalPersonProfession.CompanyId ?? null;
                    dbEntry.CreatedById = PhysicalPersonProfession.CreatedById ?? null;

                    dbEntry.ItemStatus = PhysicalPersonProfession.ItemStatus;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonProfession Delete(Guid identifier)
        {
            PhysicalPersonProfession dbEntry = context.PhysicalPersonProfessions
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonProfession))
                    .Select(x => x.Entity as PhysicalPersonProfession))
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
