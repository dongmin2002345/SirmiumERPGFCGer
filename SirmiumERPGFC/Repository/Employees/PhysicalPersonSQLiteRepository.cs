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
	public class PhysicalPersonSQLiteRepository
	{
		public static string PhysicalPersonTableCreatePart =
		   "CREATE TABLE IF NOT EXISTS PhysicalPersons " +
		   "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
		   "ServerId INTEGER NULL, " +
		   "Identifier GUID, " +
		   "Code NVARCHAR(2048) NULL, " +
		   "PhysicalPersonCode NVARCHAR(2048) NULL, " +
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
		   "VisaFrom DATETIME NULL, " +
		   "VisaTo DATETIME NULL, " +

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
			"Code, PhysicalPersonCode, Name, SurName, ConstructionSiteCode, ConstructionSiteName, " +
			"DateOfBirth, Gender, CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
			"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, CityId, CityIdentifier, CityCode, CityName, Address, " +
			"PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
			"Passport, VisaFrom, VisaTo, " +
			"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
			"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, ResidenceAddress, " +
			"EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO PhysicalPersons " +
			"(Id, ServerId, Identifier, " +
			"Code, PhysicalPersonCode, Name, SurName, ConstructionSiteCode, ConstructionSiteName, " +
			"DateOfBirth, Gender, CountryId, CountryIdentifier, CountryCode, CountryName, RegionId, RegionIdentifier, RegionCode, RegionName, " +
			"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, CityId, CityIdentifier, CityCode, CityName, Address, " +
			"PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
			"Passport, VisaFrom, VisaTo, " +
			"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
			"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, ResidenceAddress, " +
			"EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, " +
			"@Code, @PhysicalPersonCode, @Name, @SurName, @ConstructionSiteCode, @ConstructionSiteName, " +
			"@DateOfBirth, @Gender, @CountryId, @CountryIdentifier, @CountryCode, @CountryName, @RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
			"@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, @CityId, @CityIdentifier, @CityCode, @CityName, @Address, " +
			"@PassportCountryId, @PassportCountryIdentifier, @PassportCountryCode, @PassportCountryName, @PassportCityId, @PassportCityIdentifier, @PassportCityCode, @PassportCityName, " +
			"@Passport, @VisaFrom, @VisaTo, " +
			"@ResidenceCountryId, @ResidenceCountryIdentifier, @ResidenceCountryCode, @ResidenceCountryName, " +
			"@ResidenceCityId, @ResidenceCityIdentifier, @ResidenceCityCode, @ResidenceCityName, @ResidenceAddress, " +
			"@EmbassyDate, @VisaDate, @VisaValidFrom, @VisaValidTo, @WorkPermitFrom, @WorkPermitTo, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";
       
        #region Helper methods

        private static PhysicalPersonViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhysicalPersonViewModel dbEntry = new PhysicalPersonViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.PhysicalPersonCode = SQLiteHelper.GetString(query, ref counter);
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
            dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.ResidenceCountry = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.ResidenceCity = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.ResidenceAddress = SQLiteHelper.GetString(query, ref counter);

            dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.VisaDate = SQLiteHelper.GetDateTimeNullable(query, ref counter);

            dbEntry.VisaValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.VisaValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhysicalPersonViewModel PhysicalPerson)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPerson.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPerson.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)PhysicalPerson.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPerson.PhysicalPersonCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)PhysicalPerson.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@SurName", ((object)PhysicalPerson.SurName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)PhysicalPerson.ConstructionSiteCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)PhysicalPerson.ConstructionSiteName) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@DateOfBirth", ((object)PhysicalPerson.DateOfBirth) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Gender", ((object)PhysicalPerson.Gender) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)PhysicalPerson.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)PhysicalPerson.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)PhysicalPerson.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)PhysicalPerson.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionId", ((object)PhysicalPerson.Region?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)PhysicalPerson.Region?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)PhysicalPerson.Region?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionName", ((object)PhysicalPerson.Region?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)PhysicalPerson.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)PhysicalPerson.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)PhysicalPerson.Municipality?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)PhysicalPerson.Municipality?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityId", ((object)PhysicalPerson.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)PhysicalPerson.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)PhysicalPerson.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)PhysicalPerson.City?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)PhysicalPerson.Address) ?? DBNull.Value);


            insertCommand.Parameters.AddWithValue("@PassportCountryId", ((object)PhysicalPerson.PassportCountry?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryIdentifier", ((object)PhysicalPerson.PassportCountry?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryCode", ((object)PhysicalPerson.PassportCountry?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCountryName", ((object)PhysicalPerson.PassportCountry?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityId", ((object)PhysicalPerson.PassportCity?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityIdentifier", ((object)PhysicalPerson.PassportCity?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityCode", ((object)PhysicalPerson.PassportCity?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PassportCityName", ((object)PhysicalPerson.PassportCity?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Passport", ((object)PhysicalPerson.Passport) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaFrom", ((object)PhysicalPerson.VisaFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaTo", ((object)PhysicalPerson.VisaTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryId", ((object)PhysicalPerson.ResidenceCountry?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryIdentifier", ((object)PhysicalPerson.ResidenceCountry?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryCode", ((object)PhysicalPerson.ResidenceCountry?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCountryName", ((object)PhysicalPerson.ResidenceCountry?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityId", ((object)PhysicalPerson.ResidenceCity?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityIdentifier", ((object)PhysicalPerson.ResidenceCity?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityCode", ((object)PhysicalPerson.ResidenceCity?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceCityName", ((object)PhysicalPerson.ResidenceCity?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ResidenceAddress", ((object)PhysicalPerson.ResidenceAddress) ?? DBNull.Value);


            insertCommand.Parameters.AddWithValue("@EmbassyDate", ((object)PhysicalPerson.EmbassyDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaDate", ((object)PhysicalPerson.VisaDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaValidFrom", ((object)PhysicalPerson.VisaValidFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VisaValidTo", ((object)PhysicalPerson.VisaValidTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@WorkPermitFrom", ((object)PhysicalPerson.WorkPermitFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@WorkPermitTo", ((object)PhysicalPerson.WorkPermitTo) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPerson.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPerson.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion
        public PhysicalPersonListResponse GetPhysicalPersonsByPage(int companyId, PhysicalPersonViewModel PhysicalPersonSearchObject, int currentPage = 1, int itemsPerPage = 50)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			List<PhysicalPersonViewModel> PhysicalPersons = new List<PhysicalPersonViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM PhysicalPersons " +
						"WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
						"AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
						"AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
						"AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR ConstructionSiteCode LIKE @ConstructionSite OR ConstructionSiteName LIKE @ConstructionSite) " +
                        "AND (@PhysicalPersonCode IS NULL OR @PhysicalPersonCode = '' OR PhysicalPersonCode LIKE @PhysicalPersonCode) " +
                        "AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					selectCommand.Parameters.AddWithValue("@Name", ((object)PhysicalPersonSearchObject.SearchBy_Name) != null ? "%" + PhysicalPersonSearchObject.SearchBy_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@SurName", ((object)PhysicalPersonSearchObject.SearchBy_SurName) != null ? "%" + PhysicalPersonSearchObject.SearchBy_SurName + "%" : "");
					selectCommand.Parameters.AddWithValue("@Passport", ((object)PhysicalPersonSearchObject.SearchBy_Passport) != null ? "%" + PhysicalPersonSearchObject.SearchBy_Passport + "%" : "");
					selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(PhysicalPersonSearchObject.Search_ConstructionSite) ? "%" + PhysicalPersonSearchObject.Search_ConstructionSite + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonSearchObject.Search_PhysicalPersonCode) != null ? "%" + PhysicalPersonSearchObject.Search_PhysicalPersonCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
                        PhysicalPersonViewModel dbEntry = Read(query);
                        PhysicalPersons.Add(dbEntry);
                    }

					response.PhysicalPersons = PhysicalPersons;

					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM PhysicalPersons " +
					   "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
						"AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
						"AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
						"AND (@ConstructionSite IS NULL OR @ConstructionSite = '' OR ConstructionSiteCode LIKE @ConstructionSite OR ConstructionSiteName LIKE @ConstructionSite) " +
                        "AND (@PhysicalPersonCode IS NULL OR @PhysicalPersonCode = '' OR PhysicalPersonCode LIKE @PhysicalPersonCode) " +
                        "AND CompanyId = @CompanyId;", db);
					selectCommand.Parameters.AddWithValue("@Name", ((object)PhysicalPersonSearchObject.SearchBy_Name) != null ? "%" + PhysicalPersonSearchObject.SearchBy_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@SurName", ((object)PhysicalPersonSearchObject.SearchBy_SurName) != null ? "%" + PhysicalPersonSearchObject.SearchBy_SurName + "%" : "");
					selectCommand.Parameters.AddWithValue("@Passport", ((object)PhysicalPersonSearchObject.SearchBy_Passport) != null ? "%" + PhysicalPersonSearchObject.SearchBy_Passport + "%" : "");
					selectCommand.Parameters.AddWithValue("@ConstructionSite", !String.IsNullOrEmpty(PhysicalPersonSearchObject.Search_ConstructionSite) ? "%" + PhysicalPersonSearchObject.Search_ConstructionSite + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonSearchObject.Search_PhysicalPersonCode) != null ? "%" + PhysicalPersonSearchObject.Search_PhysicalPersonCode + "%" : "");
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
					response.PhysicalPersons = new List<PhysicalPersonViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.PhysicalPersons = PhysicalPersons;
			return response;
		}

		public PhysicalPersonResponse GetPhysicalPerson(Guid identifier)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			PhysicalPersonViewModel PhysicalPerson = new PhysicalPersonViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM PhysicalPersons " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

					if (query.Read())
					{
                        PhysicalPersonViewModel dbEntry = Read(query);
                        PhysicalPerson = dbEntry;
                    }
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.PhysicalPerson = new PhysicalPersonViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.PhysicalPerson = PhysicalPerson;
			return response;
		}

		//public PhysicalPersonListResponse GetUnSyncedItems(int companyId)
		//{
		//	PhysicalPersonListResponse response = new PhysicalPersonListResponse();
		//	List<PhysicalPersonViewModel> viewModels = new List<PhysicalPersonViewModel>();

		//	using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
		//	{
		//		db.Open();
		//		try
		//		{
		//			SqliteCommand selectCommand = new SqliteCommand(
		//				SqlCommandSelectPart +
		//				"FROM PhysicalPersons " +
		//				"WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
		//				"ORDER BY Id DESC;", db);
		//			selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

		//			SqliteDataReader query = selectCommand.ExecuteReader();

		//			while (query.Read())
		//			{
		//				int counter = 0;
		//				PhysicalPersonViewModel dbEntry = new PhysicalPersonViewModel();
		//				dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
		//				dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
		//				dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.PhysicalPersonCode = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.ConstructionSiteCode = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.ConstructionSiteName = SQLiteHelper.GetString(query, ref counter);

		//				dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.Gender = SQLiteHelper.GetInt(query, ref counter);
		//				dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
		//				dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
		//				dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
		//				dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
		//				dbEntry.Address = SQLiteHelper.GetString(query, ref counter);

		//				dbEntry.PassportCountry = SQLiteHelper.GetCountry(query, ref counter);
		//				dbEntry.PassportCity = SQLiteHelper.GetCity(query, ref counter);
		//				dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
		//				dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.ResidenceCountry = SQLiteHelper.GetCountry(query, ref counter);
		//				dbEntry.ResidenceCity = SQLiteHelper.GetCity(query, ref counter);
		//				dbEntry.ResidenceAddress = SQLiteHelper.GetString(query, ref counter);

		//				dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.VisaDate = SQLiteHelper.GetDateTimeNullable(query, ref counter);
		//				dbEntry.VisaValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
		//				dbEntry.VisaValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
		//				dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);

		//				dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
		//				dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
		//				dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
		//				dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
		//				viewModels.Add(dbEntry);
		//			}

		//		}
		//		catch (SqliteException error)
		//		{
		//			MainWindow.ErrorMessage = error.Message;
		//			response.Success = false;
		//			response.Message = error.Message;
		//			response.PhysicalPersons = new List<PhysicalPersonViewModel>();
		//			return response;
		//		}
		//		db.Close();
		//	}
		//	response.Success = true;
		//	response.PhysicalPersons = viewModels;
		//	return response;
		//}

        public void Sync(IPhysicalPersonService physicalPersonService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonRequest request = new SyncPhysicalPersonRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonListResponse response = physicalPersonService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersons?.Count ?? 0;
                    var items = new List<PhysicalPersonViewModel>(response.PhysicalPersons);

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhysicalPersons WHERE Identifier = @Identifier";

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
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersons WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersons WHERE CompanyId = @CompanyId", db);
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

		public PhysicalPersonResponse Create(PhysicalPersonViewModel physicalPerson)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, physicalPerson);
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

		//public PhysicalPersonResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
		//{
		//	PhysicalPersonResponse response = new PhysicalPersonResponse();

		//	using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
		//	{
		//		db.Open();

		//		SqliteCommand insertCommand = new SqliteCommand();
		//		insertCommand.Connection = db;

		//		insertCommand.CommandText = "UPDATE PhysicalPersons SET " +
		//			"IsSynced = @IsSynced, " +
  //                  "Code = @Code, " +
  //                  "UpdatedAt = @UpdatedAt, " +
  //                  "ServerId = @ServerId " +
		//			"WHERE Identifier = @Identifier ";

		//		insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
		//		insertCommand.Parameters.AddWithValue("@Code", code);
		//		insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
  //              insertCommand.Parameters.AddWithValue("@ServerId", serverId);
		//		insertCommand.Parameters.AddWithValue("@Identifier", identifier);

		//		try
		//		{
		//			insertCommand.ExecuteReader();
		//		}
		//		catch (SqliteException error)
		//		{
		//			MainWindow.ErrorMessage = error.Message;
		//			response.Success = false;
		//			response.Message = error.Message;
		//			return response;
		//		}
		//		db.Close();

		//		response.Success = true;
		//		return response;
		//	}
		//}

		public PhysicalPersonResponse Delete(Guid identifier)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText =
					"DELETE FROM PhysicalPersons WHERE Identifier = @Identifier";
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

		public PhysicalPersonResponse DeleteAll()
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();

			try
			{
				using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
				{
					db.Open();
					db.EnableExtensions(true);

					SqliteCommand insertCommand = new SqliteCommand();
					insertCommand.Connection = db;

					//Use parameterized query to prevent SQL injection attacks
					insertCommand.CommandText = "DELETE FROM PhysicalPersons";
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
