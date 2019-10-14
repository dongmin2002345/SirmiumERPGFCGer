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

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeProfessionViewRepository : IEmployeeProfessionRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeProfessionViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeProfession> GetEmployeeItems(int companyId)
        {
            List<EmployeeProfession> EmployeeProfessions = new List<EmployeeProfession>();

            string queryString =
                "SELECT EmployeeProfessionId, EmployeeProfessionIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeProfessions " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeProfession employeeProfession;
                    while (reader.Read())
                    {
                        employeeProfession = new EmployeeProfession();
                        employeeProfession.Id = Int32.Parse(reader["EmployeeProfessionId"].ToString());
                        employeeProfession.Identifier = Guid.Parse(reader["EmployeeProfessionIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeProfession.Employee = new Employee();
                            employeeProfession.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeProfession.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeProfession.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            employeeProfession.Profession = new Profession();
                            employeeProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            employeeProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            employeeProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeProfession.Country = new Country();
                            employeeProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeProfession.Country.Code = reader["CountryCode"].ToString();
                            employeeProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeProfession.Active = bool.Parse(reader["Active"].ToString());
                        employeeProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeProfession.CreatedBy = new User();
                            employeeProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeProfession.Company = new Company();
                            employeeProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeProfessions.Add(employeeProfession);
                    }
                }
            }
            return EmployeeProfessions;


            //List<EmployeeProfession> EmployeeProfessions = context.EmployeeProfessions
            //    .Include(x => x.Employee)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return EmployeeProfessions;
        }

        public List<EmployeeProfession> GetEmployeeItemsByEmployee(int EmployeeId)
        {
            List<EmployeeProfession> EmployeeProfessions = new List<EmployeeProfession>();

            string queryString =
                "SELECT EmployeeProfessionId, EmployeeProfessionIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeProfessions " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeProfession employeeProfession;
                    while (reader.Read())
                    {
                        employeeProfession = new EmployeeProfession();
                        employeeProfession.Id = Int32.Parse(reader["EmployeeProfessionId"].ToString());
                        employeeProfession.Identifier = Guid.Parse(reader["EmployeeProfessionIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeProfession.Employee = new Employee();
                            employeeProfession.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeProfession.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeProfession.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            employeeProfession.Profession = new Profession();
                            employeeProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            employeeProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            employeeProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeProfession.Country = new Country();
                            employeeProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeProfession.Country.Code = reader["CountryCode"].ToString();
                            employeeProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeProfession.Active = bool.Parse(reader["Active"].ToString());
                        employeeProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeProfession.CreatedBy = new User();
                            employeeProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeProfession.Company = new Company();
                            employeeProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeProfessions.Add(employeeProfession);
                    }
                }
            }
            return EmployeeProfessions;

            //List<EmployeeProfession> Employees = context.EmployeeProfessions
            //    .Include(x => x.Employee)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeProfession> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeProfession> EmployeeProfessions = new List<EmployeeProfession>();

            string queryString =
                "SELECT EmployeeProfessionId, EmployeeProfessionIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeProfessions " +
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
                    EmployeeProfession employeeProfession;
                    while (reader.Read())
                    {
                        employeeProfession = new EmployeeProfession();
                        employeeProfession.Id = Int32.Parse(reader["EmployeeProfessionId"].ToString());
                        employeeProfession.Identifier = Guid.Parse(reader["EmployeeProfessionIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeProfession.Employee = new Employee();
                            employeeProfession.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeProfession.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeProfession.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeProfession.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["ProfessionId"] != DBNull.Value)
                        {
                            employeeProfession.Profession = new Profession();
                            employeeProfession.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Id = Int32.Parse(reader["ProfessionId"].ToString());
                            employeeProfession.Profession.Identifier = Guid.Parse(reader["ProfessionIdentifier"].ToString());
                            employeeProfession.Profession.Code = reader["ProfessionCode"].ToString();
                            employeeProfession.Profession.Name = reader["ProfessionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeProfession.Country = new Country();
                            employeeProfession.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeProfession.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeProfession.Country.Code = reader["CountryCode"].ToString();
                            employeeProfession.Country.Name = reader["CountryName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            employeeProfession.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        employeeProfession.Active = bool.Parse(reader["Active"].ToString());
                        employeeProfession.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeProfession.CreatedBy = new User();
                            employeeProfession.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeProfession.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeProfession.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeProfession.Company = new Company();
                            employeeProfession.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeProfession.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeProfessions.Add(employeeProfession);
                    }
                }
            }
            return EmployeeProfessions;

            //List<EmployeeProfession> Employees = context.EmployeeProfessions
            //    .Include(x => x.Employee)
            //    .Include(x => x.Profession)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeProfession Create(EmployeeProfession EmployeeProfession)
        {
            if (context.EmployeeProfessions.Where(x => x.Identifier != null && x.Identifier == EmployeeProfession.Identifier).Count() == 0)
            {
                EmployeeProfession.Id = 0;

                EmployeeProfession.Active = true;
                EmployeeProfession.UpdatedAt = DateTime.Now;
                EmployeeProfession.CreatedAt = DateTime.Now;
                context.EmployeeProfessions.Add(EmployeeProfession);
                return EmployeeProfession;
            }
            else
            {
                // Load item that will be updated
                EmployeeProfession dbEntry = context.EmployeeProfessions
                    .FirstOrDefault(x => x.Identifier == EmployeeProfession.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.ProfessionId = EmployeeProfession.ProfessionId ?? null;
                    dbEntry.CountryId = EmployeeProfession.CountryId ?? null;
                    dbEntry.CompanyId = EmployeeProfession.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeProfession.CreatedById ?? null;
                    dbEntry.ItemStatus = EmployeeProfession.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeProfession Delete(Guid identifier)
        {
            EmployeeProfession dbEntry = context.EmployeeProfessions
               .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeProfession))
                   .Select(x => x.Entity as EmployeeProfession))
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
