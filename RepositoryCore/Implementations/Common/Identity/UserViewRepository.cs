using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Identity
{
    public class UserViewRepository //: IUserRepository
    {
        //private ApplicationDbContext context;
        //private string connectionString;

        //public UserViewRepository(ApplicationDbContext context)
        //{
        //    this.context = context;
        //    connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        //}

        //public List<User> GetUsers()
        //{
        //    List<User> Users = new List<User>();

        //    string queryString =
        //        "SELECT UserId, UserIdentifier, UserUsername, UserFirstName, UserLastName, UserPasswordHash, UserEmail, " +
        //        "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
        //        "FROM vUsers " +
        //        "WHERE Active = 1;";   //Password, Roles??

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandText = queryString;

        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            User user;
        //            while (reader.Read())
        //            {
        //                user = new User();
        //                user.Id = Int32.Parse(reader["UserId"].ToString());
        //                user.Identifier = Guid.Parse(reader["UserIdentifier"].ToString());
        //                user.Username = reader["UserUsername"].ToString();
        //                user.FirstName = reader["UserFirstName"].ToString();
        //                user.LastName = reader["UserLastName"].ToString();
        //                user.PasswordHash = reader["UserPasswordHash"].ToString();
        //                user.Email = reader["UserEmail"]?.ToString();

        //                user.Active = bool.Parse(reader["Active"].ToString());
        //                user.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

        //                if (reader["CreatedById"] != null)
        //                {
        //                    user.CreatedBy = new User();
        //                    user.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
        //                    user.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
        //                    user.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
        //                    user.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
        //                }

        //                if (reader["CompanyId"] != null)
        //                {
        //                    user.Company = new Company();
        //                    user.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
        //                    user.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
        //                    user.Company.Name = reader["CompanyName"].ToString();
        //                }

        //                Users.Add(user);
        //            }
        //        }
        //    }
        //    return Users;


        //    //List<User> users = context.Users
        //    //    //.Where(x => x.Active == true)
        //    //    .ToList();

        //    //return users;
        //}

        //public User GetUser(int id)
        //{
        //    //User user = new User();

        //    //string queryString =
        //    //    "SELECT UserId, UserIdentifier, UserUsername, UserFirstName, UserLastName, UserPasswordHash, UserEmail, " +
        //    //    "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
        //    //    "FROM vUsers " +
        //    //    "WHERE UserId = @UserId AND Active = 1;";   //Password, Roles??

        //    //using (SqlConnection connection = new SqlConnection(connectionString))
        //    //{
        //    //    SqlCommand command = connection.CreateCommand();
        //    //    command.CommandText = queryString;
        //    //    command.Parameters.Add(new SqlParameter("@UserId", id));

        //    //    connection.Open();
        //    //    using (SqlDataReader reader = command.ExecuteReader())
        //    //    {
        //    //        if (reader.Read())
        //    //        {
        //    //            user = new User();
        //    //            user.Id = Int32.Parse(reader["UserId"].ToString());
        //    //            user.Identifier = Guid.Parse(reader["UserIdentifier"].ToString());
        //    //            user.Username = reader["UserUsername"].ToString();
        //    //            user.FirstName = reader["UserFirstName"].ToString();
        //    //            user.LastName = reader["UserLastName"].ToString();
        //    //            user.PasswordHash = reader["UserPasswordHash"].ToString();
        //    //            user.Email = reader["UserEmail"]?.ToString();

        //    //            user.Active = bool.Parse(reader["Active"].ToString());
        //    //            user.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

        //    //            if (reader["CreatedById"] != null)
        //    //            {
        //    //                user.CreatedBy = new User();
        //    //                user.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
        //    //                user.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
        //    //                user.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
        //    //                user.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
        //    //            }

        //    //            if (reader["CompanyId"] != null)
        //    //            {
        //    //                user.Company = new Company();
        //    //                user.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
        //    //                user.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
        //    //                user.Company.Name = reader["CompanyName"].ToString();
        //    //            }

        //    //        }
        //    //    }
        //    //}
        //    //return user;


        //    ////var bp = context.BusinessPartners.Include(x => x.BusinessPartnerUsers)
        //    ////    .ThenInclude(x => x.User)
        //    ////    .Where(x => x.BusinessPartnerUsers.FirstOrDefault(y => y.User.Id == id) != null)
        //    ////    .FirstOrDefault();

        //    ////var user = context.Users.Include(x => x.CompanyUsers)
        //    ////    .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    //if (bp != null)
        //    //{
        //    //    user.BusinessPartner = bp;
        //    //}

        //    //return user;
        //    return null;
        //}

        //public User Create(User user)
        //{
        //    // var companies = user?.CompanyUsers?.Select(x => x.Company) ?? new List<Company>();
        //    //List<Company> selectedCompanies = new List<Company>(companies);

        //    // Attach companies
        //    //user.CompanyUsers.Clear();
        //    //foreach (Company company in selectedCompanies)
        //    //{
        //    //    user.CompanyUsers.Add(new CompanyUsers()
        //    //    {
        //    //        Company = context.Companies.FirstOrDefault(x => x.Id == company.Id)
        //    //    });
        //    //}

        //    //if (user.BusinessPartner != null)
        //    //{
        //    //    var bp = context.BusinessPartners.Include(x => x.BusinessPartnerUsers)
        //    //        .ThenInclude(x => x.User)
        //    //        .Where(x => x.BusinessPartnerUsers.FirstOrDefault(y => y.User.Id == user.Id) != null)
        //    //        .FirstOrDefault(x => x.Id == user.BusinessPartner.Id);


        //    //    if (bp == null)
        //    //    {
        //    //        bp = context.BusinessPartners.Include(x => x.BusinessPartnerUsers).FirstOrDefault(x => x.Id == user.BusinessPartner.Id);
        //    //    }
        //    //    if (bp.BusinessPartnerUsers == null)
        //    //        bp.BusinessPartnerUsers = new List<BusinessPartnerUser>();
        //    //    var tmpUserInList = bp.BusinessPartnerUsers.FirstOrDefault(x => x.User.Id == user.Id);
        //    //    if (tmpUserInList == null)
        //    //    {
        //    //        bp.BusinessPartnerUsers.Add(new BusinessPartnerUser()
        //    //        {
        //    //            BusinessPartner = context.BusinessPartners.FirstOrDefault(x => x.Id == bp.Id),
        //    //            User = user
        //    //        });
        //    //    }
        //    //}

        //    // Set activity
        //    user.Active = true;

        //    // Set timestamps
        //    user.CreatedAt = DateTime.Now;
        //    user.UpdatedAt = DateTime.Now;

        //    // Add user to database
        //    context.Users.Add(user);

        //    // Return created user
        //    return user;
        //}

        //public User Update(User user)
        //{
        //    // Load user that will be updated
        //    User dbEntry = context.Users.Include("CompanyUsers").FirstOrDefault(x => x.Id == user.Id && x.Active == true);
        //    //var companies = user?.CompanyUsers?.Select(x => x.Company) ?? new List<Company>();
        //    //List<Company> selectedCompanies = new List<Company>(companies);

        //    if (dbEntry != null)
        //    {
        //        //// Attach companies
        //        //dbEntry.CompanyUsers?.Clear();



        //        //foreach (Company company in selectedCompanies)
        //        //{
        //        //    dbEntry.CompanyUsers.Add(new CompanyUsers()
        //        //    {
        //        //        Company = context.Companies.FirstOrDefault(x => x.Id == company.Id)
        //        //    });
        //        //}

        //        // Set properties
        //        dbEntry.Username = user.Username;
        //        dbEntry.FirstName = user.FirstName;
        //        dbEntry.LastName = user.LastName;

        //        if (user.PasswordHash != null)
        //            dbEntry.PasswordHash = user.PasswordHash;
        //        dbEntry.Email = user.Email;
        //        //dbEntry.RolesCSV = user.RolesCSV;

        //        //var bp = context.BusinessPartners.Include(x => x.BusinessPartnerUsers)
        //        //        .ThenInclude(x => x.User)
        //        //        .Where(x => x.BusinessPartnerUsers.FirstOrDefault(y => y.User.Id == user.Id) != null)
        //        //        .FirstOrDefault(x => x.Id == user.BusinessPartner.Id);

        //        //if (bp == null)
        //        //{
        //        //    bp = context.BusinessPartners.Include(x => x.BusinessPartnerUsers)
        //        //        .ThenInclude(x => x.User)
        //        //        .FirstOrDefault(x => x.Id == user.BusinessPartner.Id);
        //        //}
        //        //if (bp.BusinessPartnerUsers == null)
        //        //    bp.BusinessPartnerUsers = new List<BusinessPartnerUser>();

        //        //var tmpUserInList = bp.BusinessPartnerUsers.FirstOrDefault(x => x.User != null && x.User.Id == user.Id);
        //        //if (user.BusinessPartner == null)
        //        //{
        //        //    if (tmpUserInList != null)
        //        //    {
        //        //        bp.BusinessPartnerUsers.Remove(tmpUserInList);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (tmpUserInList == null)
        //        //    {
        //        //        bp.BusinessPartnerUsers.Add(new BusinessPartnerUser()
        //        //        {
        //        //            BusinessPartner = bp,
        //        //            User = dbEntry
        //        //        });
        //        //    }

        //        //}
        //        // Set timestamps
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }

        //    return dbEntry;
        //}

        //public User Delete(int id)
        //{
        //    // Load user that will be updated
        //    User dbEntry = context.Users.FirstOrDefault(x => x.Id == id && x.Active == true);

        //    if (dbEntry != null)
        //    {

        //        // Set activity
        //        dbEntry.Active = false;

        //        // Set timestamps
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }

        //    return dbEntry;
        //}
    }
}
