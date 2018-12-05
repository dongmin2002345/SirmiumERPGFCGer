using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteCalculationViewRepository : IConstructionSiteCalculationRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public ConstructionSiteCalculationViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculations(int companyId)
        {
            List<ConstructionSiteCalculation> ConstructionSiteCalculations = new List<ConstructionSiteCalculation>();

            string queryString =
                "SELECT ConstructionSiteCalculationId, ConstructionSiteCalculationIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteCalculations " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteCalculation constructionSiteCalculation;
                    while (reader.Read())
                    {
                        constructionSiteCalculation = new ConstructionSiteCalculation();
                        constructionSiteCalculation.Id = Int32.Parse(reader["ConstructionSiteCalculationId"].ToString());
                        constructionSiteCalculation.Identifier = Guid.Parse(reader["ConstructionSiteCalculationIdentifier"].ToString());
                        
                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.ConstructionSite = new ConstructionSite();
                            constructionSiteCalculation.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteCalculation.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteCalculation.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["NumOfEmployees"] != DBNull.Value)
                            constructionSiteCalculation.NumOfEmployees = Int32.Parse(reader["NumOfEmployees"].ToString());
                        if (reader["EmployeePrice"] != DBNull.Value)
                            constructionSiteCalculation.EmployeePrice = decimal.Parse(reader["EmployeePrice"].ToString());
                        if (reader["NumOfMonths"] != DBNull.Value)
                            constructionSiteCalculation.NumOfMonths = Int32.Parse(reader["NumOfMonths"].ToString());
                        if (reader["OldValue"] != DBNull.Value)
                            constructionSiteCalculation.OldValue = decimal.Parse(reader["OldValue"].ToString());
                        if (reader["NewValue"] != DBNull.Value)
                            constructionSiteCalculation.NewValue = decimal.Parse(reader["NewValue"].ToString());
                        if (reader["ValueDifference"] != DBNull.Value)
                            constructionSiteCalculation.ValueDifference = decimal.Parse(reader["ValueDifference"].ToString());
                        if (reader["PlusMinus"] != DBNull.Value)
                            constructionSiteCalculation.PlusMinus = reader["PlusMinus"].ToString();

                        constructionSiteCalculation.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteCalculation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteCalculation.CreatedBy = new User();
                            constructionSiteCalculation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteCalculation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.Company = new Company();
                            constructionSiteCalculation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteCalculations.Add(constructionSiteCalculation);
                    }
                }
            }
            return ConstructionSiteCalculations;

            //List<ConstructionSiteCalculation> ConstructionSiteCalculations = context.ConstructionSiteCalculations
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSiteCalculations;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculationsByConstructionSite(int constructionSiteId)
        {
            List<ConstructionSiteCalculation> ConstructionSiteCalculations = new List<ConstructionSiteCalculation>();

            string queryString =
                "SELECT ConstructionSiteCalculationId, ConstructionSiteCalculationIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteCalculations " +
                "WHERE ConstructionSiteId = @ConstructionSiteId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", constructionSiteId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteCalculation constructionSiteCalculation;
                    while (reader.Read())
                    {
                        constructionSiteCalculation = new ConstructionSiteCalculation();
                        constructionSiteCalculation.Id = Int32.Parse(reader["ConstructionSiteCalculationId"].ToString());
                        constructionSiteCalculation.Identifier = Guid.Parse(reader["ConstructionSiteCalculationIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.ConstructionSite = new ConstructionSite();
                            constructionSiteCalculation.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteCalculation.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteCalculation.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["NumOfEmployees"] != DBNull.Value)
                            constructionSiteCalculation.NumOfEmployees = Int32.Parse(reader["NumOfEmployees"].ToString());
                        if (reader["EmployeePrice"] != DBNull.Value)
                            constructionSiteCalculation.EmployeePrice = decimal.Parse(reader["EmployeePrice"].ToString());
                        if (reader["NumOfMonths"] != DBNull.Value)
                            constructionSiteCalculation.NumOfMonths = Int32.Parse(reader["NumOfMonths"].ToString());
                        if (reader["OldValue"] != DBNull.Value)
                            constructionSiteCalculation.OldValue = decimal.Parse(reader["OldValue"].ToString());
                        if (reader["NewValue"] != DBNull.Value)
                            constructionSiteCalculation.NewValue = decimal.Parse(reader["NewValue"].ToString());
                        if (reader["ValueDifference"] != DBNull.Value)
                            constructionSiteCalculation.ValueDifference = decimal.Parse(reader["ValueDifference"].ToString());
                        if (reader["PlusMinus"] != DBNull.Value)
                            constructionSiteCalculation.PlusMinus = reader["PlusMinus"].ToString();

                        constructionSiteCalculation.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteCalculation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteCalculation.CreatedBy = new User();
                            constructionSiteCalculation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteCalculation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.Company = new Company();
                            constructionSiteCalculation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteCalculations.Add(constructionSiteCalculation);
                    }
                }
            }
            return ConstructionSiteCalculations;

            //List<ConstructionSiteCalculation> ConstructionSites = context.ConstructionSiteCalculations
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.ConstructionSiteId == constructionSiteId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public List<ConstructionSiteCalculation> GetConstructionSiteCalculationsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSiteCalculation> ConstructionSiteCalculations = new List<ConstructionSiteCalculation>();

            string queryString =
                "SELECT ConstructionSiteCalculationId, ConstructionSiteCalculationIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteCalculations " +
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
                    ConstructionSiteCalculation constructionSiteCalculation;
                    while (reader.Read())
                    {
                        constructionSiteCalculation = new ConstructionSiteCalculation();
                        constructionSiteCalculation.Id = Int32.Parse(reader["ConstructionSiteCalculationId"].ToString());
                        constructionSiteCalculation.Identifier = Guid.Parse(reader["ConstructionSiteCalculationIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.ConstructionSite = new ConstructionSite();
                            constructionSiteCalculation.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteCalculation.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteCalculation.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["NumOfEmployees"] != DBNull.Value)
                            constructionSiteCalculation.NumOfEmployees = Int32.Parse(reader["NumOfEmployees"].ToString());
                        if (reader["EmployeePrice"] != DBNull.Value)
                            constructionSiteCalculation.EmployeePrice = decimal.Parse(reader["EmployeePrice"].ToString());
                        if (reader["NumOfMonths"] != DBNull.Value)
                            constructionSiteCalculation.NumOfMonths = Int32.Parse(reader["NumOfMonths"].ToString());
                        if (reader["OldValue"] != DBNull.Value)
                            constructionSiteCalculation.OldValue = decimal.Parse(reader["OldValue"].ToString());
                        if (reader["NewValue"] != DBNull.Value)
                            constructionSiteCalculation.NewValue = decimal.Parse(reader["NewValue"].ToString());
                        if (reader["ValueDifference"] != DBNull.Value)
                            constructionSiteCalculation.ValueDifference = decimal.Parse(reader["ValueDifference"].ToString());
                        if (reader["PlusMinus"] != DBNull.Value)
                            constructionSiteCalculation.PlusMinus = reader["PlusMinus"].ToString();

                        constructionSiteCalculation.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteCalculation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteCalculation.CreatedBy = new User();
                            constructionSiteCalculation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteCalculation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.Company = new Company();
                            constructionSiteCalculation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteCalculations.Add(constructionSiteCalculation);
                    }
                }
            }
            return ConstructionSiteCalculations;

            //List<ConstructionSiteCalculation> ConstructionSites = context.ConstructionSiteCalculations
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public ConstructionSiteCalculation GetLastConstructionSiteCalculation(int companyId, int constructionSiteId)
        {
            ConstructionSiteCalculation constructionSiteCalculation = new ConstructionSiteCalculation();

            string queryString =
                "SELECT TOP 1 ConstructionSiteCalculationId, ConstructionSiteCalculationIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSiteCalculations " +
                "WHERE CompanyId = @CompanyId AND ConstructionSiteId = @ConstructionSiteId " +
                "ORDER BY ConstructionSiteCalculationId DESC;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", constructionSiteId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        constructionSiteCalculation = new ConstructionSiteCalculation();
                        constructionSiteCalculation.Id = Int32.Parse(reader["ConstructionSiteCalculationId"].ToString());
                        constructionSiteCalculation.Identifier = Guid.Parse(reader["ConstructionSiteCalculationIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.ConstructionSite = new ConstructionSite();
                            constructionSiteCalculation.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteCalculation.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteCalculation.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteCalculation.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["NumOfEmployees"] != DBNull.Value)
                            constructionSiteCalculation.NumOfEmployees = Int32.Parse(reader["NumOfEmployees"].ToString());
                        if (reader["EmployeePrice"] != DBNull.Value)
                            constructionSiteCalculation.EmployeePrice = decimal.Parse(reader["EmployeePrice"].ToString());
                        if (reader["NumOfMonths"] != DBNull.Value)
                            constructionSiteCalculation.NumOfMonths = Int32.Parse(reader["NumOfMonths"].ToString());
                        if (reader["OldValue"] != DBNull.Value)
                            constructionSiteCalculation.OldValue = decimal.Parse(reader["OldValue"].ToString());
                        if (reader["NewValue"] != DBNull.Value)
                            constructionSiteCalculation.NewValue = decimal.Parse(reader["NewValue"].ToString());
                        if (reader["ValueDifference"] != DBNull.Value)
                            constructionSiteCalculation.ValueDifference = decimal.Parse(reader["ValueDifference"].ToString());
                        if (reader["PlusMinus"] != DBNull.Value)
                            constructionSiteCalculation.PlusMinus = reader["PlusMinus"].ToString();

                        constructionSiteCalculation.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteCalculation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteCalculation.CreatedBy = new User();
                            constructionSiteCalculation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteCalculation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteCalculation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteCalculation.Company = new Company();
                            constructionSiteCalculation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteCalculation.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }

            Console.WriteLine("NOVO");

            return constructionSiteCalculation;

            //ConstructionSiteCalculation constructionSite = context.ConstructionSiteCalculations
            //    .Where(x => x.Company.Id == companyId && x.ConstructionSiteId == constructionSiteId)
            //    .OrderByDescending(x => x.Id)
            //    .FirstOrDefault();

            //Console.WriteLine("NOVO");

            //return constructionSite;
        }

        public ConstructionSiteCalculation Create(ConstructionSiteCalculation constructionSiteCalculation)
        {
            if (context.ConstructionSiteCalculations.Where(x => x.Identifier != null && x.Identifier == constructionSiteCalculation.Identifier).Count() == 0)
            {
                constructionSiteCalculation.Id = 0;

                constructionSiteCalculation.Active = true;

                context.ConstructionSiteCalculations.Add(constructionSiteCalculation);
                return constructionSiteCalculation;
            }
            else
            {
                // Load item that will be updated
                ConstructionSiteCalculation dbEntry = context.ConstructionSiteCalculations
                    .FirstOrDefault(x => x.Identifier == constructionSiteCalculation.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = constructionSiteCalculation.CompanyId ?? null;
                    dbEntry.CreatedById = constructionSiteCalculation.CreatedById ?? null;

                    // Set properties
                    dbEntry.NumOfEmployees = constructionSiteCalculation.NumOfEmployees;
                    dbEntry.EmployeePrice = constructionSiteCalculation.EmployeePrice;
                    dbEntry.NumOfMonths = constructionSiteCalculation.NumOfMonths;
                    dbEntry.OldValue = constructionSiteCalculation.OldValue;
                    dbEntry.NewValue = constructionSiteCalculation.NewValue;
                    dbEntry.ValueDifference = constructionSiteCalculation.ValueDifference;
                    dbEntry.PlusMinus = constructionSiteCalculation.PlusMinus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSiteCalculation Delete(Guid identifier)
        {
            ConstructionSiteCalculation dbEntry = context.ConstructionSiteCalculations
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
