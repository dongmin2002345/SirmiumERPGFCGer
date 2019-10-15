using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Vats;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Vats;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Vats
{
    public class VatViewRepository : IVatRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT VatId, VatIdentifier, VatCode, VatAmount, VatDescription, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vVats ";

        private Vat Read(SqlDataReader reader)
        {
            Vat Vat = new Vat();
            Vat.Id = Int32.Parse(reader["VatId"].ToString());
            Vat.Identifier = Guid.Parse(reader["VatIdentifier"].ToString());

            if (reader["VatCode"] != DBNull.Value)
                Vat.Code = reader["VatCode"].ToString();
            if (reader["VatAmount"] != DBNull.Value)
                Vat.Amount = Decimal.Parse(reader["VatAmount"].ToString());
            if (reader["VatDescription"] != DBNull.Value)
                Vat.Description = reader["VatDescription"].ToString();

            Vat.Active = bool.Parse(reader["Active"].ToString());
            Vat.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                Vat.CreatedBy = new User();
                Vat.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                Vat.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                Vat.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                Vat.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                Vat.Company = new Company();
                Vat.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                Vat.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                Vat.Company.Name = reader["CompanyName"].ToString();
            }

            return Vat;
        }

        public VatViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Vat> GetVats(int companyId)
        {
            List<Vat> Vats = new List<Vat>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Vat Vat;
                    while (reader.Read())
                    {
                        Vat = Read(reader);
                        Vats.Add(Vat);
                    }
                }
            }

            return Vats;
        }

        public List<Vat> GetVatsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Vat> Vats = new List<Vat>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND " +
                "CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Vat Vat;
                    while (reader.Read())
                    {
                        Vat = Read(reader);
                        Vats.Add(Vat);
                    }
                }
            }

            return Vats;
        }

        public Vat GetVat(int VatId)
        {
            Vat Vat = null;

            string queryString =
                SelectString +
                "WHERE VatId = @VatId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@VatId", VatId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vat = Read(reader);
                    }
                }
            }

            return Vat;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Vats
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Vat))
                    .Select(x => x.Entity as Vat))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Vats
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Vat))
                        .Select(x => x.Entity as Vat))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return count.ToString();
            }
        }

        public Vat Create(Vat Vat)
        {
            if (context.Vats.Where(x => x.Identifier != null && x.Identifier == Vat.Identifier).Count() == 0)
            {
                Vat.Id = 0;

                Vat.Code = GetNewCodeValue(Vat.CompanyId ?? 0);
                Vat.Active = true;

                Vat.UpdatedAt = DateTime.Now;
                Vat.CreatedAt = DateTime.Now;

                context.Vats.Add(Vat);
                return Vat;
            }
            else
            {
                // Load Vat that will be updated
                Vat dbEntry = context.Vats
                    .FirstOrDefault(x => x.Identifier == Vat.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = Vat.CompanyId ?? null;
                    dbEntry.CreatedById = Vat.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = Vat.Code;
                    dbEntry.Amount = Vat.Amount;
                    dbEntry.Description = Vat.Description;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Vat Delete(Guid identifier)
        {
            // Load item that will be deleted
            Vat dbEntry = context.Vats
                .FirstOrDefault(x => x.Identifier == identifier);

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
