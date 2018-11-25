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
    public class EmployeeDocumentViewRepository : IEmployeeDocumentRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeDocument> GetEmployeeDocuments(int companyId)
        {
            List<EmployeeDocument> EmployeeDocuments = new List<EmployeeDocument>();

            string queryString =
                "SELECT EmployeeDocumentId, EmployeeDocumentIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeDocuments " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeDocument employeeDocument;
                    while (reader.Read())
                    {
                        employeeDocument = new EmployeeDocument();
                        employeeDocument.Id = Int32.Parse(reader["EmployeeDocumentId"].ToString());
                        employeeDocument.Identifier = Guid.Parse(reader["EmployeeDocumentIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeDocument.Employee = new Employee();
                            employeeDocument.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Identifier = Guid.Parse(reader["EmployeeDocumentTypeIdentifier"].ToString());
                            employeeDocument.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeDocument.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            employeeDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            employeeDocument.Path = reader["Path"].ToString();

                        employeeDocument.Active = bool.Parse(reader["Active"].ToString());
                        employeeDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeDocument.CreatedBy = new User();
                            employeeDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeDocument.Company = new Company();
                            employeeDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeDocuments.Add(employeeDocument);
                    }
                }
            }
            return EmployeeDocuments;

            //List<EmployeeDocument> EmployeeDocuments = context.EmployeeDocuments
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return EmployeeDocuments;
        }

        public List<EmployeeDocument> GetEmployeeDocumentsByEmployee(int EmployeeId)
        {
            List<EmployeeDocument> EmployeeDocuments = new List<EmployeeDocument>();

            string queryString =
                "SELECT EmployeeDocumentId, EmployeeDocumentIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeDocuments " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeDocument employeeDocument;
                    while (reader.Read())
                    {
                        employeeDocument = new EmployeeDocument();
                        employeeDocument.Id = Int32.Parse(reader["EmployeeDocumentId"].ToString());
                        employeeDocument.Identifier = Guid.Parse(reader["EmployeeDocumentIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeDocument.Employee = new Employee();
                            employeeDocument.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Identifier = Guid.Parse(reader["EmployeeDocumentTypeIdentifier"].ToString());
                            employeeDocument.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeDocument.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            employeeDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            employeeDocument.Path = reader["Path"].ToString();

                        employeeDocument.Active = bool.Parse(reader["Active"].ToString());
                        employeeDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeDocument.CreatedBy = new User();
                            employeeDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeDocument.Company = new Company();
                            employeeDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeDocuments.Add(employeeDocument);
                    }
                }
            }

            return EmployeeDocuments;

            //List<EmployeeDocument> Employees = context.EmployeeDocuments
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeDocument> GetEmployeeDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeDocument> EmployeeDocuments = new List<EmployeeDocument>();

            string queryString =
                "SELECT EmployeeDocumentId, EmployeeDocumentIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeDocuments " +
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
                    EmployeeDocument employeeDocument;
                    while (reader.Read())
                    {
                        employeeDocument = new EmployeeDocument();
                        employeeDocument.Id = Int32.Parse(reader["EmployeeDocumentId"].ToString());
                        employeeDocument.Identifier = Guid.Parse(reader["EmployeeDocumentIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeDocument.Employee = new Employee();
                            employeeDocument.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeDocument.Employee.Identifier = Guid.Parse(reader["EmployeeDocumentTypeIdentifier"].ToString());
                            employeeDocument.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeDocument.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["Name"] != null)
                            employeeDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            employeeDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            employeeDocument.Path = reader["Path"].ToString();

                        employeeDocument.Active = bool.Parse(reader["Active"].ToString());
                        employeeDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeDocument.CreatedBy = new User();
                            employeeDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeDocument.Company = new Company();
                            employeeDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeDocuments.Add(employeeDocument);
                    }
                }
            }
            return EmployeeDocuments;

            //List<EmployeeDocument> Employees = context.EmployeeDocuments
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeDocument Create(EmployeeDocument EmployeeDocument)
        {
            if (context.EmployeeDocuments.Where(x => x.Identifier != null && x.Identifier == EmployeeDocument.Identifier).Count() == 0)
            {
                EmployeeDocument.Id = 0;

                EmployeeDocument.Active = true;

                context.EmployeeDocuments.Add(EmployeeDocument);
                return EmployeeDocument;
            }
            else
            {
                // Load item that will be updated
                EmployeeDocument dbEntry = context.EmployeeDocuments
                    .FirstOrDefault(x => x.Identifier == EmployeeDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeDocument.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = EmployeeDocument.Name;
                    dbEntry.CreateDate = EmployeeDocument.CreateDate;
                    dbEntry.Path = EmployeeDocument.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeDocument Delete(Guid identifier)
        {
            EmployeeDocument dbEntry = context.EmployeeDocuments
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
