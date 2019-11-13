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
    public class PhysicalPersonAttachmentSQLiteRepository
    {
        #region SQL

        public static string PhysicalPersonAttachmentsTableCreatePart =
            @"CREATE TABLE IF NOT EXISTS PhysicalPersonAttachments  
                (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                ServerId INTEGER NULL, 
                Identifier GUID, 
                Code NVARCHAR(2048) NULL, 
                OK BOOL NULL, 

                PhysicalPersonId INTEGER NULL, 
                PhysicalPersonIdentifier GUID NULL, 
                PhysicalPersonCode NVARCHAR(48) NULL, 
                PhysicalPersonName NVARCHAR(2048) NULL, 

                IsSynced BOOL NULL, 
                UpdatedAt DATETIME NULL, 
                CreatedById INTEGER NULL, 
                CreatedByName NVARCHAR(2048) NULL, 
                CompanyId INTEGER NULL, 
                CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            @"SELECT ServerId, Identifier, 
                Code, OK, PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName,            
                IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = @"
            INSERT INTO PhysicalPersonAttachments (Id, ServerId, Identifier, Code, OK, PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, 
                IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) 
            VALUES (NULL, @ServerId, @Identifier, @Code, @OK, @PhysicalPersonId, @PhysicalPersonIdentifier, @PhysicalPersonCode, @PhysicalPersonName, 
                @IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";
        #endregion

        #region Helper methods
        private static PhysicalPersonAttachmentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhysicalPersonAttachmentViewModel dbEntry = new PhysicalPersonAttachmentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.OK = SQLiteHelper.GetBoolean(query, ref counter);

            dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhysicalPersonAttachmentViewModel PhysicalPersonAttachment)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonAttachment.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonAttachment.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)PhysicalPersonAttachment.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@OK", ((object)PhysicalPersonAttachment.OK) ?? DBNull.Value);

            
            insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonAttachment.PhysicalPerson?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonAttachment.PhysicalPerson?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonAttachment.PhysicalPerson?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonAttachment.PhysicalPerson?.Name) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonAttachment.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonAttachment.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhysicalPersonAttachmentListResponse GetPhysicalPersonAttachmentsByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            List<PhysicalPersonAttachmentViewModel> PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        @"FROM PhysicalPersonAttachments att 
                            WHERE (@PhysicalPersonIdentifier IS NULL OR @PhysicalPersonIdentifier = '' OR att.PhysicalPersonIdentifier LIKE @PhysicalPersonIdentifier) 
                            AND att.CompanyId = @CompanyId 
                            ORDER BY att.Id ASC ", db);

                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonIdentifier) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        PhysicalPersonAttachmentViewModel dbEntry = Read(query);
                        PhysicalPersonAttachments.Add(dbEntry);

                    }
                    response.PhysicalPersonAttachments = PhysicalPersonAttachments;

                    selectCommand = new SqliteCommand(
                        @"SELECT Count(*) 
                            FROM PhysicalPersonAttachments att
                            WHERE(@PhysicalPersonIdentifier IS NULL OR @PhysicalPersonIdentifier = '' OR att.PhysicalPersonIdentifier LIKE @PhysicalPersonIdentifier)
                            AND att.CompanyId = @CompanyId
                            ORDER BY att.IsSynced, att.Id DESC", db);

                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonIdentifier) ?? DBNull.Value);
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
                    response.PhysicalPersonAttachments = new List<PhysicalPersonAttachmentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonAttachments = PhysicalPersonAttachments;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IPhysicalPersonAttachmentService PhysicalPersonAttachmentService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonAttachmentRequest request = new SyncPhysicalPersonAttachmentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonAttachmentListResponse response = PhysicalPersonAttachmentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersonAttachments?.Count ?? 0;
                    List<PhysicalPersonAttachmentViewModel> PhysicalPersonsFromDB = response.PhysicalPersonAttachments;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhysicalPersonAttachments WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var PhysicalPerson in PhysicalPersonsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", PhysicalPerson.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (PhysicalPerson.IsActive)
                                {
                                    PhysicalPerson.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, PhysicalPerson);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersonAttachments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersonAttachments WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonResponse Create(PhysicalPersonAttachmentViewModel attachment)
        {
            PhysicalPersonResponse response = new PhysicalPersonResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, attachment);
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

        public PhysicalPersonAttachmentResponse Delete(Guid identifier)
        {
            PhysicalPersonAttachmentResponse response = new PhysicalPersonAttachmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM PhysicalPersonAttachments WHERE Identifier = @Identifier";
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

        public PhysicalPersonAttachmentResponse DeleteAll()
        {
            PhysicalPersonAttachmentResponse response = new PhysicalPersonAttachmentResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonAttachments ";
                    try
                    {
                        insertCommand.ExecuteNonQuery();
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

        #endregion
    }
}
