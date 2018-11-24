using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Companies
{
    public class CompanySQLiteRepository
    {
        public static string CompanyTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Companies (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER, " +
            "Code INTEGER, " +
            "Name NVARCHAR(2048) NULL, " +
            "Address NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL)";

        public string SqlCommandSelectPart = "SELECT ServerId, Code, Name, " +
            "Address, IsSynced, UpdatedAt ";

        public string SqlCommandInsertPart = "INSERT INTO Companies " +
            "(Id, ServerId, Code, Name, " +
            "Address, IsSynced, UpdatedAt) " +
            "VALUES (NULL, @ServerId, @Code, @Name, " +
            "@Address, @IsSynced, @UpdatedAt);";

        public CompanyListResponse GetCompanies()
        {
            CompanyListResponse response = new CompanyListResponse();
            List<CompanyViewModel> companies = new List<CompanyViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Companies", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        CompanyViewModel dbEntry = new CompanyViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.CompanyCode = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.CompanyName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        companies.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    response.Success = false;
                    response.Message = error.Message;
                    response.Companies = new List<CompanyViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Companies = companies;
            return response;
        }

        public CompanyResponse Create(CompanyViewModel company)
        {
            CompanyResponse response = new CompanyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", company.Id);
                insertCommand.Parameters.AddWithValue("@Code", company.CompanyCode);
                insertCommand.Parameters.AddWithValue("@Name", company.CompanyName);
                insertCommand.Parameters.AddWithValue("@Address", company.Address);
                insertCommand.Parameters.AddWithValue("@IsSynced", company.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)company.UpdatedAt) ?? DBNull.Value);

                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }

        public CompanyResponse DeleteAll()
        {
            CompanyResponse response = new CompanyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Companies";
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }
    }
}

