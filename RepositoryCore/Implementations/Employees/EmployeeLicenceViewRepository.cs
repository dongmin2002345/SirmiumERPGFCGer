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

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeLicenceViewRepository : IEmployeeLicenceRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeLicenceViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeLicence> GetEmployeeItems(int companyId)  // izmene u IEmployeeLicenceRepository -> GetEmployeeLicences?
        {
            List<EmployeeLicence> EmployeeLicences = new List<EmployeeLicence>();

            string queryString =
                "SELECT EmployeeLicenceId, EmployeeLicenceIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeLicences " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeLicence employeeLicence;
                    while (reader.Read())
                    {
                        employeeLicence = new EmployeeLicence();
                        employeeLicence.Id = Int32.Parse(reader["EmployeeLicenceId"].ToString());
                        employeeLicence.Identifier = Guid.Parse(reader["EmployeeLicenceIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeLicence.Employee = new Employee();
                            employeeLicence.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeLicence.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeLicence.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            employeeLicence.Licence = new LicenceType();
                            employeeLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            employeeLicence.Licence.Code = reader["LicenceCode"].ToString();
                            employeeLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            employeeLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }
                        
                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeLicence.Country = new Country();
                            employeeLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeLicence.Country.Code = reader["CountryCode"].ToString();
                            employeeLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            employeeLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            employeeLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        employeeLicence.Active = bool.Parse(reader["Active"].ToString());
                        employeeLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeLicence.CreatedBy = new User();
                            employeeLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeLicence.Company = new Company();
                            employeeLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeLicences.Add(employeeLicence);
                    }
                }
            }
            return EmployeeLicences;

            //List<EmployeeLicence> EmployeeLicences = context.EmployeeLicences
            //    .Include(x => x.Employee)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return EmployeeLicences;
        }

        public List<EmployeeLicence> GetEmployeeItemsByEmployee(int EmployeeId) //GetEmployeeLicencesByEmployee?
        {
            List<EmployeeLicence> EmployeeLicences = new List<EmployeeLicence>();

            string queryString =
                "SELECT EmployeeLicenceId, EmployeeLicenceIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeLicences " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeLicence employeeLicence;
                    while (reader.Read())
                    {
                        employeeLicence = new EmployeeLicence();
                        employeeLicence.Id = Int32.Parse(reader["EmployeeLicenceId"].ToString());
                        employeeLicence.Identifier = Guid.Parse(reader["EmployeeLicenceIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeLicence.Employee = new Employee();
                            employeeLicence.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeLicence.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeLicence.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            employeeLicence.Licence = new LicenceType();
                            employeeLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            employeeLicence.Licence.Code = reader["LicenceCode"].ToString();
                            employeeLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            employeeLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeLicence.Country = new Country();
                            employeeLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeLicence.Country.Code = reader["CountryCode"].ToString();
                            employeeLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            employeeLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            employeeLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        employeeLicence.Active = bool.Parse(reader["Active"].ToString());
                        employeeLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeLicence.CreatedBy = new User();
                            employeeLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeLicence.Company = new Company();
                            employeeLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeLicences.Add(employeeLicence);
                    }
                }
            }
            return EmployeeLicences;

            //List<EmployeeLicence> Employees = context.EmployeeLicences
            //    .Include(x => x.Employee)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeLicence> GetEmployeeItemsNewerThan(int companyId, DateTime lastUpdateTime) //GetEmployeeLicencesNewerThan?
        {
            List<EmployeeLicence> EmployeeLicences = new List<EmployeeLicence>();

            string queryString =
                "SELECT EmployeeLicenceId, EmployeeLicenceIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "LicenceId, LicenceIdentifier, LicenceCode, LicenceCategory, LicenceDescription, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "ValidFrom, ValidTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeLicences " +
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
                    EmployeeLicence employeeLicence;
                    while (reader.Read())
                    {
                        employeeLicence = new EmployeeLicence();
                        employeeLicence.Id = Int32.Parse(reader["EmployeeLicenceId"].ToString());
                        employeeLicence.Identifier = Guid.Parse(reader["EmployeeLicenceIdentifier"].ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeLicence.Employee = new Employee();
                            employeeLicence.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeLicence.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeLicence.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeLicence.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["LicenceId"] != DBNull.Value)
                        {
                            employeeLicence.Licence = new LicenceType();
                            employeeLicence.LicenceId = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Id = Int32.Parse(reader["LicenceId"].ToString());
                            employeeLicence.Licence.Identifier = Guid.Parse(reader["LicenceIdentifier"].ToString());
                            employeeLicence.Licence.Code = reader["LicenceCode"].ToString();
                            employeeLicence.Licence.Category = reader["LicenceCategory"].ToString();
                            employeeLicence.Licence.Description = reader["LicenceDescription"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employeeLicence.Country = new Country();
                            employeeLicence.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employeeLicence.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employeeLicence.Country.Code = reader["CountryCode"].ToString();
                            employeeLicence.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["ValidFrom"] != DBNull.Value)
                            employeeLicence.ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString());
                        if (reader["ValidTo"] != DBNull.Value)
                            employeeLicence.ValidTo = DateTime.Parse(reader["ValidTo"].ToString());

                        employeeLicence.Active = bool.Parse(reader["Active"].ToString());
                        employeeLicence.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeLicence.CreatedBy = new User();
                            employeeLicence.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeLicence.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeLicence.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeLicence.Company = new Company();
                            employeeLicence.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeLicence.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeLicences.Add(employeeLicence);
                    }
                }
            }
            return EmployeeLicences;

            //List<EmployeeLicence> Employees = context.EmployeeLicences
            //    .Include(x => x.Employee)
            //    .Include(x => x.Licence)
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeLicence Create(EmployeeLicence EmployeeLicence)
        {
            if (context.EmployeeLicences.Where(x => x.Identifier != null && x.Identifier == EmployeeLicence.Identifier).Count() == 0)
            {
                EmployeeLicence.Id = 0;

                EmployeeLicence.Active = true;

                context.EmployeeLicences.Add(EmployeeLicence);
                return EmployeeLicence;
            }
            else
            {
                // Load item that will be updated
                EmployeeLicence dbEntry = context.EmployeeLicences
                    .FirstOrDefault(x => x.Identifier == EmployeeLicence.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.LicenceId = EmployeeLicence.LicenceId ?? null;
                    dbEntry.CountryId = EmployeeLicence.CountryId ?? null;
                    dbEntry.CompanyId = EmployeeLicence.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeLicence.CreatedById ?? null;

                    dbEntry.ValidFrom = EmployeeLicence.ValidFrom;
                    dbEntry.ValidTo = EmployeeLicence.ValidTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeLicence Delete(Guid identifier)
        {
            EmployeeLicence dbEntry = context.EmployeeLicences
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
