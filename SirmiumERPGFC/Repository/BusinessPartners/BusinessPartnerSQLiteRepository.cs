using Microsoft.Data.Sqlite;
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
            "InternalCode NVARCHAR(48) NULL, " +
            "Name NVARCHAR(48) NULL, " +
            "PIB NVARCHAR(48) NULL, " +
            "PIO NVARCHAR(48) NULL, " +
            "PDV NVARCHAR(48) NULL, " +
            "IdentificationNumber NVARCHAR(48) NULL, " +
            "Rebate DOUBLE NULL, " +
            "DueDate INTEGER NULL, " +
            "WebSite NVARCHAR(2048) NULL, " +
            "ContactPerson NVARCHAR(2048) NULL, " +
            "IsInPdv BOOL NULL, " +
            "JBKJS NVARCHAR(48) NULL, " +
            "NameGer NVARCHAR(2048) NULL, " +
            "IsInPDVGer BOOL NULL, " +
            "TaxAdministrationId INTEGER NULL, " +
            "TaxAdministrationIdentifier GUID NULL, " +
            "TaxAdministrationCode NVARCHAR(48) NULL, " +
            "TaxAdministrationName NVARCHAR(2048) NULL, " +
            "IBAN NVARCHAR(48) NULL, " +
            "BetriebsNumber NVARCHAR(48) NULL, " +
            "TaxNr NVARCHAR(2048) NULL, " +
            "CommercialNr NVARCHAR(2048) NULL, " +
            "ContactPersonGer NVARCHAR(2048) NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID NULL, " +
            "CountryCode NVARCHAR(48) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "SectorId INTEGER NULL, " +
            "SectorIdentifier GUID NULL, " +
            "SectorCode NVARCHAR(48) NULL, " +
            "SectorName NVARCHAR(2048) NULL, " +
            "AgencyId INTEGER NULL, " +
            "AgencyIdentifier GUID NULL, " +
            "AgencyCode NVARCHAR(48) NULL, " +
            "AgencyName NVARCHAR(2048) NULL, " +
            "VatDeductionFrom DATETIME NULL, " +
            "VatDeductionTo DATETIME NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, InternalCode, Name, PIB, PIO, PDV, IdentificationNumber, " +
            "Rebate, DueDate, WebSite, ContactPerson, IsInPdv, JBKJS, " +
            "NameGer, IsInPDVGer, TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
            "IBAN, BetriebsNumber, TaxNr, CommercialNr, ContactPersonGer, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "SectorId, SectorIdentifier, SectorCode, SectorName, " +
            "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, VatDeductionFrom, VatDeductionTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartners " +
            "(Id, ServerId, Identifier, Code, InternalCode, Name, PIB, PIO, PDV, IdentificationNumber, " +
            "Rebate, DueDate, WebSite, ContactPerson, IsInPdv, JBKJS, " +
            "NameGer, IsInPDVGer, TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
            "IBAN, BetriebsNumber, TaxNr, CommercialNr, ContactPersonGer, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "SectorId, SectorIdentifier, SectorCode, SectorName, " +
            "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, VatDeductionFrom, VatDeductionTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @InternalCode, @Name, @PIB, @PIO, @PDV, @IdentificationNumber, " +
            "@Rebate, @DueDate, @WebSite, @ContactPerson, @IsInPdv, @JBKJS, " +
            "@NameGer, @IsInPDVGer, @TaxAdministrationId, @TaxAdministrationIdentifier, @TaxAdministrationCode, @TaxAdministrationName, " +
            "@IBAN, @BetriebsNumber, @TaxNr, @CommercialNr, @ContactPersonGer, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@SectorId, @SectorIdentifier, @SectorCode, @SectorName, " +
            "@AgencyId, @AgencyIdentifier, @AgencyCode, @AgencyName, @VatDeductionFrom, @VatDeductionTo, " +
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
                        "SELECT bp.ServerId, bp.Identifier, bp.Code, bp.InternalCode, bp.Name, bp.PIB, bp.PIO, bp.PDV, bp.IdentificationNumber, " +
                        "bp.Rebate, bp.DueDate, bp.WebSite, bp.ContactPerson, bp.IsInPdv, bp.JBKJS, " +
                        "bp.NameGer, bp.IsInPDVGer, bp.TaxAdministrationId, bp.TaxAdministrationIdentifier, bp.TaxAdministrationCode, bp.TaxAdministrationName, " +
                        "bp.IBAN, bp.BetriebsNumber, bp.TaxNr, bp.CommercialNr, bp.ContactPersonGer, " +
                        "bp.CountryId, bp.CountryIdentifier, bp.CountryCode, bp.CountryName, " +
                        "bp.SectorId, bp.SectorIdentifier, bp.SectorCode, bp.SectorName, " +
                        "bp.AgencyId, bp.AgencyIdentifier, bp.AgencyCode, bp.AgencyName, bp.VatDeductionFrom, bp.VatDeductionTo, " +
                        "bp.IsSynced, bp.UpdatedAt, bp.CreatedById, bp.CreatedByName, bp.CompanyId, bp.CompanyName " +
                        "FROM BusinessPartners bp " +
                        "WHERE (@Name IS NULL OR @Name = '' OR bp.Name LIKE @Name OR bp.NameGer LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR ((SELECT GROUP_CONCAT(cities.CityName) FROM BusinessPartnerLocations cities Where bp.Identifier = cities.BusinessPartnerIdentifier) LIKE @PIB)) " +
                        "AND (@InternalCode IS NULL OR @InternalCode = '' OR bp.InternalCode LIKE @InternalCode) " +
                        "AND (@AgencyName IS NULL OR @AgencyName = '' OR bp.AgencyName LIKE @AgencyName) " +
                        "AND bp.CompanyId = @CompanyId " +
                        "ORDER BY bp.IsSynced, bp.Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", (((object)businessPartnerSearchObject?.Search_Name) != null && businessPartnerSearchObject?.Search_Name != "") ? "%" + businessPartnerSearchObject?.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", (((object)businessPartnerSearchObject?.Search_PIB) != null && businessPartnerSearchObject?.Search_PIB != "") ? "%" + businessPartnerSearchObject?.Search_PIB + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InternalCode", ((object)businessPartnerSearchObject?.Search_Code) != null ? "%" + businessPartnerSearchObject?.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@AgencyName", ((object)businessPartnerSearchObject?.Search_Agency) != null ? "%" + businessPartnerSearchObject?.Search_Agency + "%" : "");
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
                        dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NameGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDVGer = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.TaxAdministration = SQLiteHelper.GetTaxAdministration(query, ref counter);
                        dbEntry.IBAN = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BetriebsNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.TaxNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CommercialNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPersonGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.Agency = SQLiteHelper.GetAgency(query, ref counter);
                        dbEntry.VatDeductionFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VatDeductionTo = SQLiteHelper.GetDateTime(query, ref counter);
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
                        "AND (@InternalCode IS NULL OR @InternalCode = '' OR InternalCode LIKE @InternalCode) " +
                        "AND (@AgencyName IS NULL OR @AgencyName = '' OR AgencyName LIKE @AgencyName) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerSearchObject?.Search_Name) != null ? "%" + businessPartnerSearchObject?.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)businessPartnerSearchObject?.Search_PIB) != null ? "%" + businessPartnerSearchObject?.Search_PIB + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InternalCode", ((object)businessPartnerSearchObject?.Search_Code) != null ? "%" + businessPartnerSearchObject?.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@AgencyName", ((object)businessPartnerSearchObject?.Search_Agency) != null ? "%" + businessPartnerSearchObject?.Search_Agency + "%" : "");
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
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR NameGer LIKE @Name) " +
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
                        dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NameGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDVGer = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.TaxAdministration = SQLiteHelper.GetTaxAdministration(query, ref counter);
                        dbEntry.IBAN = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BetriebsNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.TaxNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CommercialNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPersonGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.Agency = SQLiteHelper.GetAgency(query, ref counter);
                        dbEntry.VatDeductionFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VatDeductionTo = SQLiteHelper.GetDateTime(query, ref counter);
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

        public BusinessPartnerListResponse GetBusinessPartnersOnConstructionSiteByPage(int companyId, Guid constructionSiteIdentifier, BusinessPartnerViewModel BusinessPartnerSearchObject, int currentPage = 1, int itemsPerPage = 50)
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
                        "WHERE Identifier IN (SELECT BusinessPartnerIdentifier FROM BusinessPartnerByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR PIB LIKE @PIB) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)BusinessPartnerSearchObject.Search_Name) != null ? "%" + BusinessPartnerSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)BusinessPartnerSearchObject.Search_PIB) != null ? "%" + BusinessPartnerSearchObject.Search_PIB + "%" : "");
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
                        dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NameGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDVGer = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.TaxAdministration = SQLiteHelper.GetTaxAdministration(query, ref counter);
                        dbEntry.IBAN = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BetriebsNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.TaxNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CommercialNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPersonGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.Agency = SQLiteHelper.GetAgency(query, ref counter);
                        dbEntry.VatDeductionFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VatDeductionTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        BusinessPartners.Add(dbEntry);
                    }

                    response.BusinessPartners = BusinessPartners;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM BusinessPartners " +
                        "WHERE Identifier IN (SELECT BusinessPartnerIdentifier FROM BusinessPartnerByConstructionSites WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@PIB IS NULL OR @PIB = '' OR PIB LIKE @PIB) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)BusinessPartnerSearchObject.Search_Name) != null ? "%" + BusinessPartnerSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@PIB", ((object)BusinessPartnerSearchObject.Search_PIB) != null ? "%" + BusinessPartnerSearchObject.Search_PIB + "%" : "");
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
            response.BusinessPartners = BusinessPartners;
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
                        dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIB = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PIO = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PDV = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IdentificationNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Rebate = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.DueDate = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.WebSite = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDV = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.JBKJS = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NameGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsInPDVGer = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.TaxAdministration = SQLiteHelper.GetTaxAdministration(query, ref counter);
                        dbEntry.IBAN = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BetriebsNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.TaxNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CommercialNr = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPersonGer = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.Agency = SQLiteHelper.GetAgency(query, ref counter);
                        dbEntry.VatDeductionFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VatDeductionTo = SQLiteHelper.GetDateTime(query, ref counter);
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
                    if (bp.IsActive)
                    {
                        bp.IsSynced = true;
                        Create(bp);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartners WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartners WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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
                insertCommand.Parameters.AddWithValue("@Code", ((object)businessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@InternalCode", ((object)businessPartner.InternalCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)businessPartner.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PIB", ((object)businessPartner.PIB) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PIO", ((object)businessPartner.PIO) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PDV", ((object)businessPartner.PDV) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IdentificationNumber", ((object)businessPartner.IdentificationNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Rebate", ((object)businessPartner.Rebate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DueDate", ((object)businessPartner.DueDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WebSite", ((object)businessPartner.WebSite) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPerson", ((object)businessPartner.ContactPerson) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsInPdv", ((object)businessPartner.IsInPDV) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@JBKJS", ((object)businessPartner.JBKJS) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NameGer", ((object)businessPartner.NameGer) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsInPDVGer", ((object)businessPartner.IsInPDVGer) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TaxAdministrationId", ((object)businessPartner.TaxAdministration?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TaxAdministrationIdentifier", ((object)businessPartner.TaxAdministration?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TaxAdministrationCode", ((object)businessPartner.TaxAdministration?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TaxAdministrationName", ((object)businessPartner.TaxAdministration?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IBAN", ((object)businessPartner.IBAN) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BetriebsNumber", ((object)businessPartner.BetriebsNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TaxNr", ((object)businessPartner.TaxNr) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CommercialNr", ((object)businessPartner.CommercialNr) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPersonGer", ((object)businessPartner.ContactPersonGer) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorId", ((object)businessPartner.Sector?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorIdentifier", ((object)businessPartner.Sector?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorCode", ((object)businessPartner.Sector?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorName", ((object)businessPartner.Sector?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)businessPartner.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)businessPartner.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)businessPartner.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)businessPartner.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AgencyId", ((object)businessPartner.Agency?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AgencyIdentifier", ((object)businessPartner.Agency?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AgencyCode", ((object)businessPartner.Agency?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AgencyName", ((object)businessPartner.Agency?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@VatDeductionFrom", ((object)businessPartner.VatDeductionFrom) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@VatDeductionTo", ((object)businessPartner.VatDeductionTo) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartner.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartner.UpdatedAt) ?? DBNull.Value);
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

        public BusinessPartnerResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartners SET " +
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
