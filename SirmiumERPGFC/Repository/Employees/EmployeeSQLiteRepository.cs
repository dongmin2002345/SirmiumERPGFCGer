﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeSQLiteRepository
    {
        #region SQL

        public static string EmployeeTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Employees " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(2048) NULL, " +
            "EmployeeCode NVARCHAR(2048) NULL, " +
            "Name NVARCHAR(2048) NULL, " +
            "SurName NVARCHAR(2048) NULL, " +
            "ConstructionSiteCode NVARCHAR(2048) NULL, " +
            "ConstructionSiteName NVARCHAR(2048) NULL, " +

            "DateOfBirth DATETIME NULL, " +
            "Gender INTEGER NOT NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID, " +
            "CountryCode NVARCHAR(2048) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "RegionId INTEGER NULL, " +
            "RegionIdentifier GUID, " +
            "RegionCode NVARCHAR(2048) NULL, " +
            "RegionName NVARCHAR(2048) NULL, " +
            "MunicipalityId INTEGER NULL, " +
            "MunicipalityIdentifier GUID, " +
            "MunicipalityCode NVARCHAR(2048) NULL, " +
            "MunicipalityName NVARCHAR(2048) NULL, " +
            "CityId INTEGER NULL, " +
            "CityIdentifier GUID, " +
            "CityCode NVARCHAR(2048) NULL, " +
            "CityName NVARCHAR(2048) NULL, " +
            "Address NVARCHAR(2048) NULL, " +

            "PassportCountryId INTEGER NULL, " +
            "PassportCountryIdentifier GUID, " +
            "PassportCountryCode NVARCHAR(2048) NULL, " +
            "PassportCountryName NVARCHAR(2048) NULL, " +
            "PassportCityId INTEGER NULL, " +
            "PassportCityIdentifier GUID, " +
            "PassportCityCode NVARCHAR(2048) NULL, " +
            "PassportCityName NVARCHAR(2048) NULL, " +
            "Passport NVARCHAR(2048) NULL, " +
            "PassportMup NVARCHAR(2048) NULL, " +
            "VisaFrom DATETIME NULL, " +
            "VisaTo DATETIME NULL, " +
            "IsVisaApplied BOOL NULL, " +
            "ResidenceCountryId INTEGER NULL, " +
            "ResidenceCountryIdentifier GUID, " +
            "ResidenceCountryCode NVARCHAR(2048) NULL, " +
            "ResidenceCountryName NVARCHAR(2048) NULL, " +
            "ResidenceCityId INTEGER NULL, " +
            "ResidenceCityIdentifier GUID, " +
            "ResidenceCityCode NVARCHAR(2048) NULL, " +
            "ResidenceCityName NVARCHAR(2048) NULL, " +
            "ResidenceAddress NVARCHAR(2048) NULL, " +


            "EmbassyDate DATETIME NULL, " +
            "VisaDate DATETIME NULL, " +
            "VisaValidFrom DATETIME NULL, " +
            "VisaValidTo DATETIME NULL, " +
            "WorkPermitFrom DATETIME NULL, " +
            "WorkPermitTo DATETIME NULL, " +

            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " +
            "Code, EmployeeCode, Name, SurName, ConstructionSiteCode, ConstructionSiteName, " +
            "DateOfBirth, Gender, CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, CityId, CityIdentifier, CityCode, CityName, Address, " +
            "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
            "Passport, PassportMup, VisaFrom, VisaTo, IsVisaApplied, " +
            "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
            "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, ResidenceAddress, " +
            "EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Employees " +
            "(Id, ServerId, Identifier, " +
            "Code, EmployeeCode, Name, SurName, ConstructionSiteCode, ConstructionSiteName, " +
            "DateOfBirth, Gender, CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, CityId, CityIdentifier, CityCode, CityName, Address, " +
            "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
            "Passport, PassportMup, VisaFrom, VisaTo, IsVisaApplied, " +
            "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
            "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, ResidenceAddress, " +
            "EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@Code, @EmployeeCode, @Name, @SurName, @ConstructionSiteCode, @ConstructionSiteName, " +
            "@DateOfBirth, @Gender, @CountryId, @CountryIdentifier, @CountryCode, @CountryName, @RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, @CityId, @CityIdentifier, @CityCode, @CityName, @Address, " +
            "@PassportCountryId, @PassportCountryIdentifier, @PassportCountryCode, @PassportCountryName, @PassportCityId, @PassportCityIdentifier, @PassportCityCode, @PassportCityName, " +
            "@Passport, @PassportMup, @VisaFrom, @VisaTo, @IsVisaApplied, " +
            "@ResidenceCountryId, @ResidenceCountryIdentifier, @ResidenceCountryCode, @ResidenceCountryName, " +
            "@ResidenceCityId, @ResidenceCityIdentifier, @ResidenceCityCode, @ResidenceCityName, @ResidenceAddress, " +
            "@EmbassyDate, @VisaDate, @VisaValidFrom, @VisaValidTo, @WorkPermitFrom, @WorkPermitTo, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";
        #endregion

        #region Helper methods
        private static EmployeeViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeViewModel dbEntry = new EmployeeViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.EmployeeCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ConstructionSiteCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ConstructionSiteName = SQLiteHelper.GetString(query, ref counter);

            dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Gender = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
            dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
            dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);

            dbEntry.PassportCountry = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.PassportCity = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
            dbEntry.PassportMup = SQLiteHelper.GetString(query, ref counter);
            dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.IsVisaApplied = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.ResidenceCountry = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.ResidenceCity = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.ResidenceAddress = SQLiteHelper.GetString(query, ref counter);

            dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.VisaDate = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.VisaValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.VisaValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);

            //dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeViewModel Employee)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", Employee.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", Employee.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)Employee.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)Employee.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)Employee.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@SurName", ((object)Employee.SurName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)Employee.ConstructionSiteCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)Employee.ConstructionSiteName) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@DateOfBirth", ((object)Employee.DateOfBirth) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Gender", ((object)Employee.Gender) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)Employee.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)Employee.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)Employee.Country?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)Employee.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionId", ((object)Employee.Region?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)Employee.Region?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)Employee.Region?.RegionCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionName", ((object)Employee.Region?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)Employee.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)Employee.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)Employee.Municipality?.MunicipalityCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)Employee.Municipality?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityId", ((object)Employee.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)Employee.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)Employee.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)Employee.City?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)Employee.Address) ?? DBNull.Value);


            insertCommand.Parameters.AddWithValue("@PassportCountryId", ((object)Employee.PassportCountry?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryIdentifier", ((object)Employee.PassportCountry?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryCode", ((object)Employee.PassportCountry?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryName", ((object)Employee.PassportCountry?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityId", ((object)Employee.PassportCity?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityIdentifier", ((object)Employee.PassportCity?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityCode", ((object)Employee.PassportCity?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityName", ((object)Employee.PassportCity?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Passport", ((object)Employee.Passport) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportMup", ((object)Employee.PassportMup) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaFrom", ((object)Employee.VisaFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaTo", ((object)Employee.VisaTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsVisaApplied", ((object)Employee.IsVisaApplied) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@ResidenceCountryId", ((object)Employee.ResidenceCountry?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryIdentifier", ((object)Employee.ResidenceCountry?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryCode", ((object)Employee.ResidenceCountry?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryName", ((object)Employee.ResidenceCountry?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityId", ((object)Employee.ResidenceCity?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityIdentifier", ((object)Employee.ResidenceCity?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityCode", ((object)Employee.ResidenceCity?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityName", ((object)Employee.ResidenceCity?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceAddress", ((object)Employee.ResidenceAddress) ?? DBNull.Value);


            insertCommand.Parameters.AddWithValue("@EmbassyDate", ((object)Employee.EmbassyDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaDate", ((object)Employee.VisaDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaValidFrom", ((object)Employee.VisaValidFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaValidTo", ((object)Employee.VisaValidTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@WorkPermitFrom", ((object)Employee.WorkPermitFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@WorkPermitTo", ((object)Employee.WorkPermitTo) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", Employee.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)Employee.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public EmployeeListResponse GetEmployeesByPage(int companyId, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        "SELECT emp.ServerId, emp.Identifier, " +
                        "emp.Code, emp.EmployeeCode, emp.Name, emp.SurName, emp.ConstructionSiteCode, emp.ConstructionSiteName, " +
                        "emp.DateOfBirth, emp.Gender, emp.CountryId, emp.CountryIdentifier, emp.CountryCode, emp.CountryName, emp.RegionId, emp.RegionIdentifier, emp.RegionCode, emp.RegionName, " +
                        "emp.MunicipalityId, emp.MunicipalityIdentifier, emp.MunicipalityCode, emp.MunicipalityName, emp.CityId, emp.CityIdentifier, emp.CityCode, emp.CityName, emp.Address, " +
                        "emp.PassportCountryId, emp.PassportCountryIdentifier, emp.PassportCountryCode, emp.PassportCountryName, emp.PassportCityId, emp.PassportCityIdentifier, emp.PassportCityCode, emp.PassportCityName, " +
                        "emp.Passport, emp.PassportMup, emp.VisaFrom, emp.VisaTo, emp.IsVisaApplied, " +
                        "emp.ResidenceCountryId, emp.ResidenceCountryIdentifier, emp.ResidenceCountryCode, emp.ResidenceCountryName, " +
                        "emp.ResidenceCityId, emp.ResidenceCityIdentifier, emp.ResidenceCityCode, emp.ResidenceCityName, emp.ResidenceAddress, " +
                        "emp.EmbassyDate, emp.VisaDate, emp.VisaValidFrom, emp.VisaValidTo, emp.WorkPermitFrom, emp.WorkPermitTo, " +
                        "empBP.BusinessPartnerId, empBP.BusinessPartnerIdentifier, empBP.BusinessPartnerCode, empBP.BusinessPartnerName, empBP.BusinessPartnerInternalCode, empBP.BusinessPartnerNameGer, " + 
                        "emp.IsSynced, emp.UpdatedAt, emp.CreatedById, emp.CreatedByName, emp.CompanyId, emp.CompanyName " +
                        "FROM Employees emp " +
                        "LEFT JOIN EmployeeByBusinessPartners empBP ON emp.Identifier = empBP.EmployeeIdentifier " +
                        "WHERE (@Name IS NULL OR @Name = '' OR emp.Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR emp.SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR emp.Passport LIKE @Passport) " +
                        "AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR emp.ConstructionSiteCode LIKE @ConstructionSite OR emp.ConstructionSiteName LIKE @ConstructionSite) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR emp.EmployeeCode LIKE @EmployeeCode) " +
                        "AND emp.CompanyId = @CompanyId " +
                        "ORDER BY emp.IsSynced, emp.Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(EmployeeSearchObject.Search_ConstructionSite) ? "%" + EmployeeSearchObject.Search_ConstructionSite + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeViewModel dbEntry = new EmployeeViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.EmployeeCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ConstructionSiteCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ConstructionSiteName = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Gender = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
                        dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.PassportCountry = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.PassportCity = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PassportMup = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsVisaApplied = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.ResidenceCountry = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.ResidenceCity = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.ResidenceAddress = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaDate = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.VisaValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.VisaValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);

                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);

                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Employees.Add(dbEntry);

                    }
                    response.Employees = Employees;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees emp " +
                        "LEFT JOIN EmployeeByBusinessPartners empBP ON emp.Identifier = empBP.EmployeeIdentifier " +
                        "WHERE (@Name IS NULL OR @Name = '' OR emp.Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR emp.SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR emp.Passport LIKE @Passport) " +
                        "AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR emp.ConstructionSiteCode LIKE @ConstructionSite OR emp.ConstructionSiteName LIKE @ConstructionSite) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR emp.EmployeeCode LIKE @EmployeeCode) " +
                        "AND emp.CompanyId = @CompanyId " +
                        "ORDER BY emp.IsSynced, emp.Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(EmployeeSearchObject.Search_ConstructionSite) ? "%" + EmployeeSearchObject.Search_ConstructionSite + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        public EmployeeListResponse GetEmployeesNotOnConstructionSiteByPage(int companyId, Guid constructionSiteIdentifier, Guid businessPartnerIdentifier, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        //"WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites) " + 
                        "AND Identifier IN (Select EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        EmployeeViewModel dbEntry = Read(query);
                        Employees.Add(dbEntry);
                    }

                    response.Employees = Employees;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees " +
                         //"WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites) " +
                        "AND Identifier IN (Select EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        public EmployeeListResponse GetEmployeesOnConstructionSiteByPage(int companyId, Guid constructionSiteIdentifier, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE Identifier IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    { 
                    EmployeeViewModel dbEntry = Read(query);
                    Employees.Add(dbEntry);
                    }

                    response.Employees = Employees;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees " +
                        "WHERE Identifier IN (SELECT EmployeeIdentifier FROM EmployeeByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                         "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        #region EmployeeByBusinessPartner (BusinessPartnerEmployee)

        public EmployeeListResponse GetEmployeesNotOnBusinessPartnerByPage(int companyId, Guid businessPartnerIdentifier, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByBusinessPartners) " +
                        //"WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        EmployeeViewModel dbEntry = Read(query);
                        Employees.Add(dbEntry);
                    }

                    response.Employees = Employees;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees " +
                        "WHERE Identifier NOT IN (SELECT EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        public EmployeeListResponse GetEmployeesOnBusinessPartnerByPage(int companyId, Guid businessPartnerIdentifier, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE Identifier IN (SELECT EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        EmployeeViewModel dbEntry = Read(query);
                        Employees.Add(dbEntry);
                    }

                    response.Employees = Employees;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees " +
                        "WHERE Identifier IN (SELECT EmployeeIdentifier FROM EmployeeByBusinessPartners WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@EmployeeCode IS NULL OR @EmployeeCode = '' OR EmployeeCode LIKE @EmployeeCode) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeSearchObject.Search_EmployeeCode) != null ? "%" + EmployeeSearchObject.Search_EmployeeCode + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        #endregion

        public EmployeeResponse GetEmployee(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();
            EmployeeViewModel Employee = new EmployeeViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        EmployeeViewModel dbEntry = Read(query);
                        Employee = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Employee = new EmployeeViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employee = Employee;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeService EmployeeService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeRequest request = new SyncEmployeeRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeListResponse response = EmployeeService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Employees?.Count ?? 0;
                    List<EmployeeViewModel> employeesFromDB = response.Employees;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Employees WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employee in employeesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employee.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employee.IsActive)
                                {
                                    employee.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employee);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Employees WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Employees WHERE CompanyId = @CompanyId", db);
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

        public EmployeeResponse Create(EmployeeViewModel Employee)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, Employee);
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

        public EmployeeResponse Delete(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Employees WHERE Identifier = @Identifier";
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

        public EmployeeResponse DeleteAll()
        {
            EmployeeResponse response = new EmployeeResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Employees";
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
