using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Phonebooks
{
    public class PhonebookSQLiteRepository
    {
        #region SQL

        public static string PhonebookTableCreatePart =
          "CREATE TABLE IF NOT EXISTS Phonebooks " +
          "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
          "ServerId INTEGER NULL, " +
          "Identifier GUID, " +
          "Code NVARCHAR(48) NULL, " +
          "Name NVARCHAR(48) NULL, " +
          "CountryId INTEGER NULL, " +
          "CountryIdentifier GUID, " +
          "CountryCode NVARCHAR(2048) NULL, " +
          "CountryName NVARCHAR(2048) NULL, " +
          "RegionId INTEGER NULL, " +
          "RegionIdentifier GUID, " +
          "RegionCode NVARCHAR(2048) NULL, " +
          "RegionName NVARCHAR(2048) NULL, " +
          "MunicipalityId INTEGER NULL, " +
          "MunicipalityIdentifier GUID, " +
          "MunicipalityCode NVARCHAR(2048) NULL, " +
          "MunicipalityName NVARCHAR(2048) NULL, " +
          "CityId INTEGER NULL, " +
          "CityIdentifier GUID, " +
          "CityCode NVARCHAR(2048) NULL, " +
          "CityName NVARCHAR(2048) NULL, " +
          "Address NVARCHAR(2048) NULL, " +

          "IsSynced BOOL NULL, " +
          "UpdatedAt DATETIME NULL, " +
          "CreatedById INTEGER NULL, " +
          "CreatedByName NVARCHAR(2048) NULL, " +
          "CompanyId INTEGER NULL, " +
          "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +

            "CityId, CityIdentifier, CityCode, CityName, Address, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Phonebooks " +
            "(Id, ServerId, Identifier, Code, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "CityId, CityIdentifier, CityCode, CityName, Address, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, @RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, @Address, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private static PhonebookViewModel Read(SqliteDataReader query)
            {
            int counter = 0;
            PhonebookViewModel dbEntry = new PhonebookViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
            dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
            dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);

            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhonebookViewModel Phonebook)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", Phonebook.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", Phonebook.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)Phonebook.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)Phonebook.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)Phonebook.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)Phonebook.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)Phonebook.Country?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)Phonebook.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionId", ((object)Phonebook.Region?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)Phonebook.Region?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)Phonebook.Region?.RegionCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionName", ((object)Phonebook.Region?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)Phonebook.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)Phonebook.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)Phonebook.Municipality?.MunicipalityCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)Phonebook.Municipality?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityId", ((object)Phonebook.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)Phonebook.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)Phonebook.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)Phonebook.City?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)Phonebook.Address) ?? DBNull.Value);
            
            insertCommand.Parameters.AddWithValue("@IsSynced", Phonebook.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)Phonebook.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhonebookListResponse GetPhonebooksByPage(int companyId, PhonebookViewModel PhonebookSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            List<PhonebookViewModel> Phonebooks = new List<PhonebookViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Phonebooks " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +

                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@Name", ((object)PhonebookSearchObject.Search_Name) != null ? "%" + PhonebookSearchObject.Search_Name + "%" : "");

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        PhonebookViewModel dbEntry = Read(query);
                        Phonebooks.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Phonebooks " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +

                        "AND CompanyId = @CompanyId;", db);

                    selectCommand.Parameters.AddWithValue("@Name", ((object)PhonebookSearchObject.Search_Name) != null ? "%" + PhonebookSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Phonebooks = new List<PhonebookViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Phonebooks = Phonebooks;
            return response;
        }

        public PhonebookListResponse GetPhonebooksForPopup(int companyId, string filterString)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            List<PhonebookViewModel> Phonebooks = new List<PhonebookViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Phonebooks " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code OR Code LIKE @Code) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);

                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        PhonebookViewModel dbEntry = Read(query);
                        Phonebooks.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Phonebooks = new List<PhonebookViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Phonebooks = Phonebooks;
            return response;
        }

        public PhonebookResponse GetPhonebook(Guid identifier)
        {
            PhonebookResponse response = new PhonebookResponse();
            PhonebookViewModel Phonebook = new PhonebookViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Phonebooks " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhonebookViewModel dbEntry = Read(query);
                        Phonebook = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Phonebook = new PhonebookViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Phonebook = Phonebook;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IPhonebookService PhonebookService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhonebookRequest request = new SyncPhonebookRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhonebookListResponse response = PhonebookService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Phonebooks?.Count ?? 0;
                    var items = new List<PhonebookViewModel>(response.Phonebooks);

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Phonebooks WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var item in items)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", item.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (item.IsActive)
                                {
                                    item.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, item);
                                    insertCommand.ExecuteNonQuery();
                                    insertCommand.Parameters.Clear();

                                    syncedItems++;
                                    callback?.Invoke(syncedItems, toSync);
                                }
                            }

                            transaction.Commit();
                        }
                        db.Close();
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
        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Phonebooks WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Phonebooks WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
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
                    MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return null;
        }

        #endregion

        #region Create

        public PhonebookResponse Create(PhonebookViewModel Phonebook)
        {
            PhonebookResponse response = new PhonebookResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, Phonebook);
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

        #endregion

        #region Delete

        public PhonebookResponse Delete(Guid identifier)
        {
            PhonebookResponse response = new PhonebookResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Phonebooks WHERE Identifier = @Identifier";
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

        #endregion
    }
}
