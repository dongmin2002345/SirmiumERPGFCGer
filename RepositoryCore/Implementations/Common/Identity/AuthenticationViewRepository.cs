using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Identity
{
    public class AuthenticationViewRepository : IAuthenticationRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public AuthenticationViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public User Authenticate(string username, string passwordHash)
        {
            User user = new User();

            string queryString =
                "SELECT UserId, UserIdentifier, UserUsername, UserFirstName, UserLastName, UserPasswordHash, UserEmail, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vUsers " +
                "WHERE UserUsername = @UserUsername AND UserPasswordHash = @UserPasswordHash;";   //AND Active = 1

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@UserUsername", username));
                command.Parameters.Add(new SqlParameter("@UserPasswordHash", passwordHash));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User();
                        user.Id = Int32.Parse(reader["UserId"].ToString());
                        user.Identifier = Guid.Parse(reader["UserIdentifier"].ToString());
                        user.Username = reader["UserUsername"].ToString();
                        user.FirstName = reader["UserFirstName"].ToString();
                        user.LastName = reader["UserLastName"].ToString();
                        user.PasswordHash = reader["UserPasswordHash"].ToString();
                        user.Email = reader["UserEmail"]?.ToString();

                        user.Active = bool.Parse(reader["Active"].ToString());
                        user.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            user.CreatedBy = new User();
                            user.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            user.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            user.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            user.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            user.Company = new Company();
                            user.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            user.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            user.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }
            return user;

            //User user = context.Users
            //    .FirstOrDefault(x => x.Username == username && x.PasswordHash == passwordHash);

            //return user;
        }
    }
}
