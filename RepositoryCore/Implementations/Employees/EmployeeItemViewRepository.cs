﻿using Configurator;
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
                "Name, DateOfBirth, EmbassyDate, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

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

                        if (reader["EmployeeId"] != null)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeItemTypeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != null)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != null)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != null)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());

                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "Name, DateOfBirth, EmbassyDate, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

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

                        if (reader["EmployeeId"] != null)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeItemTypeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != null)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != null)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != null)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());

                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "Name, DateOfBirth, EmbassyDate, " + //Passport??? ima u EmployeeItemSQLite tabeli (vEmployeeItems)
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeItems " +
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
                    EmployeeItem employeeItem;
                    while (reader.Read())
                    {
                        employeeItem = new EmployeeItem();
                        employeeItem.Id = Int32.Parse(reader["EmployeeItemId"].ToString());
                        employeeItem.Identifier = Guid.Parse(reader["EmployeeItemIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeItem.Employee = new Employee();
                            employeeItem.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeItem.Employee.Identifier = Guid.Parse(reader["EmployeeItemTypeIdentifier"].ToString());
                            employeeItem.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeItem.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["FamilyMemberId"] != null)
                        {
                            employeeItem.FamilyMember = new FamilyMember();
                            employeeItem.FamilyMemberId = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Id = Int32.Parse(reader["FamilyMemberId"].ToString());
                            employeeItem.FamilyMember.Identifier = Guid.Parse(reader["FamilyMemberIdentifier"].ToString());
                            employeeItem.FamilyMember.Code = reader["FamilyMemberCode"].ToString();
                            employeeItem.FamilyMember.Name = reader["FamilyMemberName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeItem.Name = reader["Name"].ToString();
                        if (reader["DateOfBirth"] != null)
                            employeeItem.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                        if (reader["EmbassyDate"] != null)
                            employeeItem.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());

                        employeeItem.Active = bool.Parse(reader["Active"].ToString());
                        employeeItem.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeItem.CreatedBy = new User();
                            employeeItem.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeItem.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeItem.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeItem Delete(Guid identifier)
        {
            EmployeeItem dbEntry = context.EmployeeItems
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