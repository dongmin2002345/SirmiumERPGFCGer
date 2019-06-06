using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Companies
{
    public class CompanySQLiteRepository
    {
        public static string CompanyTableCreatePart =
               "CREATE TABLE IF NOT EXISTS Companies (" +
               "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID NOT NULL, " +
               "Code NVARCHAR(48) NULL, " +
               "Name NVARCHAR(2048) NULL, " +
               "Address NVARCHAR(2048) NULL, " +
               "ZipCode NVARCHAR(48) NULL, " +
               "IdentificationNumber NVARCHAR(48) NULL, " +
               "PIBNumber NVARCHAR(48) NULL, " +
               "PIONumber NVARCHAR(48) NULL, " +
               "PDVNumber NVARCHAR(48) NULL, " +
               "IndustryCode NVARCHAR(48) NULL, " +
               "IndustryName NVARCHAR(48) NULL, " +
            "Email NVARCHAR(2048) NULL, " +
            "WebSite NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL)";

        public string SqlCommandSelectPart = "SELECT ServerId, Identifier, Code, Name, " +
            "Address, ZipCode, IdentificationNumber, PIBNumber, PIONumber, PDVNumber, " +
            "IndustryCode, IndustryName, Email, WebSite, " +
            "IsSynced, UpdatedAt ";

        public string SqlCommandInsertPart = "INSERT INTO Companies " +
            "(Id, ServerId, Identifier, Code, Name, " +
            "Address, ZipCode, IdentificationNumber, PIBNumber, PIONumber, PDVNumber, " +
            "IndustryCode, IndustryName, Email, WebSite, " +
            "IsSynced, UpdatedAt) " +
            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
            "@Address,@ZipCode, @IdentificationNumber, @PIBNumber, @PIONumber, @PDVNumber, " +
            "@IndustryCode, @IndustryName, @Email, @WebSite, " +
            "@IsSynced, @UpdatedAt);";

        public CompanyListResponse GetCompanies(CompanyViewModel filterObject)
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
                        "FROM Companies " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR PIBNumber LIKE @PIB)", db);

                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterObject?.Search_Code) != null ? "%" + filterObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterObject?.Search_Name) != null ? "%" + filterObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)filterObject?.Search_Pib) != null ? "%" + filterObject.Search_Pib + "%" : "");


                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        CompanyViewModel dbEntry = new CompanyViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.CompanyCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CompanyName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ZipCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIBNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIONumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDVNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);

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
                insertCommand.Parameters.AddWithValue("@Identifier", company?.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)company?.CompanyCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)company?.CompanyName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", ((object)company?.Address) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ZipCode", ((object)company?.ZipCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IdentificationNumber", ((object)company.IdentificationNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PIBNumber", ((object)company.PIBNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PIONumber", ((object)company.PIONumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PDVNumber", ((object)company.PDVNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IndustryCode", ((object)company.IndustryCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IndustryName", ((object)company.IndustryName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Email", ((object)company.Email) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WebSite", ((object)company.WebSite) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", ((object)company.IsSynced) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)company.UpdatedAt) ?? DBNull.Value);

                try
                {
                    insertCommand.ExecuteNonQuery();
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

        public CompanyResponse Delete(Guid identifier)
        {
            CompanyResponse response = new CompanyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Companies WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }

        public CompanyResponse UpdateSyncStatus(Guid identifier, int serverId, string code, bool isSynced, DateTime? lastUpdate)
        {
            CompanyResponse response = new CompanyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Companies SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId," +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);
                insertCommand.Parameters.AddWithValue("@Code", ((object)code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)lastUpdate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
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


        public DateTime? GetLastUpdatedAt()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Companies", db);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Companies", db);
                        query = selectCommand.ExecuteReader();
                        if (query.Read())
                        {
                            int counter = 0;
                            return SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return null;
        }

        public void Sync(ICompanyService compService, Action<int, int> callback = null)
        {
            try
            {
                SyncCompanyRequest request = new SyncCompanyRequest();
                request.LastUpdatedAt = GetLastUpdatedAt();

                int toSync = 0;
                int syncedItems = 0;

                CompanyListResponse response = compService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Companies?.Count ?? 0;
                    List<CompanyViewModel> companiesFromDB = response.Companies;
                    foreach (var user in companiesFromDB.OrderBy(x => x.Id))
                    {
                            Delete(user.Identifier);
                            if (user.IsActive)
                            {
                                user.IsSynced = true;
                                Create(user);
                                syncedItems++;
                                callback?.Invoke(syncedItems, toSync);
                            }
                    }
                }
                else
                    throw new Exception(response.Message);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
    }
}

