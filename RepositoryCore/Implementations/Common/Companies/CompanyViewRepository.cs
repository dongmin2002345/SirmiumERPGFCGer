using Configurator;
using DomainCore.Common.Companies;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Companies
{
    public class CompanyViewRepository //: ICompanyRepository
    {
        //public static ConcurrentDictionary<int, ConcurrentDictionary<int, Company>> CompanyDictionary = new ConcurrentDictionary<int, ConcurrentDictionary<int, Company>>();

        //private ApplicationDbContext context;
        //private string connectionString;

        //public CompanyViewRepository(ApplicationDbContext context)
        //{
        //    this.context = context;
        //    connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        //}

        //public List<Company> GetCompanies()
        //{
        //    List<Company> Companies = new List<Company>();

        //    string queryString =
        //        "SELECT Id, Identifier, Code, Name, Address, " +
        //        "Active, UpdatedAt " +
        //        "FROM vCompanies " +
        //        "WHERE Active = 1;";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandText = queryString;


        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            Company company;
        //            while (reader.Read())
        //            {
        //                company = new Company();
        //                company.Id = Int32.Parse(reader["Id"].ToString());
        //                company.Identifier = Guid.Parse(reader["Identifier"].ToString());
        //                company.Code = Int32.Parse(reader["Code"].ToString());
        //                company.Name = reader["Name"].ToString();
        //                company.Address = reader["Address"].ToString();

        //                company.Active = bool.Parse(reader["Active"].ToString());
        //                company.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

        //                //if (reader["CreatedById"] != DBNull.Value)
        //                //{
        //                //    company.CreatedBy = new User();
        //                //    company.CreatedById = Int32.Parse(reader["CreatedById"].ToString()); //???
        //                //    company.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
        //                //    company.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
        //                //    company.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
        //                //}

        //                Companies.Add(company);
        //            }
        //        }
        //    }
        //    return Companies;

        //    //List<Company> Companies = context.Companies
        //    //    .Where(x => x.Active == true)
        //    //    .OrderByDescending(x => x.UpdatedAt)
        //    //    .ToList();

        //    //return Companies;
        //}

        //public Company GetCompany(int id)
        //{
        //    Company company = new Company();

        //    string queryString =
        //        "SELECT Id, Identifier, Code, Name, Address, " +
        //        "Active, UpdatedAt " +
        //        "FROM vCompanies " +
        //        "WHERE Id = @Id AND Active = 1;";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandText = queryString;
        //        command.Parameters.Add(new SqlParameter("@Id", id));

        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                company = new Company();
        //                company.Id = Int32.Parse(reader["Id"].ToString());
        //                company.Identifier = Guid.Parse(reader["Identifier"].ToString());
        //                company.Code = Int32.Parse(reader["Code"].ToString());
        //                company.Name = reader["Name"].ToString();
        //                company.Address = reader["Address"].ToString();

        //                company.Active = bool.Parse(reader["Active"].ToString());
        //                company.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

        //                //if (reader["CreatedById"] != DBNull.Value)
        //                //{
        //                //    company.CreatedBy = new User();
        //                //    company.CreatedById = Int32.Parse(reader["CreatedById"].ToString()); //???
        //                //    company.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
        //                //    company.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
        //                //    company.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
        //                //}

        //            }
        //        }
        //    }
        //    return company;

        //    //return context.Companies
        //    //    .FirstOrDefault(x => x.Id == id && x.Active == true);
        //}

        ////public Company GetFromDictionary(int userId, int companyId)
        ////{
        ////    lock (CompanyDictionary)
        ////    {
        ////        if (CompanyDictionary.ContainsKey(userId))
        ////        {
        ////            if (CompanyDictionary[userId].ContainsKey(companyId))
        ////            {
        ////                return CompanyDictionary[userId][companyId];
        ////            }
        ////            else
        ////            {
        ////                Company company = GetCompany(companyId);
        ////                ConcurrentDictionary<int, Company> tmpDictionary = CompanyDictionary[userId];
        ////                tmpDictionary.TryAdd(companyId, company);
        ////                CompanyDictionary[userId] = tmpDictionary;

        ////                return CompanyDictionary[userId][companyId];
        ////            }
        ////        }
        ////        else
        ////        {
        ////            Company company = GetCompany(companyId);
        ////            ConcurrentDictionary<int, Company> tmpDictionary = new ConcurrentDictionary<int, Company>();
        ////            tmpDictionary.TryAdd(companyId, company);
        ////            CompanyDictionary.TryAdd(userId, tmpDictionary);

        ////            return CompanyDictionary[userId][companyId];
        ////        }
        ////    }
        ////}

        ////public void ClearDictionary(int userId)
        ////{
        ////    if (CompanyDictionary.ContainsKey(userId))
        ////        CompanyDictionary[userId] = new ConcurrentDictionary<int, Company>();
        ////}

        //public int GetNewCodeValue()
        //{
        //    var companyID = context.Companies.Max(x => (int?)x.Id);
        //    var maxId = (companyID == null ? 0 : companyID);
        //    var newId = maxId + 1;
        //    if (newId < 1000)
        //        newId = 1000 + newId;
        //    return (int)newId;
        //}

        //public Company Create(Company company)
        //{
        //    // Set activity
        //    company.Active = true;

        //    // Set timestamps
        //    company.CreatedAt = DateTime.Now;
        //    company.UpdatedAt = DateTime.Now;

        //    // Add Company to database
        //    context.Companies.Add(company);
        //    return company;
        //}

        //public Company Update(Company company)
        //{
        //    // Load Company that will be updated
        //    Company dbEntry = context.Companies
        //        .FirstOrDefault(x => x.Id == company.Id && x.Active == true);

        //    if (dbEntry != null)
        //    {
        //        dbEntry.Code = company.Code;
        //        dbEntry.Name = company.Name;
        //        dbEntry.Address = company.Address;
        //        dbEntry.BankAccountNo = company.BankAccountNo;
        //        dbEntry.BankAccountName = company.BankAccountName;
        //        dbEntry.IdentificationNumber = company.IdentificationNumber;
        //        dbEntry.PIBNumber = company.PIBNumber;
        //        dbEntry.PIONumber = company.PIONumber;
        //        dbEntry.PDVNumber = company.PDVNumber;
        //        dbEntry.IndustryCode = company.IndustryCode;
        //        dbEntry.IndustryName = company.IndustryName;
        //        dbEntry.Email = company.Email;
        //        dbEntry.WebSite = company.WebSite;
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }
        //    return dbEntry;
        //}

        //public Company Delete(int id)
        //{
        //    // Load Company that will be deleted
        //    Company dbEntry = context.Companies
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    if (dbEntry != null)
        //    {
        //        // Set activity
        //        dbEntry.Active = false;
        //        // Set timestamp
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }
        //    return dbEntry;
        //}
    }
}
