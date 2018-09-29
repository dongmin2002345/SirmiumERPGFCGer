using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerSQLiteRepository
    {
        public static string BusinessPartnerTableCreatePart =
                "CREATE TABLE IF NOT EXISTS BusinessPartners " +
                "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "ServerId INTEGER NULL, " +
                "Identifier GUID, " +
                "Code NVARCHAR(48) NULL, " +
                "Name NVARCHAR(48) NULL, " +
                "Director NVARCHAR(2048) NULL, " +
                "Address NVARCHAR(2048) NULL, " +
                "InoAddress NVARCHAR(2048) NULL, " +
                "PIB NVARCHAR(2048) NULL, " +
                "MatCode NVARCHAR(2048) NULL, " +
                "Mobile NVARCHAR(2048) NULL, " +
                "Phone NVARCHAR(2048) NULL, " +
                "Email NVARCHAR(2048) NULL, " +
                "ActivityCode NVARCHAR(2048) NULL, " +
                "BankAccountNumber NVARCHAR(2048) NULL, " +
                "OpeningDate DATETIME NULL, " +
                "BranchOpeningDate DATETIME NULL, " +
                "IsSynced BOOL NULL, " +
                "UpdatedAt DATETIME NULL, " +
                "CreatedById INTEGER NULL, " +
                "CreatedByName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, Director, Address, InoAddress, " +
            "PIB, MatCode, Mobile, Phone, Email, ActivityCode, BankAccountNumber, OpeningDate, BranchOpeningDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartners " +
            "(Id, ServerId, Identifier, Code, Name, Director,Address, InoAddress, " +
            "PIB, MatCode, Mobile, Phone, Email, ActivityCode, BankAccountNumber, OpeningDate, BranchOpeningDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName) " +
            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @Director, @Address, @InoAddress, " +
            "@PIB, @MatCode, @Mobile, @Phone, @Email, @ActivityCode, @BankAccountNumber, @OpeningDate, @BranchOpeningDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName)";

        public BusinessPartnerListResponse GetBusinessPartnersByPage(BusinessPartnerViewModel businessPartnerSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> BusinessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerSearchObject.SearchBy_BusinessPartnerName) != null ? "%" + businessPartnerSearchObject.SearchBy_BusinessPartnerName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerViewModel dbEntry = new BusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Director = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InoAddress = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MatCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ActivityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BankAccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.OpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.BranchOpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        BusinessPartners.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerSearchObject.SearchBy_BusinessPartnerName) != null ? "%" + businessPartnerSearchObject.SearchBy_BusinessPartnerName + "%" : "");

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartners = new List<BusinessPartnerViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartners = BusinessPartners;
            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersForPopup(string filterString)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> BusinessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerViewModel dbEntry = new BusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Director = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InoAddress = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MatCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ActivityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BankAccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.OpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.BranchOpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        BusinessPartners.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartners = new List<BusinessPartnerViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartners = BusinessPartners;
            return response;
        }

        public BusinessPartnerListResponse GetUnSyncedBusinessPartners()
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> BusinessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerViewModel dbEntry = new BusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Director = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InoAddress = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MatCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ActivityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BankAccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.OpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.BranchOpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        BusinessPartners.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartners = new List<BusinessPartnerViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartners = BusinessPartners;
            return response;
        }

        public BusinessPartnerResponse GetBusinessPartner(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            BusinessPartnerViewModel BusinessPartner = new BusinessPartnerViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerViewModel dbEntry = new BusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Director = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InoAddress = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MatCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ActivityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BankAccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.OpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.BranchOpeningDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        BusinessPartner = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartner = new BusinessPartnerViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartner = BusinessPartner;
            return response;
        }

        public string GetNewCodeValue()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartners", db);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return "LEK-00001";
                    else
                    {
                        selectCommand = new SqliteCommand(
                            "SELECT Code from BusinessPartners " +
                            "WHERE Id = (SELECT MAX(Id) FROM BusinessPartners)", db);
                        query = selectCommand.ExecuteReader();

                        string activeCode = query.Read() ? query.GetString(0) : "";
                        int intValue = Int32.Parse(activeCode.Replace("LEK-", ""));
                        return "LEK-" + (intValue + 1).ToString("00000");
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return "";
        }

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartners", db);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartners", db);
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

        public BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartner.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartner.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", businessPartner.Code);
                insertCommand.Parameters.AddWithValue("@Name", ((object)businessPartner.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartner.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", businessPartner.UpdatedAt);
                insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
                insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);

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

        public BusinessPartnerResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartners SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
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

        public BusinessPartnerResponse Delete(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartners WHERE Identifier = @Identifier";
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

        public BusinessPartnerResponse DeleteAll()
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartners";
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
