﻿using Microsoft.Data.Sqlite;
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
    public class PhysicalPersonItemSQLiteRepository
    {
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
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonItems " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @FamilyMemberId, @FamilyMemberIdentifier, " +
            "@FamilyMemberCode, @FamilyMemberName, @Name, @DateOfBirth, @EmbassyDate, @Passport, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

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
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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
                    response.PhysicalPersonItem = new PhysicalPersonItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonItem = PhysicalPersonItem;
            return response;
        }

        public PhysicalPersonItemListResponse GetUnSyncedItems(int companyId)
        {
            PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
            List<PhysicalPersonItemViewModel> viewModels = new List<PhysicalPersonItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonItems " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
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
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        viewModels.Add(dbEntry);
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
            response.PhysicalPersonItems = viewModels;
            return response;
        }

        public void Sync(IPhysicalPersonItemService PhysicalPersonItemService)
        {
            var unSynced = GetUnSyncedItems(MainWindow.CurrentCompanyId);
            SyncPhysicalPersonItemRequest request = new SyncPhysicalPersonItemRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            PhysicalPersonItemListResponse response = PhysicalPersonItemService.Sync(request);
            if (response.Success)
            {
                List<PhysicalPersonItemViewModel> PhysicalPersonItemsFromDB = response.PhysicalPersonItems;
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

        public PhysicalPersonItemResponse Create(PhysicalPersonItemViewModel PhysicalPersonItem)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

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

        public PhysicalPersonItemResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE PhysicalPersonItems SET " +
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

        public PhysicalPersonItemResponse Delete(Guid identifier)
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM PhysicalPersonItems WHERE Identifier = @Identifier";
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

        public PhysicalPersonItemResponse DeleteAll()
        {
            PhysicalPersonItemResponse response = new PhysicalPersonItemResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonItems";
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