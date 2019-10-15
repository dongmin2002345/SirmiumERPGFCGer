using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonItemSQLiteRepository
    {
        #region SQL

        public static string PhysicalPersonItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS PhysicalPersonItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "PhysicalPersonId INTEGER NULL, " +
               "PhysicalPersonIdentifier GUID NULL, " +
               "PhysicalPersonCode NVARCHAR(48) NULL, " +
               "PhysicalPersonName NVARCHAR(48) NULL, " +
               "FamilyMemberId INTEGER NULL, " +
               "FamilyMemberIdentifier GUID NULL, " +
               "FamilyMemberCode NVARCHAR(48) NULL, " +
               "FamilyMemberName NVARCHAR(48) NULL, " +
               "Name NVARCHAR(2048), " +
               "DateOfBirth DATETIME NULL, " +
               "EmbassyDate DATETIME NULL, " +
               "Passport NVARCHAR(2048) NULL, " +
               "ItemStatus INTEGER NOT NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonItems " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @FamilyMemberId, @FamilyMemberIdentifier, " +
            "@FamilyMemberCode, @FamilyMemberName, @Name, @DateOfBirth, @EmbassyDate, @Passport, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static PhysicalPersonItemViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhysicalPersonItemViewModel dbEntry = new PhysicalPersonItemViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
            dbEntry.FamilyMember = SQLiteHelper.GetFamilyMember(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhysicalPersonItemViewModel PhysicalPersonItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonItem.Identifier);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonItem.PhysicalPerson.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonItem.PhysicalPerson.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonItem.PhysicalPerson.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonItem.PhysicalPerson.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberId", ((object)PhysicalPersonItem.FamilyMember.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberIdentifier", ((object)PhysicalPersonItem.PhysicalPerson.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberCode", ((object)PhysicalPersonItem.FamilyMember.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberName", ((object)PhysicalPersonItem.FamilyMember.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", PhysicalPersonItem.Name);
            insertCommand.Parameters.AddWithValue("@DateOfBirth", ((object)PhysicalPersonItem.DateOfBirth) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmbassyDate", ((object)PhysicalPersonItem.EmbassyDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Passport", ((object)PhysicalPersonItem.Passport) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", PhysicalPersonItem.ItemStatus);
            insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonItem.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonItem.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhysicalPersonItemListResponse GetPhysicalPersonItemsByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
            List<PhysicalPersonItemViewModel> PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonItems " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                   
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        PhysicalPersonItemViewModel dbEntry = Read(query);
                        PhysicalPersonItems.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonItems = PhysicalPersonItems;
            return response;
        }

        public PhysicalPersonItemResponse GetPhysicalPersonItem(Guid identifier)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();
            PhysicalPersonItemViewModel PhysicalPersonItem = new PhysicalPersonItemViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonItems " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhysicalPersonItemViewModel dbEntry = Read(query);
                        PhysicalPersonItem = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonItem = new PhysicalPersonItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonItem = PhysicalPersonItem;
            return response;
        }

        //public PhysicalPersonItemListResponse GetUnSyncedItems(int companyId)
        //{
        //    PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
        //    List<PhysicalPersonItemViewModel> viewModels = new List<PhysicalPersonItemViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM  PhysicalPersonItems " +
        //                "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
        //                "ORDER BY Id DESC;", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                PhysicalPersonItemViewModel dbEntry = new PhysicalPersonItemViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
        //                dbEntry.FamilyMember = SQLiteHelper.GetFamilyMember(query, ref counter);
        //                dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                viewModels.Add(dbEntry);
        //            }

        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.PhysicalPersonItems = viewModels;
        //    return response;
        //}

        #endregion

        #region Sync

        public void Sync(IPhysicalPersonItemService physicalPersonItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonItemRequest request = new SyncPhysicalPersonItemRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonItemListResponse response = physicalPersonItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersonItems?.Count ?? 0;
                    List<PhysicalPersonItemViewModel> items = response.PhysicalPersonItems;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhysicalPersonItems WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersonItems WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersonItems WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonItemResponse Create(PhysicalPersonItemViewModel PhysicalPersonItem)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, PhysicalPersonItem);
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

        public PhysicalPersonItemResponse Delete(Guid identifier)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM PhysicalPersonItems WHERE Identifier = @Identifier";
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

        //public PhysicalPersonItemResponse DeleteAll()
        //{
        //    PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM PhysicalPersonItems";
        //            try
        //            {
        //                insertCommand.ExecuteNonQuery();
        //            }
        //            catch (SqliteException error)
        //            {
        //                response.Success = false;
        //                response.Message = error.Message;

        //                MainWindow.ErrorMessage = error.Message;
        //                return response;
        //            }
        //            db.Close();
        //        }
        //    }
        //    catch (SqliteException error)
        //    {
        //        response.Success = false;
        //        response.Message = error.Message;
        //        return response;
        //    }

        //    response.Success = true;
        //    return response;
        //}
        public PhysicalPersonItemResponse SetStatusDeleted(Guid identifier)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE PhysicalPersonItems SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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

        #endregion
    }
}
