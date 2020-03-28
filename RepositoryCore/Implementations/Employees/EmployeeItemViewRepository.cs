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
    public class EmployeeItemViewRepository : IEmployeeItemRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeItemViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeItem> GetEmployeeItems(int companyId)
        {
            List<EmployeeItem> EmployeeItems = new List<EmployeeItem>();

            string queryString =
                "SELECT EmployeeItemId, EmployeeItemIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
                "WHERE CompanyId = @CompanyId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeItem employeeItem;
                    while (reader.Read())
                    {
                        employeeItem = new EmployeeItem();
                        employeeItem.Id = Int32.Parse(reader["EmployeeItemId"].ToString());
                        employeeItem.Identifier = Guid.Parse(reader["EmployeeItemIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeItem.Company = new Company();
                            employeeItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeItems.Add(employeeItem);
                    }
                }
            }
            return EmployeeItems;

            //List<EmployeeItem> EmployeeItems = context.EmployeeItems
            //    .Include(x => x.Employee)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return EmployeeItems;
        }

        public List<EmployeeItem> GetEmployeeItemsByEmployee(int EmployeeId)
        {
            List<EmployeeItem> EmployeeItems = new List<EmployeeItem>();

            string queryString =
                "SELECT EmployeeItemId, EmployeeItemIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
                "WHERE EmployeeId = @EmployeeId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeItem employeeItem;
                    while (reader.Read())
                    {
                        employeeItem = new EmployeeItem();
                        employeeItem.Id = Int32.Parse(reader["EmployeeItemId"].ToString());
                        employeeItem.Identifier = Guid.Parse(reader["EmployeeItemIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeItem.Company = new Company();
                            employeeItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeItems.Add(employeeItem);
                    }
                }
            }
            return EmployeeItems;

            //List<EmployeeItem> Employees = context.EmployeeItems
            //    .Include(x => x.Employee)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeItem> GetEmployeeItemsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeItem> EmployeeItems = new List<EmployeeItem>();

            string queryString =
                "SELECT EmployeeItemId, EmployeeItemIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "FamilyMemberId, FamilyMemberIdentifier, FamilyMemberCode, FamilyMemberName, " +
                "Name, DateOfBirth, EmbassyDate, ItemStatus, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
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
                    EmployeeItem employeeItem;
                    while (reader.Read())
                    {
                        employeeItem = new EmployeeItem();
                        employeeItem.Id = Int32.Parse(reader["EmployeeItemId"].ToString());
                        employeeItem.Identifier = Guid.Parse(reader["EmployeeItemIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != DBNull.Value)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeItem.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeItem.Company = new Company();
                            employeeItem.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeItem.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeItems.Add(employeeItem);
                    }
                }
            }
            return EmployeeItems;

            //List<EmployeeItem> Employees = context.EmployeeItems
            //    .Include(x => x.Employee)
            //    .Include(x => x.FamilyMember)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeItem Create(EmployeeItem EmployeeItem)
        {
            if (context.EmployeeItems.Where(x => x.Identifier != null && x.Identifier == EmployeeItem.Identifier).Count() == 0)
            {
                EmployeeItem.Id = 0;

                EmployeeItem.Active = true;
                EmployeeItem.UpdatedAt = DateTime.Now;
                EmployeeItem.CreatedAt = DateTime.Now;
                context.EmployeeItems.Add(EmployeeItem);
                return EmployeeItem;
            }
            else
            {
                // Load item that will be updated
                EmployeeItem dbEntry = context.EmployeeItems
                    .FirstOrDefault(x => x.Identifier == EmployeeItem.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.FamilyMemberId = EmployeeItem.FamilyMemberId ?? null;
                    dbEntry.CompanyId = EmployeeItem.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeItem.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = EmployeeItem.Name;
                    dbEntry.DateOfBirth = EmployeeItem.DateOfBirth;
                    dbEntry.EmbassyDate = EmployeeItem.EmbassyDate;
                    dbEntry.ItemStatus = EmployeeItem.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeItem Delete(Guid identifier)
        {
            EmployeeItem dbEntry = context.EmployeeItems
                .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeItem))
                   .Select(x => x.Entity as EmployeeItem))
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
