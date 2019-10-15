using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Prices;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Prices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Prices
{
    public class DiscountViewRepository : IDiscountRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vDiscounts ";

        private Discount Read(SqlDataReader reader)
        {
            Discount discount = new Discount();
            discount.Id = Int32.Parse(reader["DiscountId"].ToString());
            discount.Identifier = Guid.Parse(reader["DiscountIdentifier"].ToString());

            if (reader["DiscountCode"] != DBNull.Value)
                discount.Code = reader["DiscountCode"].ToString();
            if (reader["DiscountName"] != DBNull.Value)
                discount.Name = reader["DiscountName"].ToString();
            if (reader["DiscountAmount"] != DBNull.Value)
                discount.Amount = decimal.Parse(reader["DiscountAmount"].ToString());

            discount.Active = bool.Parse(reader["Active"].ToString());
            discount.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                discount.CreatedBy = new User();
                discount.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                discount.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                discount.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                discount.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                discount.Company = new Company();
                discount.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                discount.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                discount.Company.Name = reader["CompanyName"].ToString();
            }

            return discount;
        }

        public DiscountViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Discount> GetDiscounts(int companyId)
        {
            List<Discount> discounts = new List<Discount>();

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
                    Discount discount;
                    while (reader.Read())
                    {
                        discount = Read(reader);
                        discounts.Add(discount);
                    }
                }
            }

            return discounts;
        }

        public List<Discount> GetDiscountsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Discount> discounts = new List<Discount>();

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
                    Discount discount;
                    while (reader.Read())
                    {
                        discount = Read(reader);
                        discounts.Add(discount);
                    }
                }
            }

            return discounts;
        }

        public Discount GetDiscount(int discountId)
        {
            Discount discount = null;

            string queryString =
                SelectString +
                "WHERE DiscountId = @DiscountId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@DiscountId", discountId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        discount = Read(reader);
                    }
                }
            }

            return discount;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Discounts
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Discount))
                    .Select(x => x.Entity as Discount))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Discounts
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Discount))
                        .Select(x => x.Entity as Discount))
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

        public Discount Create(Discount discount)
        {
            if (context.Discounts.Where(x => x.Identifier != null && x.Identifier == discount.Identifier).Count() == 0)
            {
                discount.Id = 0;

                discount.Code = GetNewCodeValue(discount.CompanyId ?? 0);
                discount.Active = true;

                discount.UpdatedAt = DateTime.Now;
                discount.CreatedAt = DateTime.Now;

                context.Discounts.Add(discount);
                return discount;
            }
            else
            {
                // Load Discount that will be updated
                Discount dbEntry = context.Discounts
                    .FirstOrDefault(x => x.Identifier == discount.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = discount.CompanyId ?? null;
                    dbEntry.CreatedById = discount.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = discount.Code;
                    dbEntry.Name = discount.Name;
                    dbEntry.Amount = discount.Amount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Discount Delete(Guid identifier)
        {
            // Load item that will be deleted
            Discount dbEntry = context.Discounts
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

