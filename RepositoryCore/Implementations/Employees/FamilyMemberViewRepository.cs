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

namespace RepositoryCore.Implementations.Employees
{
    public class FamilyMemberViewRepository : IFamilyMemberRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public FamilyMemberViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<FamilyMember> GetFamilyMembers(int companyId)
        {
            List<FamilyMember> FamilyMembers = new List<FamilyMember>();

            string queryString =
                "SELECT FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vFamilyMembers " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    FamilyMember familyMember;
                    while (reader.Read())
                    {
                        familyMember = new FamilyMember();
                        familyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                        familyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                        familyMember.Code = reader["FamilyMemberCode"].ToString();
                        familyMember.Name = reader["FamilyMemberName"].ToString();

                        familyMember.Active = bool.Parse(reader["Active"].ToString());
                        familyMember.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            familyMember.CreatedBy = new User();
                            familyMember.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            familyMember.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            familyMember.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            familyMember.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            familyMember.Company = new Company();
                            familyMember.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            familyMember.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            familyMember.Company.Name = reader["CompanyName"].ToString();
                        }

                        FamilyMembers.Add(familyMember);
                    }
                }
            }
            return FamilyMembers;

            //List<FamilyMember> cities = context.FamilyMembers
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return cities;
        }

        public List<FamilyMember> GetFamilyMembersNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<FamilyMember> FamilyMembers = new List<FamilyMember>();

            string queryString =
                "SELECT FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vFamilyMembers " +
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
                    FamilyMember familyMember;
                    while (reader.Read())
                    {
                        familyMember = new FamilyMember();
                        familyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                        familyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                        familyMember.Code = reader["FamilyMemberCode"].ToString();
                        familyMember.Name = reader["FamilyMemberName"].ToString();

                        familyMember.Active = bool.Parse(reader["Active"].ToString());
                        familyMember.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            familyMember.CreatedBy = new User();
                            familyMember.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            familyMember.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            familyMember.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            familyMember.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            familyMember.Company = new Company();
                            familyMember.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            familyMember.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            familyMember.Company.Name = reader["CompanyName"].ToString();
                        }

                        FamilyMembers.Add(familyMember);
                    }
                }
            }
            return FamilyMembers;

            //List<FamilyMember> cities = context.FamilyMembers
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return cities;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.FamilyMembers
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(FamilyMember))
                    .Select(x => x.Entity as FamilyMember))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "CLAN-00001";
            else
            {
                string activeCode = context.FamilyMembers
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(FamilyMember))
                        .Select(x => x.Entity as FamilyMember))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("CLAN-", ""));
                    return "CLAN-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public FamilyMember Create(FamilyMember familyMember)
        {
            if (context.FamilyMembers.Where(x => x.Identifier != null && x.Identifier == familyMember.Identifier).Count() == 0)
            {
                familyMember.Id = 0;

                familyMember.Code = GetNewCodeValue(familyMember.CompanyId ?? 0);
                familyMember.Active = true;

                familyMember.UpdatedAt = DateTime.Now;
                familyMember.CreatedAt = DateTime.Now;

                context.FamilyMembers.Add(familyMember);
                return familyMember;
            }
            else
            {
                // Load remedy that will be updated
                FamilyMember dbEntry = context.FamilyMembers
                .FirstOrDefault(x => x.Identifier == familyMember.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = familyMember.CompanyId ?? null;
                    dbEntry.CreatedById = familyMember.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = familyMember.Code;
                    dbEntry.Name = familyMember.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public FamilyMember Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            FamilyMember dbEntry = context.FamilyMembers
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
