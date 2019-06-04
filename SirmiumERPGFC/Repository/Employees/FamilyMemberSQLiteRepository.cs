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
    public class FamilyMemberSQLiteRepository
    {
        public static string FamilyMemberTableCreatePart =
                "CREATE TABLE IF NOT EXISTS FamilyMembers " +
                 "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                 "ServerId INTEGER NULL, " +
                 "Identifier GUID, " +
                 "Code NVARCHAR(48) NULL, " +
                 "Name NVARCHAR(48) NULL, " +
                 "IsSynced BOOL NULL, " +
                 "UpdatedAt DATETIME NULL, " +
                 "CreatedById INTEGER NULL, " +
                 "CreatedByName NVARCHAR(2048) NULL, " +
                 "CompanyId INTEGER NULL, " +
                 "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO FamilyMembers " +
            "(Id, ServerId, Identifier, Code, Name, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public FamilyMemberListResponse GetFamilyMembersByPage(int companyId, FamilyMemberViewModel familyMemberSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            List<FamilyMemberViewModel> Remedies = new List<FamilyMemberViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM FamilyMembers " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)familyMemberSearchObject.Search_Code) != null ? "%" + familyMemberSearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)familyMemberSearchObject.Search_Name) != null ? "%" + familyMemberSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        FamilyMemberViewModel dbEntry = new FamilyMemberViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Remedies.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM FamilyMembers " +
                        "WHERE ((@Code IS NULL OR @Code = '' OR Name LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name)) " +            
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)familyMemberSearchObject.Search_Code) != null ? "%" + familyMemberSearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)familyMemberSearchObject.Search_Name) != null ? "%" + familyMemberSearchObject.Search_Name + "%" : "");
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
                    response.FamilyMembers = new List<FamilyMemberViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.FamilyMembers = Remedies;
            return response;
        }

        public FamilyMemberListResponse GetFamilyMembersForPopup(int companyId, string filterString)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            List<FamilyMemberViewModel> familyMembers = new List<FamilyMemberViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM FamilyMembers " +
                        "WHERE ((@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "OR (@Name IS NULL OR @Name = '' OR Name LIKE @Name)) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        FamilyMemberViewModel dbEntry = new FamilyMemberViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        familyMembers.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.FamilyMembers = new List<FamilyMemberViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.FamilyMembers = familyMembers;
            return response;
        }

        public FamilyMemberResponse GetFamilyMember(Guid identifier)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            FamilyMemberViewModel FamilyMember = new FamilyMemberViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM FamilyMembers " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        FamilyMemberViewModel dbEntry = new FamilyMemberViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        FamilyMember = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.FamilyMember = new FamilyMemberViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.FamilyMember = FamilyMember;
            return response;
        }

        public void Sync(IFamilyMemberService familyMemberService, Action<int, int> callback = null)
        {
            try
            {
                SyncFamilyMemberRequest request = new SyncFamilyMemberRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                FamilyMemberListResponse response = familyMemberService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.FamilyMembers?.Count ?? 0;
                    List<FamilyMemberViewModel> familyMembersFromDB = response.FamilyMembers;
                    foreach (var FamilyMember in familyMembersFromDB.OrderBy(x => x.Id))
                    {
                        ThreadPool.QueueUserWorkItem((k) =>
                        {
                            Delete(FamilyMember.Identifier);
                            if (FamilyMember.IsActive)
                            {
                                FamilyMember.IsSynced = true;
                                Create(FamilyMember);
                                syncedItems++;
                                callback?.Invoke(syncedItems, toSync);
                            }
                        });
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from FamilyMembers WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from FamilyMembers WHERE CompanyId = @CompanyId", db);
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

        public FamilyMemberResponse Create(FamilyMemberViewModel FamilyMember)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", FamilyMember.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", FamilyMember.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)FamilyMember.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)FamilyMember.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", FamilyMember.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)FamilyMember.UpdatedAt) ?? DBNull.Value);
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

        public FamilyMemberResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE FamilyMembers SET " +
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

        public FamilyMemberResponse Delete(Guid identifier)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM FamilyMembers WHERE Identifier = @Identifier";
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

        public FamilyMemberResponse DeleteAll()
        {
            FamilyMemberResponse response = new FamilyMemberResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM FamilyMembers";
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