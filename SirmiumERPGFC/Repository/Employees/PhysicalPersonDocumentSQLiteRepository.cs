using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonDocumentSQLiteRepository
    {
        public static string PhysicalPersonDocumentTableCreatePart =
                       "CREATE TABLE IF NOT EXISTS PhysicalPersonDocuments " +
                       "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                       "ServerId INTEGER NULL, " +
                       "Identifier GUID, " +
                       "PhysicalPersonId INTEGER NULL, " +
                       "PhysicalPersonIdentifier GUID NULL, " +
                       "PhysicalPersonCode NVARCHAR(48) NULL, " +
                       "PhysicalPersonName NVARCHAR(48) NULL, " +
                       "Name NVARCHAR(2048), " +
                       "CreateDate DATETIME NULL, " +
                       "Path NVARCHAR(2048) NULL, " +
                       "IsSynced BOOL NULL, " +
                       "UpdatedAt DATETIME NULL, " +
                       "CreatedById INTEGER NULL, " +
                       "CreatedByName NVARCHAR(2048) NULL, " +
                       "CompanyId INTEGER NULL, " +
                       "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, Name, CreateDate, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonDocuments " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, Name, CreateDate, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @Name, @CreateDate, @Path, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public PhysicalPersonDocumentListResponse GetPhysicalPersonDocumentsByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentListResponse();
            List<PhysicalPersonDocumentViewModel> PhysicalPersonDocuments = new List<PhysicalPersonDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonDocuments " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonDocumentViewModel dbEntry = new PhysicalPersonDocumentViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonDocuments.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonDocuments = new List<PhysicalPersonDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonDocuments = PhysicalPersonDocuments;
            return response;
        }

        public PhysicalPersonDocumentResponse GetPhysicalPersonDocument(Guid identifier)
        {
            PhysicalPersonDocumentResponse response = new PhysicalPersonDocumentResponse();
            PhysicalPersonDocumentViewModel PhysicalPersonDocument = new PhysicalPersonDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonDocumentViewModel dbEntry = new PhysicalPersonDocumentViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonDocument = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonDocument = new PhysicalPersonDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonDocument = PhysicalPersonDocument;
            return response;
        }

        public void Sync(IPhysicalPersonDocumentService PhysicalPersonDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonDocumentRequest request = new SyncPhysicalPersonDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonDocumentListResponse response = PhysicalPersonDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersonDocuments?.Count ?? 0;
                    List<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentsFromDB = response.PhysicalPersonDocuments;
                    foreach (var PhysicalPersonDocument in PhysicalPersonDocumentsFromDB.OrderBy(x => x.Id))
                    {
                            Delete(PhysicalPersonDocument.Identifier);
                            if (PhysicalPersonDocument.IsActive)
                            {
                                PhysicalPersonDocument.IsSynced = true;
                                Create(PhysicalPersonDocument);
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

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersonDocuments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersonDocuments WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonDocumentResponse Create(PhysicalPersonDocumentViewModel PhysicalPersonDocument)
        {
            PhysicalPersonDocumentResponse response = new PhysicalPersonDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonDocument.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonDocument.Identifier);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonDocument.PhysicalPerson.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonDocument.PhysicalPerson.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonDocument.PhysicalPerson.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonDocument.PhysicalPerson.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", PhysicalPersonDocument.Name);
                insertCommand.Parameters.AddWithValue("@CreateDate", ((object)PhysicalPersonDocument.CreateDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Path", ((object)PhysicalPersonDocument.Path) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonDocument.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonDocument.UpdatedAt) ?? DBNull.Value);
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

        public PhysicalPersonDocumentResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            PhysicalPersonDocumentResponse response = new PhysicalPersonDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE PhysicalPersonDocuments SET " +
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

        public PhysicalPersonDocumentResponse Delete(Guid identifier)
        {
            PhysicalPersonDocumentResponse response = new PhysicalPersonDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM PhysicalPersonDocuments WHERE Identifier = @Identifier";
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

        public PhysicalPersonDocumentResponse DeleteAll()
        {
            PhysicalPersonDocumentResponse response = new PhysicalPersonDocumentResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonDocuments";
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
