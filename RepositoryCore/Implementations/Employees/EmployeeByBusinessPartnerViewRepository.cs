using Configurator;
using DomainCore.Common.BusinessPartners;
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
    public class EmployeeByBusinessPartnerViewRepository : IEmployeeByBusinessPartnerRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public EmployeeByBusinessPartnerViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartners(int companyId)
        {
            List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = new List<EmployeeByBusinessPartner>();

            string queryString =
                "SELECT EmployeeByBusinessPartnerId, EmployeeByBusinessPartnerIdentifier, EmployeeByBusinessPartnerCode, StartDate, EndDate, RealEndDate, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "EmployeeCount, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeByBusinessPartners " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeByBusinessPartner employeeByBusinessPartner;
                    while (reader.Read())
                    {
                        employeeByBusinessPartner = new EmployeeByBusinessPartner();
                        employeeByBusinessPartner.Id = Int32.Parse(reader["EmployeeByBusinessPartnerId"].ToString());
                        employeeByBusinessPartner.Identifier = Guid.Parse(reader["EmployeeByBusinessPartnerIdentifier"].ToString());
                        employeeByBusinessPartner.Code = reader["EmployeeByBusinessPartnerCode"].ToString();
                        if (reader["StartDate"] != DBNull.Value)
                            employeeByBusinessPartner.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        if (reader["EndDate"] != DBNull.Value)
                            employeeByBusinessPartner.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        if (reader["RealEndDate"] != DBNull.Value)
                            employeeByBusinessPartner.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.Employee = new Employee();
                            employeeByBusinessPartner.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByBusinessPartner.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByBusinessPartner.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeByBusinessPartner.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeByBusinessPartner.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["EmployeeCount"] != DBNull.Value)
                            employeeByBusinessPartner.EmployeeCount = Int32.Parse(reader["EmployeeCount"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.BusinessPartner = new BusinessPartner();
                            employeeByBusinessPartner.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            employeeByBusinessPartner.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        employeeByBusinessPartner.Active = bool.Parse(reader["Active"].ToString());
                        employeeByBusinessPartner.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.CreatedBy = new User();
                            employeeByBusinessPartner.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByBusinessPartner.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByBusinessPartner.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeByBusinessPartner.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.Company = new Company();
                            employeeByBusinessPartner.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByBusinessPartner.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByBusinessPartner.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeByBusinessPartners.Add(employeeByBusinessPartner);
                    }
                }
            }

            //List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = context.EmployeeByBusinessPartners
            //    .Include(x => x.Employee)
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return EmployeeByBusinessPartners;
        }

        public List<EmployeeByBusinessPartner> GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = new List<EmployeeByBusinessPartner>();

            string queryString =
                "SELECT EmployeeByBusinessPartnerId, EmployeeByBusinessPartnerIdentifier, EmployeeByBusinessPartnerCode, StartDate, EndDate, RealEndDate, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "EmployeeCount, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployeeByBusinessPartners " +
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
                    EmployeeByBusinessPartner employeeByBusinessPartner;
                    while (reader.Read())
                    {
                        employeeByBusinessPartner = new EmployeeByBusinessPartner();
                        employeeByBusinessPartner.Id = Int32.Parse(reader["EmployeeByBusinessPartnerId"].ToString());
                        employeeByBusinessPartner.Identifier = Guid.Parse(reader["EmployeeByBusinessPartnerIdentifier"].ToString());
                        employeeByBusinessPartner.Code = reader["EmployeeByBusinessPartnerCode"].ToString();
                        if (reader["StartDate"] != DBNull.Value)
                            employeeByBusinessPartner.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        if (reader["EndDate"] != DBNull.Value)
                            employeeByBusinessPartner.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                        if (reader["RealEndDate"] != DBNull.Value)
                            employeeByBusinessPartner.RealEndDate = DateTime.Parse(reader["RealEndDate"]?.ToString());

                        if (reader["EmployeeId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.Employee = new Employee();
                            employeeByBusinessPartner.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByBusinessPartner.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeByBusinessPartner.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                            employeeByBusinessPartner.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeByBusinessPartner.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["EmployeeCount"] != DBNull.Value)
                            employeeByBusinessPartner.EmployeeCount = Int32.Parse(reader["EmployeeCount"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.BusinessPartner = new BusinessPartner();
                            employeeByBusinessPartner.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            employeeByBusinessPartner.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            employeeByBusinessPartner.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        employeeByBusinessPartner.Active = bool.Parse(reader["Active"].ToString());
                        employeeByBusinessPartner.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.CreatedBy = new User();
                            employeeByBusinessPartner.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByBusinessPartner.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeByBusinessPartner.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeByBusinessPartner.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employeeByBusinessPartner.Company = new Company();
                            employeeByBusinessPartner.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByBusinessPartner.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeByBusinessPartner.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeByBusinessPartners.Add(employeeByBusinessPartner);
                    }
                }
            }

            //List<EmployeeByBusinessPartner> EmployeeByBusinessPartners = context.EmployeeByBusinessPartners
            //    .Include(x => x.Employee)
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            return EmployeeByBusinessPartners;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.EmployeeByBusinessPartners
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartner))
                    .Select(x => x.Entity as EmployeeByBusinessPartner))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-BP-00001";
            else
            {
                string activeCode = context.EmployeeByBusinessPartners
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeByBusinessPartner))
                        .Select(x => x.Entity as EmployeeByBusinessPartner))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("EMP-BY-BP-", ""));
                    return "EMP-BY-BP-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public EmployeeByBusinessPartner Create(EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            if (context.EmployeeByBusinessPartners.Where(x => x.Identifier != null && x.Identifier == employeeByBusinessPartner.Identifier).Count() == 0)
            {
                employeeByBusinessPartner.Id = 0;

                employeeByBusinessPartner.Code = GetNewCodeValue(employeeByBusinessPartner.CompanyId ?? 0);
                employeeByBusinessPartner.Active = true;

                employeeByBusinessPartner.UpdatedAt = DateTime.Now;
                employeeByBusinessPartner.CreatedAt = DateTime.Now;

                context.EmployeeByBusinessPartners.Add(employeeByBusinessPartner);
                return employeeByBusinessPartner;
            }
            else
            {
                // Load employeeByBusinessPartner that will be updated
                EmployeeByBusinessPartner dbEntry = context.EmployeeByBusinessPartners
                .FirstOrDefault(x => x.Identifier == employeeByBusinessPartner.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.EmployeeId = employeeByBusinessPartner.EmployeeId ?? null;
                    dbEntry.BusinessPartnerId = employeeByBusinessPartner.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = employeeByBusinessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = employeeByBusinessPartner.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employeeByBusinessPartner.Code;
                    dbEntry.StartDate = employeeByBusinessPartner.StartDate;
                    dbEntry.EndDate = employeeByBusinessPartner.EndDate;
                    dbEntry.RealEndDate = employeeByBusinessPartner.RealEndDate;
                    dbEntry.EmployeeCount = employeeByBusinessPartner.EmployeeCount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeByBusinessPartner Delete(EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            // Load EmployeeByBusinessPartner that will be deleted
            EmployeeByBusinessPartner dbEntry = context.EmployeeByBusinessPartners
                .FirstOrDefault(x => x.Identifier == employeeByBusinessPartner.Identifier);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                dbEntry.RealEndDate = employeeByBusinessPartner.RealEndDate;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
