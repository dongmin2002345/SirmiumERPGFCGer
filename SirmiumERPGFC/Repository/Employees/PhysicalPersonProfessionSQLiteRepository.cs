using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonProfessionSQLiteRepository
    {
        public static string PhysicalPersonItemTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS PhysicalPersonProfessions " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "PhysicalPersonId INTEGER NULL, " +
                  "PhysicalPersonIdentifier GUID NULL, " +
                  "PhysicalPersonCode NVARCHAR(48) NULL, " +
                  "PhysicalPersonName NVARCHAR(48) NULL, " +
                  "ProfessionId INTEGER NULL, " +
                  "ProfessionIdentifier GUID NULL, " +
                  "ProfessionCode NVARCHAR(48) NULL, " +
                  "ProfessionName NVARCHAR(2048) NULL, " +
                  "ProfessionSecondCode NVARCHAR(48) NULL, " +
                  "CountryId INTEGER NULL, " +
                  "CountryIdentifier GUID NULL, " +
                  "CountryCode NVARCHAR(48) NULL, " +
                  "CountryName NVARCHAR(48) NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, ProfessionId, ProfessionIdentifier, " +
            "ProfessionCode, ProfessionName, ProfessionSecondCode, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonProfessions " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, ProfessionId, ProfessionIdentifier, " +
            "ProfessionCode, ProfessionName, ProfessionSecondCode, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @ProfessionId, @ProfessionIdentifier, " +
            "@ProfessionCode, @ProfessionName, @ProfessionSecondCode, @CountryId, @CountryIdentifier, " +
            "@CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public PhysicalPersonProfessionListResponse GetPhysicalPersonProfessionsByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            List<PhysicalPersonProfessionViewModel> PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonProfessions " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonProfessionViewModel dbEntry = new PhysicalPersonProfessionViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Profession = SQLiteHelper.GetProfession(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonProfessions.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonProfessions = new List<PhysicalPersonProfessionViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonProfessions = PhysicalPersonProfessions;
            return response;
        }

        public PhysicalPersonProfessionResponse GetPhysicalPersonProfession(Guid identifier)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();
            PhysicalPersonProfessionViewModel PhysicalPersonItem = new PhysicalPersonProfessionViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonProfessions " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonProfessionViewModel dbEntry = new PhysicalPersonProfessionViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Profession = SQLiteHelper.GetProfession(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonItem = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonProfession = new PhysicalPersonProfessionViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonProfession = PhysicalPersonItem;
            return response;
        }

        public void Sync(IPhysicalPersonProfessionService PhysicalPersonItemService)
        {
            SyncPhysicalPersonProfessionRequest request = new SyncPhysicalPersonProfessionRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);


            PhysicalPersonProfessionListResponse response = PhysicalPersonItemService.Sync(request);
            if (response.Success)
            {
                List<PhysicalPersonProfessionViewModel> PhysicalPersonItemsFromDB = response.PhysicalPersonProfessions;
                foreach (var PhysicalPersonItem in PhysicalPersonItemsFromDB.OrderBy(x => x.Id))
                {
                    Delete(PhysicalPersonItem.Identifier);
                    if (PhysicalPersonItem.IsActive)
                    {
                        PhysicalPersonItem.IsSynced = true;
                        Create(PhysicalPersonItem);
                    }
                }
            }
        }

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from  PhysicalPersonProfessions WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from  PhysicalPersonProfessions WHERE CompanyId = @CompanyId", db);
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                        query = selectCommand.ExecuteReader();
                        if (query.Read())
                        {
                            return query.GetDateTime(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return null;
        }

        public PhysicalPersonProfessionResponse Create(PhysicalPersonProfessionViewModel PhysicalPersonItem)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonItem.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonItem.Identifier);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonItem.PhysicalPerson?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonItem.PhysicalPerson?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonItem.PhysicalPerson?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonItem.PhysicalPerson?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionId", ((object)PhysicalPersonItem.Profession?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionIdentifier", ((object)PhysicalPersonItem.Profession?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionCode", ((object)PhysicalPersonItem.Profession?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionName", ((object)PhysicalPersonItem.Profession?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionSecondCode", ((object)PhysicalPersonItem.Profession?.SecondCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)PhysicalPersonItem.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)PhysicalPersonItem.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)PhysicalPersonItem.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)PhysicalPersonItem.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonItem.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonItem.UpdatedAt) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
                insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
                insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
                insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

                try
                {
                    insertCommand.ExecuteReader();
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

        public PhysicalPersonProfessionResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE PhysicalPersonProfessions SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);

                try
                {
                    insertCommand.ExecuteReader();
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

        public PhysicalPersonProfessionResponse Delete(Guid identifier)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM  PhysicalPersonProfessions WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
                    insertCommand.ExecuteReader();
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

        public PhysicalPersonProfessionResponse DeleteAll()
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonProfessions";
                    try
                    {
                        insertCommand.ExecuteReader();
                    }
                    catch (SqliteException error)
                    {
                        response.Success = false;
                        response.Message = error.Message;

                        MainWindow.ErrorMessage = error.Message;
                        return response;
                    }
                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                return response;
            }

            response.Success = true;
            return response;
        }
    }
}
