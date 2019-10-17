using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Vats
{
    public class VatSQLiteRepository
    {
        #region SQL
        public static string VatTableCreatePart =
           "CREATE TABLE IF NOT EXISTS Vats " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
           "Amount NVARCHAR(2048) NULL, " +
           "Description NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Amount, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Vats " +
            "(Id, ServerId, Identifier, Code, Amount, Description," +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Amount, @Description, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private VatViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            VatViewModel dbEntry = new VatViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Amount = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        public SqliteCommand AddCreateParameters(SqliteCommand insertCommand, VatViewModel vat)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", vat.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", vat.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)vat.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Amount", ((object)vat.Amount) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)vat.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", vat.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)vat.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read 

        public VatListResponse GetVatsByPage(int companyId, VatViewModel vatSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            VatListResponse response = new VatListResponse();
            List<VatViewModel> vats = new List<VatViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Vats " +
                        "WHERE (@Description IS NULL OR @Description = '' OR Description LIKE @Description) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, ServerId " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                   
                    selectCommand.Parameters.AddWithValue("@Description", ((object)vatSearchObject.Search_Description) != null ? "%" + vatSearchObject.Search_Description + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        vats.Add(Read(query));

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Vats " +
                        "WHERE (@Description IS NULL OR @Description = '' OR Description LIKE @Description) " +
                        "AND CompanyId = @CompanyId;", db);
                   
                    selectCommand.Parameters.AddWithValue("@Description", ((object)vatSearchObject.Search_Description) != null ? "%" + vatSearchObject.Search_Description + "%" : "");
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
                    response.Vats = new List<VatViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Vats = vats;
            return response;
        }

        public VatListResponse GetVatsForPopup(int companyId, string filterString)
        {
            VatListResponse response = new VatListResponse();
            List<VatViewModel> vats = new List<VatViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Vats " +
                        "WHERE (@Description IS NULL OR @Description = '' OR Description LIKE @Description OR Code LIKE @Description) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                   
                    selectCommand.Parameters.AddWithValue("@Description", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        vats.Add(Read(query));
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Vats = new List<VatViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Vats = vats;
            return response;
        }

        public VatResponse GetVat(Guid identifier)
        {
            VatResponse response = new VatResponse();
            VatViewModel vat = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Vats " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        vat = Read(query);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Vat = new VatViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Vat = vat;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IVatService vatService, Action<int, int> callback = null)
        {
            try
            {
                SyncVatRequest request = new SyncVatRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                VatListResponse response = vatService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Vats?.Count ?? 0;
                    List<VatViewModel> vatsFromDB = response.Vats;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Vats WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var vat in vatsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", vat.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (vat.IsActive)
                                {
                                    vat.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, vat);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Vats WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Vats WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public VatResponse Create(VatViewModel vat)
        {
            VatResponse response = new VatResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, vat);
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

        public VatResponse Delete(Guid identifier)
        {
            VatResponse response = new VatResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Vats WHERE Identifier = @Identifier";
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
