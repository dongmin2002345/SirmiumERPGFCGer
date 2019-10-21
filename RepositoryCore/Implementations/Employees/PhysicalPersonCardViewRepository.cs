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

namespace RepositoryCore.Implementations.PhysicalPersons
{
    public class PhysicalPersonCardViewRepository : IPhysicalPersonCardRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonCardViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<PhysicalPersonCard> GetPhysicalPersonCards(int companyId)
        {
            List<PhysicalPersonCard> PhysicalPersonCards = new List<PhysicalPersonCard>();

            string queryString =
                "SELECT PhysicalPersonCardId, PhysicalPersonCardIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "CardDate, Description, PlusMinus, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonCards " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonCard physicalPersonCard;
                    while (reader.Read())
                    {
                        physicalPersonCard = new PhysicalPersonCard();
                        physicalPersonCard.Id = Int32.Parse(reader["PhysicalPersonCardId"].ToString());
                        physicalPersonCard.Identifier = Guid.Parse(reader["PhysicalPersonCardIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonCard.PhysicalPerson = new PhysicalPerson();
                            physicalPersonCard.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonCard.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonCard.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["CardDate"] != DBNull.Value)
                            physicalPersonCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            physicalPersonCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != DBNull.Value)
                            physicalPersonCard.PlusMinus = reader["PlusMinus"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonCard.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonCard.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonCard.CreatedBy = new User();
                            physicalPersonCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonCard.Company = new Company();
                            physicalPersonCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonCards.Add(physicalPersonCard);
                    }
                }
            }

            //List<PhysicalPersonCard> PhysicalPersonCards = context.PhysicalPersonCards
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            return PhysicalPersonCards;
        }

        public List<PhysicalPersonCard> GetPhysicalPersonCardsByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonCard> PhysicalPersonCards = new List<PhysicalPersonCard>();

            string queryString =
                "SELECT PhysicalPersonCardId, PhysicalPersonCardIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "CardDate, Description, PlusMinus, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonCards " +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonCard physicalPersonCard;
                    while (reader.Read())
                    {
                        physicalPersonCard = new PhysicalPersonCard();
                        physicalPersonCard.Id = Int32.Parse(reader["PhysicalPersonCardId"].ToString());
                        physicalPersonCard.Identifier = Guid.Parse(reader["PhysicalPersonCardIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonCard.PhysicalPerson = new PhysicalPerson();
                            physicalPersonCard.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonCard.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonCard.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["CardDate"] != DBNull.Value)
                            physicalPersonCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            physicalPersonCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != DBNull.Value)
                            physicalPersonCard.PlusMinus = reader["PlusMinus"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonCard.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonCard.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonCard.CreatedBy = new User();
                            physicalPersonCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonCard.Company = new Company();
                            physicalPersonCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonCards.Add(physicalPersonCard);
                    }
                }
            }

            return PhysicalPersonCards;

            //List<PhysicalPersonCard> PhysicalPersons = context.PhysicalPersonCards
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public List<PhysicalPersonCard> GetPhysicalPersonCardsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonCard> PhysicalPersonCards = new List<PhysicalPersonCard>();

            string queryString =
                "SELECT PhysicalPersonCardId, PhysicalPersonCardIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "CardDate, Description, PlusMinus, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonCards " +
                "WHERE CompanyId = @CompanyId AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonCard physicalPersonCard;
                    while (reader.Read())
                    {
                        physicalPersonCard = new PhysicalPersonCard();
                        physicalPersonCard.Id = Int32.Parse(reader["PhysicalPersonCardId"].ToString());
                        physicalPersonCard.Identifier = Guid.Parse(reader["PhysicalPersonCardIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonCard.PhysicalPerson = new PhysicalPerson();
                            physicalPersonCard.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonCard.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonCard.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonCard.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["CardDate"] != DBNull.Value)
                            physicalPersonCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != DBNull.Value)
                            physicalPersonCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != DBNull.Value)
                            physicalPersonCard.PlusMinus = reader["PlusMinus"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonCard.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonCard.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonCard.CreatedBy = new User();
                            physicalPersonCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonCard.Company = new Company();
                            physicalPersonCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonCards.Add(physicalPersonCard);
                    }
                }
            }

            return PhysicalPersonCards;

            //List<PhysicalPersonCard> PhysicalPersons = context.PhysicalPersonCards
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public PhysicalPersonCard Create(PhysicalPersonCard PhysicalPersonCard)
        {
            if (context.PhysicalPersonCards.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonCard.Identifier).Count() == 0)
            {
                PhysicalPersonCard.Id = 0;

                PhysicalPersonCard.Active = true;
                PhysicalPersonCard.UpdatedAt = DateTime.Now;
                PhysicalPersonCard.CreatedAt = DateTime.Now;

                context.PhysicalPersonCards.Add(PhysicalPersonCard);
                return PhysicalPersonCard;
            }
            else
            {
                // Load item that will be updated
                PhysicalPersonCard dbEntry = context.PhysicalPersonCards
                    .FirstOrDefault(x => x.Identifier == PhysicalPersonCard.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = PhysicalPersonCard.CompanyId ?? null;
                    dbEntry.CreatedById = PhysicalPersonCard.CreatedById ?? null;

                    // Set properties
                    dbEntry.CardDate = PhysicalPersonCard.CardDate;
                    dbEntry.Description = PhysicalPersonCard.Description;
                    dbEntry.PlusMinus = PhysicalPersonCard.PlusMinus;
                    dbEntry.ItemStatus = PhysicalPersonCard.ItemStatus;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonCard Delete(Guid identifier)
        {
            PhysicalPersonCard dbEntry = context.PhysicalPersonCards
               .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonCard))
                   .Select(x => x.Entity as PhysicalPersonCard))
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
