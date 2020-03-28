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
    public class PhysicalPersonItemViewRepository : IPhysicalPersonItemRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonItemViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<PhysicalPersonItem> GetPhysicalPersonItems(int companyId)
        {
            List<PhysicalPersonItem> PhysicalPersonItems = new List<PhysicalPersonItem>();

            string queryString =
                "SELECT PhysicalPersonItemId, PhysicalPersonItemIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + 
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonItems " +
                "WHERE CompanyId = @CompanyId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonItem physicalPersonItem;
                    while (reader.Read())
                    {
                        physicalPersonItem = new PhysicalPersonItem();
                        physicalPersonItem.Id = Int32.Parse(reader["PhysicalPersonItemId"].ToString());
                        physicalPersonItem.Identifier = Guid.Parse(reader["PhysicalPersonItemIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonItem.PhysicalPerson = new PhysicalPerson();
                            physicalPersonItem.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonItem.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonItem.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            physicalPersonItem.FamilyMember = new FamilyMember();
                            physicalPersonItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            physicalPersonItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            physicalPersonItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            physicalPersonItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPersonItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPersonItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonItem.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonItem.CreatedBy = new User();
                            physicalPersonItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonItem.Company = new Company();
                            physicalPersonItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonItems.Add(physicalPersonItem);
                    }
                }
            }
            return PhysicalPersonItems;

            //List<PhysicalPersonItem> PhysicalPersonItems = context.PhysicalPersonItems
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersonItems;
        }

        public List<PhysicalPersonItem> GetPhysicalPersonItemsByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonItem> PhysicalPersonItems = new List<PhysicalPersonItem>();

            string queryString =
                "SELECT PhysicalPersonItemId, PhysicalPersonItemIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + //Passport??? ima u PhysicalPersonItemSQLite tabeli (vPhysicalPersonItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonItems " +
                "WHERE PhysicalPersonId = @PhysicalPersonId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonItem physicalPersonItem;
                    while (reader.Read())
                    {
                        physicalPersonItem = new PhysicalPersonItem();
                        physicalPersonItem.Id = Int32.Parse(reader["PhysicalPersonItemId"].ToString());
                        physicalPersonItem.Identifier = Guid.Parse(reader["PhysicalPersonItemIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonItem.PhysicalPerson = new PhysicalPerson();
                            physicalPersonItem.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonItem.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonItem.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            physicalPersonItem.FamilyMember = new FamilyMember();
                            physicalPersonItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            physicalPersonItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            physicalPersonItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            physicalPersonItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPersonItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPersonItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonItem.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonItem.CreatedBy = new User();
                            physicalPersonItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonItem.Company = new Company();
                            physicalPersonItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonItems.Add(physicalPersonItem);
                    }
                }
            }
            return PhysicalPersonItems;

            //List<PhysicalPersonItem> PhysicalPersons = context.PhysicalPersonItems
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public List<PhysicalPersonItem> GetPhysicalPersonItemsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonItem> PhysicalPersonItems = new List<PhysicalPersonItem>();

            string queryString =
                "SELECT PhysicalPersonItemId, PhysicalPersonItemIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + //Passport??? ima u PhysicalPersonItemSQLite tabeli (vPhysicalPersonItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonItems " +
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
                    PhysicalPersonItem physicalPersonItem;
                    while (reader.Read())
                    {
                        physicalPersonItem = new PhysicalPersonItem();
                        physicalPersonItem.Id = Int32.Parse(reader["PhysicalPersonItemId"].ToString());
                        physicalPersonItem.Identifier = Guid.Parse(reader["PhysicalPersonItemIdentifier"].ToString());

                        if (reader["PhysicalPersonId"] != DBNull.Value)
                        {
                            physicalPersonItem.PhysicalPerson = new PhysicalPerson();
                            physicalPersonItem.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                            physicalPersonItem.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                            physicalPersonItem.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                            physicalPersonItem.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            physicalPersonItem.FamilyMember = new FamilyMember();
                            physicalPersonItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            physicalPersonItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            physicalPersonItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            physicalPersonItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            physicalPersonItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPersonItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPersonItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            physicalPersonItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        physicalPersonItem.Active = bool.Parse(reader["Active"].ToString());
                        physicalPersonItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPersonItem.CreatedBy = new User();
                            physicalPersonItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPersonItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPersonItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPersonItem.Company = new Company();
                            physicalPersonItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPersonItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersonItems.Add(physicalPersonItem);
                    }
                }
            }
            return PhysicalPersonItems;

            //List<PhysicalPersonItem> PhysicalPersons = context.PhysicalPersonItems
            //    .Include(x => x.PhysicalPerson)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return PhysicalPersons;
        }

        public PhysicalPersonItem Create(PhysicalPersonItem PhysicalPersonItem)
        {
            if (context.PhysicalPersonItems.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonItem.Identifier).Count() == 0)
            {
                PhysicalPersonItem.Id = 0;

                PhysicalPersonItem.Active = true;
                PhysicalPersonItem.UpdatedAt = DateTime.Now;
                PhysicalPersonItem.CreatedAt = DateTime.Now;

                context.PhysicalPersonItems.Add(PhysicalPersonItem);
                return PhysicalPersonItem;
            }
            else
            {
                // Load item that will be updated
                PhysicalPersonItem dbEntry = context.PhysicalPersonItems
                    .FirstOrDefault(x => x.Identifier == PhysicalPersonItem.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.FamilyMemberId = PhysicalPersonItem.FamilyMemberId ?? null;
                    dbEntry.CompanyId = PhysicalPersonItem.CompanyId ?? null;
                    dbEntry.CreatedById = PhysicalPersonItem.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = PhysicalPersonItem.Name;
                    dbEntry.DateOfBirth = PhysicalPersonItem.DateOfBirth;
                    dbEntry.EmbassyDate = PhysicalPersonItem.EmbassyDate;
                    dbEntry.ItemStatus = PhysicalPersonItem.ItemStatus;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonItem Delete(Guid identifier)
        {
            PhysicalPersonItem dbEntry = context.PhysicalPersonItems
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonItem))
                    .Select(x => x.Entity as PhysicalPersonItem))
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
