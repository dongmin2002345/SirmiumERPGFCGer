using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.CalendarAssignments
{
    public class CalendarAssignmentSQLiteRepository
    {
        //#region SQL
        //public static string CalendarAssignmentTableCreatePart =
        //    "CREATE TABLE IF NOT EXISTS CalendarAssignments " +
        //    "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        //    "ServerId INTEGER NULL, " +
        //    "Identifier GUID, " +
        //    "Name NVARCHAR(2048) NULL, " +
        //    "Description NVARCHAR(2048) NULL, " +
        //    "Date DATETIME NOT NULL, " +

        //    "IsSynced BOOL NULL, " +
        //    "UpdatedAt DATETIME NULL, " +
        //    "AssignedToId INTEGER NULL, " +
        //    "AssignedToName NVARCHAR(2048) NULL, " +
        //    "CreatedById INTEGER NULL, " +
        //    "CreatedByName NVARCHAR(2048) NULL, " +
        //    "CompanyId INTEGER NULL, " +
        //    "CompanyName NVARCHAR(2048) NULL)";

        //public string SqlCommandSelectPart =
        //    "SELECT ServerId, Identifier, " +
        //    "Name, Description, Date, " +
        //    "IsSynced, UpdatedAt, AssignedToId, AssignedToName, CreatedById, CreatedByName, CompanyId, CompanyName ";

        //public string SqlCommandInsertPart = "INSERT INTO CalendarAssignments " +
        //    "(Id, ServerId, Identifier, " +
        //    "Name, Description, Date, " +
        //    "IsSynced, UpdatedAt, AssignedToId, AssignedToName, CreatedById, CreatedByName, CompanyId, CompanyName) " +

        //    "VALUES (NULL, @ServerId, @Identifier, " +
        //    "@Name, @Description, @Date, " +
        //    "@IsSynced, @UpdatedAt, @AssignedToId, @AssignedToName, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";
        //#endregion

        //#region Helper methods
        //private static CalendarAssignmentViewModel Read(SqliteDataReader query)
        //{
        //    int counter = 0;
        //    CalendarAssignmentViewModel dbEntry = new CalendarAssignmentViewModel();
        //    dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //    dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //    dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
        //    dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
        //    dbEntry.Date = SQLiteHelper.GetDateTime(query, ref counter);

        //    dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //    dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //    dbEntry.AssignedTo = SQLiteHelper.GetCreatedBy(query, ref counter);
        //    dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //    dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //    return dbEntry;
        //}

        //private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, CalendarAssignmentViewModel CalendarAssignment)
        //{
        //    insertCommand.Parameters.AddWithValue("@ServerId", CalendarAssignment.Id);
        //    insertCommand.Parameters.AddWithValue("@Identifier", CalendarAssignment.Identifier);
        //    insertCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignment.Name) ?? DBNull.Value);
        //    insertCommand.Parameters.AddWithValue("@Description", ((object)CalendarAssignment.Description) ?? DBNull.Value);
        //    insertCommand.Parameters.AddWithValue("@Date", ((object)CalendarAssignment.Date) ?? DBNull.Value);
            
        //    insertCommand.Parameters.AddWithValue("@IsSynced", CalendarAssignment.IsSynced);
        //    insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)CalendarAssignment.UpdatedAt) ?? DBNull.Value);
        //    insertCommand.Parameters.AddWithValue("@AssignedToId", ((object)CalendarAssignment?.AssignedTo?.Id) ?? DBNull.Value);
        //    insertCommand.Parameters.AddWithValue("@AssignedToName", (object)(CalendarAssignment?.AssignedTo?.FirstName + " " + CalendarAssignment?.AssignedTo?.LastName) ?? DBNull.Value);
        //    insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
        //    insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
        //    insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
        //    insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

        //    return insertCommand;
        //}

        //#endregion

        //#region Read

        //public CalendarAssignmentListResponse GetCalendarAssignmentsByPage(int companyId, CalendarAssignmentViewModel CalendarAssignmentSearchObject, int currentPage = 1, int itemsPerPage = 50)
        //{
        //    CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
        //    List<CalendarAssignmentViewModel> CalendarAssignments = new List<CalendarAssignmentViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                "SELECT emp.ServerId, emp.Identifier, " +
        //                "emp.Code, emp.CalendarAssignmentCode, emp.Name, emp.SurName, emp.ConstructionSiteCode, emp.ConstructionSiteName, " +
        //                "emp.DateOfBirth, emp.Gender, emp.CountryId, emp.CountryIdentifier, emp.CountryCode, emp.CountryName, emp.RegionId, emp.RegionIdentifier, emp.RegionCode, emp.RegionName, " +
        //                "emp.MunicipalityId, emp.MunicipalityIdentifier, emp.MunicipalityCode, emp.MunicipalityName, emp.CityId, emp.CityIdentifier, emp.CityCode, emp.CityName, emp.Address, " +
        //                "emp.PassportCountryId, emp.PassportCountryIdentifier, emp.PassportCountryCode, emp.PassportCountryName, emp.PassportCityId, emp.PassportCityIdentifier, emp.PassportCityCode, emp.PassportCityName, " +
        //                "emp.Passport, emp.PassportMup, emp.VisaFrom, emp.VisaTo, " +
        //                "emp.ResidenceCountryId, emp.ResidenceCountryIdentifier, emp.ResidenceCountryCode, emp.ResidenceCountryName, " +
        //                "emp.ResidenceCityId, emp.ResidenceCityIdentifier, emp.ResidenceCityCode, emp.ResidenceCityName, emp.ResidenceAddress, " +
        //                "emp.EmbassyDate, emp.VisaDate, emp.VisaValidFrom, emp.VisaValidTo, emp.WorkPermitFrom, emp.WorkPermitTo, " +
        //                "empBP.BusinessPartnerId, empBP.BusinessPartnerIdentifier, empBP.BusinessPartnerCode, empBP.BusinessPartnerName, empBP.BusinessPartnerInternalCode, empBP.BusinessPartnerNameGer, " + 
        //                "emp.IsSynced, emp.UpdatedAt, emp.CreatedById, emp.CreatedByName, emp.CompanyId, emp.CompanyName " +
        //                "FROM CalendarAssignments emp " +
        //                "LEFT JOIN CalendarAssignmentByBusinessPartners empBP ON emp.Identifier = empBP.CalendarAssignmentIdentifier " +
        //                "WHERE (@Name IS NULL OR @Name = '' OR emp.Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR emp.SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR emp.Passport LIKE @Passport) " +
        //                "AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR emp.ConstructionSiteCode LIKE @ConstructionSite OR emp.ConstructionSiteName LIKE @ConstructionSite) " +
        //                "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR emp.CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND emp.CompanyId = @CompanyId " +
        //                "ORDER BY emp.IsSynced, emp.Id DESC " +
        //                "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(CalendarAssignmentSearchObject.Search_ConstructionSite) ? "%" + CalendarAssignmentSearchObject.Search_ConstructionSite + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
        //            selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                CalendarAssignmentViewModel dbEntry = Read(query);
        //                CalendarAssignments.Add(dbEntry);

        //            }
        //            response.CalendarAssignments = CalendarAssignments;

        //            selectCommand = new SqliteCommand(
        //                "SELECT Count(*) " +
        //                "FROM CalendarAssignments emp " +
        //                "LEFT JOIN CalendarAssignmentByBusinessPartners empBP ON emp.Identifier = empBP.CalendarAssignmentIdentifier " +
        //                "WHERE (@Name IS NULL OR @Name = '' OR emp.Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR emp.SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR emp.Passport LIKE @Passport) " +
        //                "AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR emp.ConstructionSiteCode LIKE @ConstructionSite OR emp.ConstructionSiteName LIKE @ConstructionSite) " +
        //                "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR emp.CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND emp.CompanyId = @CompanyId " +
        //                "ORDER BY emp.IsSynced, emp.Id DESC;", db);
                    
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(CalendarAssignmentSearchObject.Search_ConstructionSite) ? "%" + CalendarAssignmentSearchObject.Search_ConstructionSite + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //                response.TotalItems = query.GetInt32(0);
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.CalendarAssignments = CalendarAssignments;
        //    return response;
        //}

        //public CalendarAssignmentListResponse GetCalendarAssignmentsNotOnConstructionSiteByPage(int companyId, Guid constructionSiteIdentifier, Guid businessPartnerIdentifier, CalendarAssignmentViewModel CalendarAssignmentSearchObject, int currentPage = 1, int itemsPerPage = 50)
        //{
        //    CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
        //    List<CalendarAssignmentViewModel> CalendarAssignments = new List<CalendarAssignmentViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM CalendarAssignments " +
        //                //"WHERE Identifier NOT IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
        //                "WHERE Identifier NOT IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites) " + 
        //                "AND Identifier IN (Select CalendarAssignmentIdentifier FROM CalendarAssignmentByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
        //                "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
        //                "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND CompanyId = @CompanyId " +
        //                "ORDER BY IsSynced, Id DESC " +
        //                "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
        //            selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
        //            selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
        //            selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                CalendarAssignmentViewModel dbEntry = Read(query);
        //                CalendarAssignments.Add(dbEntry);
        //            }

        //            response.CalendarAssignments = CalendarAssignments;

        //            selectCommand = new SqliteCommand(
        //                "SELECT Count(*) " +
        //                "FROM CalendarAssignments " +
        //                 //"WHERE Identifier NOT IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
        //                "WHERE Identifier NOT IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites) " +
        //                "AND Identifier IN (Select CalendarAssignmentIdentifier FROM CalendarAssignmentByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
        //                "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
        //                "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND CompanyId = @CompanyId;", db);
                    
        //            selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
        //            selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //                response.TotalItems = query.GetInt32(0);
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.CalendarAssignments = CalendarAssignments;
        //    return response;
        //}

        //public CalendarAssignmentListResponse GetCalendarAssignmentsOnConstructionSiteByPage(int companyId, Guid constructionSiteIdentifier, CalendarAssignmentViewModel CalendarAssignmentSearchObject, int currentPage = 1, int itemsPerPage = 50)
        //{
        //    CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
        //    List<CalendarAssignmentViewModel> CalendarAssignments = new List<CalendarAssignmentViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM CalendarAssignments " +
        //                "WHERE Identifier IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
        //                "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
        //                "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND CompanyId = @CompanyId " +
        //                "ORDER BY IsSynced, Id DESC " +
        //                "LIMIT @ItemsPerPage OFFSET @Offset;", db);

        //            selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
        //            selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            { 
        //            CalendarAssignmentViewModel dbEntry = Read(query);
        //            CalendarAssignments.Add(dbEntry);
        //            }

        //            response.CalendarAssignments = CalendarAssignments;

        //            selectCommand = new SqliteCommand(
        //                "SELECT Count(*) " +
        //                "FROM CalendarAssignments " +
        //                "WHERE Identifier IN (SELECT CalendarAssignmentIdentifier FROM CalendarAssignmentByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
        //                "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
        //                "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
        //                 "AND (@CalendarAssignmentCode IS NULL OR @CalendarAssignmentCode = '' OR CalendarAssignmentCode LIKE @CalendarAssignmentCode) " +
        //                "AND CompanyId = @CompanyId;", db);
                    
        //            selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignmentSearchObject.SearchBy_Name) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@SurName", ((object)CalendarAssignmentSearchObject.SearchBy_SurName) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_SurName + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@Passport", ((object)CalendarAssignmentSearchObject.SearchBy_Passport) != null ? "%" + CalendarAssignmentSearchObject.SearchBy_Passport + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CalendarAssignmentCode", ((object)CalendarAssignmentSearchObject.Search_CalendarAssignmentCode) != null ? "%" + CalendarAssignmentSearchObject.Search_CalendarAssignmentCode + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //                response.TotalItems = query.GetInt32(0);
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.CalendarAssignments = CalendarAssignments;
        //    return response;
        //}

        //public CalendarAssignmentResponse GetCalendarAssignment(Guid identifier)
        //{
        //    CalendarAssignmentResponse response = new CalendarAssignmentResponse();
        //    CalendarAssignmentViewModel CalendarAssignment = new CalendarAssignmentViewModel();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM CalendarAssignments " +
        //                "WHERE Identifier = @Identifier;", db);
        //            selectCommand.Parameters.AddWithValue("@Identifier", identifier);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //            {
        //                CalendarAssignmentViewModel dbEntry = Read(query);
        //                CalendarAssignment = dbEntry;
        //            }
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.CalendarAssignment = new CalendarAssignmentViewModel();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.CalendarAssignment = CalendarAssignment;
        //    return response;
        //}

        //#endregion

        //#region Sync

        //public void Sync(ICalendarAssignmentService CalendarAssignmentService, Action<int, int> callback = null)
        //{
        //    try
        //    {
        //        SyncCalendarAssignmentRequest request = new SyncCalendarAssignmentRequest();
        //        request.CompanyId = MainWindow.CurrentCompanyId;
        //        request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

        //        int toSync = 0;
        //        int syncedItems = 0;

        //        CalendarAssignmentListResponse response = CalendarAssignmentService.Sync(request);
        //        if (response.Success)
        //        {
        //            toSync = response?.CalendarAssignments?.Count ?? 0;
        //            List<CalendarAssignmentViewModel> employeesFromDB = response.CalendarAssignments;

        //            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
        //            {
        //                db.Open();
        //                using (var transaction = db.BeginTransaction())
        //                {
        //                    SqliteCommand deleteCommand = db.CreateCommand();
        //                    deleteCommand.CommandText = "DELETE FROM CalendarAssignments WHERE Identifier = @Identifier";

        //                    SqliteCommand insertCommand = db.CreateCommand();
        //                    insertCommand.CommandText = SqlCommandInsertPart;

        //                    foreach (var employee in employeesFromDB)
        //                    {
        //                        deleteCommand.Parameters.AddWithValue("@Identifier", employee.Identifier);
        //                        deleteCommand.ExecuteNonQuery();
        //                        deleteCommand.Parameters.Clear();

        //                        if (employee.IsActive)
        //                        {
        //                            employee.IsSynced = true;

        //                            insertCommand = AddCreateParameters(insertCommand, employee);
        //                            insertCommand.ExecuteNonQuery();
        //                            insertCommand.Parameters.Clear();

        //                            syncedItems++;
        //                            callback?.Invoke(syncedItems, toSync);
        //                        }
        //                    }

        //                    transaction.Commit();
        //                }
        //                db.Close();
        //            }
        //        }
        //        else
        //            throw new Exception(response.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        MainWindow.ErrorMessage = ex.Message;
        //    }
        //}

        //public DateTime? GetLastUpdatedAt(int companyId)
        //{
        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from CalendarAssignments WHERE CompanyId = @CompanyId", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            SqliteDataReader query = selectCommand.ExecuteReader();
        //            int count = query.Read() ? query.GetInt32(0) : 0;

        //            if (count == 0)
        //                return null;
        //            else
        //            {
        //                selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from CalendarAssignments WHERE CompanyId = @CompanyId", db);
        //                selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //                query = selectCommand.ExecuteReader();
        //                if (query.Read())
        //                {
        //                    int counter = 0;
        //                    return SQLiteHelper.GetDateTimeNullable(query, ref counter);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MainWindow.ErrorMessage = ex.Message;
        //        }
        //        db.Close();
        //    }
        //    return null;
        //}

        //#endregion

        //#region Create

        //public CalendarAssignmentResponse Create(CalendarAssignmentViewModel CalendarAssignment)
        //{
        //    CalendarAssignmentResponse response = new CalendarAssignmentResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = db.CreateCommand();
        //        insertCommand.CommandText = SqlCommandInsertPart;

        //        try
        //        {
        //            insertCommand = AddCreateParameters(insertCommand, CalendarAssignment);
        //            insertCommand.ExecuteNonQuery();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //#endregion

        //#region Delete

        //public CalendarAssignmentResponse Delete(Guid identifier)
        //{
        //    CalendarAssignmentResponse response = new CalendarAssignmentResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        //Use parameterized query to prevent SQL injection attacks
        //        insertCommand.CommandText = "DELETE FROM CalendarAssignments WHERE Identifier = @Identifier";
        //        insertCommand.Parameters.AddWithValue("@Identifier", identifier);
               
        //        try
        //        {
        //            insertCommand.ExecuteNonQuery();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public CalendarAssignmentResponse DeleteAll()
        //{
        //    CalendarAssignmentResponse response = new CalendarAssignmentResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM CalendarAssignments";
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

        //#endregion
    }
}
