using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Gloabals;
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
    public class PhonebookPhoneSQLiteRepository
    {
        #region SQL
        public static string PhonebookPhoneTableCreatePart =
                        "CREATE TABLE IF NOT EXISTS PhonebookPhones " +
                        "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "ServerId INTEGER NULL, " +
                        "Identifier GUID, " +
                        "PhonebookId INTEGER NULL, " +
                        "PhonebookIdentifier GUID NULL, " +
                        "PhonebookCode NVARCHAR(48) NULL, " +
                        "PhonebookName NVARCHAR(48) NULL, " +
                        "Name NVARCHAR(2048), " +
                        "SurName NVARCHAR(2048), " +
                        "PhoneNumber NVARCHAR(2048), " +
                        "Email NVARCHAR(2048), " +
                       
                        "ItemStatus INTEGER NOT NULL, " +
                        "IsSynced BOOL NULL, " +
                        "UpdatedAt DATETIME NULL, " +
                        "CreatedById INTEGER NULL, " +
                        "CreatedByName NVARCHAR(2048) NULL, " +
                        "CompanyId INTEGER NULL, " +
                        "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Name, SurName, PhoneNumber, Email, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhonebookPhones " +
            "(Id, ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Name, SurName, PhoneNumber, Email, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhonebookId, @PhonebookIdentifier, " +
            "@PhonebookCode, @PhonebookName, @Name, @SurName, @PhoneNumber, @Email, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static PhonebookPhoneViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhonebookPhoneViewModel dbEntry = new PhonebookPhoneViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Phonebook = SQLiteHelper.GetPhonebook(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.PhoneNumber = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhonebookPhoneViewModel PhonebookPhone)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhonebookPhone.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhonebookPhone.Identifier);
            insertCommand.Parameters.AddWithValue("@PhonebookId", ((object)PhonebookPhone.Phonebook.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookIdentifier", ((object)PhonebookPhone.Phonebook.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookCode", ((object)PhonebookPhone.Phonebook.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookName", ((object)PhonebookPhone.Phonebook.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)PhonebookPhone.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@SurName", ((object)PhonebookPhone.SurName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhoneNumber", ((object)PhonebookPhone.PhoneNumber) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Email", ((object)PhonebookPhone.Email) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)PhonebookPhone.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", PhonebookPhone.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhonebookPhone.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhonebookPhoneListResponse GetPhonebookPhonesByPhonebook(int companyId, Guid PhonebookIdentifier)
        {
            PhonebookPhoneListResponse response = new PhonebookPhoneListResponse();
            List<PhonebookPhoneViewModel> PhonebookPhones = new List<PhonebookPhoneViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookPhones " +
                        "WHERE PhonebookIdentifier = @PhonebookIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@PhonebookIdentifier", PhonebookIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        PhonebookPhoneViewModel dbEntry = Read(query);
                        PhonebookPhones.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookPhones = new List<PhonebookPhoneViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookPhones = PhonebookPhones;
            return response;
        }

        public PhonebookPhoneResponse GetPhonebookPhone(Guid identifier)
        {
            PhonebookPhoneResponse response = new PhonebookPhoneResponse();
            PhonebookPhoneViewModel PhonebookPhone = new PhonebookPhoneViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookPhones " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhonebookPhoneViewModel dbEntry = Read(query);
                        PhonebookPhone = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookPhone = new PhonebookPhoneViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookPhone = PhonebookPhone;
            return response;
        }

        #endregion

        #region Sync
        public void Sync(IPhonebookPhoneService PhonebookPhoneservice, Action<int, int> callback = null)
        {
            try
            {
                SyncPhonebookPhoneRequest request = new SyncPhonebookPhoneRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhonebookPhoneListResponse response = PhonebookPhoneservice.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhonebookPhones?.Count ?? 0;
                    List<PhonebookPhoneViewModel> items = response.PhonebookPhones;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhonebookPhones WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhonebookPhones WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhonebookPhones WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public PhonebookPhoneResponse Create(PhonebookPhoneViewModel PhonebookPhone)
        {
            PhonebookPhoneResponse response = new PhonebookPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, PhonebookPhone);
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

        public PhonebookPhoneResponse Delete(Guid identifier)
        {
            PhonebookPhoneResponse response = new PhonebookPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM PhonebookPhones WHERE Identifier = @Identifier";
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


        public PhonebookPhoneResponse SetStatusDeleted(Guid identifier)
        {
            PhonebookPhoneResponse response = new PhonebookPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE PhonebookPhones SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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
