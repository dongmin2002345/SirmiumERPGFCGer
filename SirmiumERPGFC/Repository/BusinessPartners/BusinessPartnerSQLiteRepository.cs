﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
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
               "PIB NVARCHAR(48) NULL, " +
               "PIO NVARCHAR(48) NULL, " +
               "PDV NVARCHAR(48) NULL, " +
               "IndustryCode NVARCHAR(48) NULL, " +
               "IdentificationNumber NVARCHAR(48) NULL, " +
               "Rebate DOUBLE NULL, " +
               "DueDate INTEGER NULL, " +
               "WebSite NVARCHAR(2048) NULL, " +
               "ContactPerson NVARCHAR(2048) NULL, " +
               "IsInPdv BOOL NULL, " +
               "JBKJS NVARCHAR(48) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, PIB, PIO, PDV, IndustryCode, IdentificationNumber, " +
            "Rebate, DueDate, WebSite, ContactPerson, IsInPdv, JBKJS, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartners " +
            "(Id, ServerId, Identifier, Code, Name, PIB, PIO, PDV, IndustryCode, IdentificationNumber, " +
            "Rebate, DueDate, WebSite, ContactPerson, IsInPdv, JBKJS, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @PIB, @PIO, @PDV, @IndustryCode, @IdentificationNumber, " +
            "@Rebate, @DueDate, @WebSite, @ContactPerson, @IsInPdv, @JBKJS, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public BusinessPartnerListResponse GetBusinessPartnersByPage(int companyId, BusinessPartnerViewModel businessPartnerSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> businessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR PIB LIKE @PIB) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerSearchObject?.Search_Name) != null ? "%" + businessPartnerSearchObject?.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)businessPartnerSearchObject?.Search_PIB) != null ? "%" + businessPartnerSearchObject?.Search_PIB + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
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
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartners.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR PIB LIKE @PIB) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerSearchObject?.Search_Name) != null ? "%" + businessPartnerSearchObject?.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)businessPartnerSearchObject?.Search_PIB) != null ? "%" + businessPartnerSearchObject?.Search_PIB + "%" : "");
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
                    response.BusinessPartners = new List<BusinessPartnerViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartners = businessPartners;
            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersForPopup(int companyId, string filterString)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> businessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
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
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartners.Add(dbEntry);
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
            response.BusinessPartners = businessPartners;
            return response;
        }

        public BusinessPartnerListResponse GetUnSyncedBusinessPartners(int companyId)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            List<BusinessPartnerViewModel> businessPartners = new List<BusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartners " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerViewModel dbEntry = new BusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartners.Add(dbEntry);
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
            response.BusinessPartners = businessPartners;
            return response;
        }

        public BusinessPartnerResponse GetBusinessPartner(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            BusinessPartnerViewModel businessPartner = new BusinessPartnerViewModel();

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
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IndustryCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartner = dbEntry;
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
            response.BusinessPartner = businessPartner;
            return response;
        }

        public string GetNewCodeValue(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartners WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return "KOR-00001";
                    else
                    {
                        selectCommand = new SqliteCommand(
                            "SELECT Code from BusinessPartners " +
                            "WHERE Id = (SELECT MAX(Id) FROM BusinessPartners WHERE CompanyId = @CompanyId)", db);
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                        query = selectCommand.ExecuteReader();

                        string activeCode = query.Read() ? query.GetString(0) : "";
                        int intValue = Int32.Parse(activeCode.Replace("KOR-", ""));
                        return "KOR-" + (intValue + 1).ToString("00000");
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

        public void Sync(IBusinessPartnerService bpService)
        {
            SyncBusinessPartnerRequest request = new SyncBusinessPartnerRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            BusinessPartnerListResponse response = bpService.Sync(request);
            if (response.Success)
            {
                List<BusinessPartnerViewModel> businessPartnersFromDB = response.BusinessPartners;
                foreach (var bp in businessPartnersFromDB.OrderBy(x => x.Id))
                {
                    Delete(bp.Identifier);
                    bp.IsSynced = true;
                    Create(bp);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartners WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartners WHERE CompanyId = @CompanyId", db);
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
                insertCommand.Parameters.AddWithValue("@PIB", ((object)businessPartner.PIB) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PIO", ((object)businessPartner.PIO) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PDV", ((object)businessPartner.PDV) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IndustryCode", ((object)businessPartner.IndustryCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IdentificationNumber", ((object)businessPartner.IdentificationNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Rebate", ((object)businessPartner.Rebate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DueDate", ((object)businessPartner.DueDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WebSite", ((object)businessPartner.WebSite) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPerson", ((object)businessPartner.ContactPerson) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsInPdv", ((object)businessPartner.IsInPDV) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@JBKJS", ((object)businessPartner.JBKJS) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartner.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", businessPartner.UpdatedAt);
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

        public BusinessPartnerResponse Update(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                Delete(businessPartner.Identifier);
                Create(businessPartner);
            }
            catch (SqliteException error)
            {
                MainWindow.ErrorMessage = error.Message;
                response.Success = false;
                response.Message = error.Message;
                return response;
            }

            response.Success = true;
            return response;
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
