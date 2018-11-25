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
    public class EmployeeCardViewRepository : IEmployeeCardRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public EmployeeCardViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<EmployeeCard> GetEmployeeCards(int companyId)
        {
            List<EmployeeCard> EmployeeCards = new List<EmployeeCard>();

            string queryString =
                "SELECT EmployeeCardId, EmployeeCardIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "CardDate, Description, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeCards " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeCard employeeCard;
                    while (reader.Read())
                    {
                        employeeCard = new EmployeeCard();
                        employeeCard.Id = Int32.Parse(reader["EmployeeCardId"].ToString());
                        employeeCard.Identifier = Guid.Parse(reader["EmployeeCardIdentifier"].ToString());

                       if (reader["EmployeeId"] != null)
                        {
                            employeeCard.Employee = new Employee();
                            employeeCard.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Identifier = Guid.Parse(reader["EmployeeCardTypeIdentifier"].ToString());
                            employeeCard.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeCard.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["CardDate"] != null)
                            employeeCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != null)
                            employeeCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != null)
                            employeeCard.PlusMinus = reader["PlusMinus"].ToString();

                        employeeCard.Active = bool.Parse(reader["Active"].ToString());
                        employeeCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeCard.CreatedBy = new User();
                            employeeCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeCard.Company = new Company();
                            employeeCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeCards.Add(employeeCard);
                    }
                }
            }

            //List<EmployeeCard> EmployeeCards = context.EmployeeCards
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            return EmployeeCards;
        }

        public List<EmployeeCard> GetEmployeeCardsByEmployee(int EmployeeId)
        {
            List<EmployeeCard> EmployeeCards = new List<EmployeeCard>();

            string queryString =
                "SELECT EmployeeCardId, EmployeeCardIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "CardDate, Description, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeCards " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EmployeeCard employeeCard;
                    while (reader.Read())
                    {
                        employeeCard = new EmployeeCard();
                        employeeCard.Id = Int32.Parse(reader["EmployeeCardId"].ToString());
                        employeeCard.Identifier = Guid.Parse(reader["EmployeeCardIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeCard.Employee = new Employee();
                            employeeCard.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Identifier = Guid.Parse(reader["EmployeeCardTypeIdentifier"].ToString());
                            employeeCard.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeCard.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["CardDate"] != null)
                            employeeCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != null)
                            employeeCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != null)
                            employeeCard.PlusMinus = reader["PlusMinus"].ToString();

                        employeeCard.Active = bool.Parse(reader["Active"].ToString());
                        employeeCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeCard.CreatedBy = new User();
                            employeeCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeCard.Company = new Company();
                            employeeCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeCards.Add(employeeCard);
                    }
                }
            }

            return EmployeeCards;

            //List<EmployeeCard> Employees = context.EmployeeCards
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public List<EmployeeCard> GetEmployeeCardsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeCard> EmployeeCards = new List<EmployeeCard>();

            string queryString =
                "SELECT EmployeeCardId, EmployeeCardIdentifier, " +
                "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, " +
                "CardDate, Description, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vEmployeeCards " +
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
                    EmployeeCard employeeCard;
                    while (reader.Read())
                    {
                        employeeCard = new EmployeeCard();
                        employeeCard.Id = Int32.Parse(reader["EmployeeCardId"].ToString());
                        employeeCard.Identifier = Guid.Parse(reader["EmployeeCardIdentifier"].ToString());

                        if (reader["EmployeeId"] != null)
                        {
                            employeeCard.Employee = new Employee();
                            employeeCard.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                            employeeCard.Employee.Identifier = Guid.Parse(reader["EmployeeCardTypeIdentifier"].ToString());
                            employeeCard.Employee.Code = reader["EmployeeCode"].ToString();
                            employeeCard.Employee.Name = reader["EmployeeName"].ToString();
                        }

                        if (reader["CardDate"] != null)
                            employeeCard.CardDate = DateTime.Parse(reader["CardDate"].ToString());
                        if (reader["Description"] != null)
                            employeeCard.Description = reader["Description"].ToString();
                        if (reader["PlusMinus"] != null)
                            employeeCard.PlusMinus = reader["PlusMinus"].ToString();

                        employeeCard.Active = bool.Parse(reader["Active"].ToString());
                        employeeCard.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employeeCard.CreatedBy = new User();
                            employeeCard.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employeeCard.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employeeCard.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            employeeCard.Company = new Company();
                            employeeCard.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employeeCard.Company.Name = reader["CompanyName"].ToString();
                        }

                        EmployeeCards.Add(employeeCard);
                    }
                }
            }

            return EmployeeCards;

            //List<EmployeeCard> Employees = context.EmployeeCards
            //    .Include(x => x.Employee)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return Employees;
        }

        public EmployeeCard Create(EmployeeCard EmployeeCard)
        {
            if (context.EmployeeCards.Where(x => x.Identifier != null && x.Identifier == EmployeeCard.Identifier).Count() == 0)
            {
                EmployeeCard.Id = 0;

                EmployeeCard.Active = true;

                context.EmployeeCards.Add(EmployeeCard);
                return EmployeeCard;
            }
            else
            {
                // Load item that will be updated
                EmployeeCard dbEntry = context.EmployeeCards
                    .FirstOrDefault(x => x.Identifier == EmployeeCard.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeCard.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeCard.CreatedById ?? null;

                    // Set properties
                    dbEntry.CardDate = EmployeeCard.CardDate;
                    dbEntry.Description = EmployeeCard.Description;
                    dbEntry.PlusMinus = EmployeeCard.PlusMinus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeCard Delete(Guid identifier)
        {
            EmployeeCard dbEntry = context.EmployeeCards
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
