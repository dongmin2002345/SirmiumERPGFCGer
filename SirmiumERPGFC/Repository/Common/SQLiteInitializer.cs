﻿using Microsoft.Data.Sqlite;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.Locations;
using SirmiumERPGFC.Repository.Companies;
using SirmiumERPGFC.Repository.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SirmiumERPGFC.Repository.Sectors;
using SirmiumERPGFC.Repository.Professions;
using SirmiumERPGFC.Repository.Banks;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Repository.ToDos;
using SirmiumERPGFC.Repository.OutputInvoices;
using SirmiumERPGFC.Repository.InputInvoices;
using SirmiumERPGFC.Repository.TaxAdministrations;
using SirmiumERPGFC.Repository.Limitations;

namespace SirmiumERPGFC.Repository.Common
{
    public static class SQLiteInitializer
    {
        public static void Initalize(bool withTableDrop)
        {
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    #region Companies

                    #region Company
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Companies", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    SqliteCommand createTable = new SqliteCommand(CompanySQLiteRepository.CompanyTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #endregion

                    #region ToDos
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE ToDos", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    SqliteCommand createTableToDo = new SqliteCommand(ToDoSQLiteRepository.ToDoTableCreatePart, db);
                    createTableToDo.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("ToDos", "ToDoDate", "DATETIME NULL");
                    #endregion

                    #region Limitations
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Limitations", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    SqliteCommand createTableLimitation = new SqliteCommand(LimitationSQLiteRepository.LimitationTableCreatePart, db);
                    createTableLimitation.ExecuteReader();

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE LimitationEmails", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    SqliteCommand createTableLimitationEmail = new SqliteCommand(LimitationEmailSQLiteRepository.LimitationEmailTableCreatePart, db);
                    createTableLimitationEmail.ExecuteReader();
                    #endregion

                    #region Users

                    #region Users
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Users", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(UserSQLiteRepository.UserTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region CompanyUsers
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE CompanyUsers", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(CompanyUserSQLiteRepository.CompanyUserTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #endregion

                    #region Countries
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Countries", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(CountrySQLiteRepository.CountryTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region Cities
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Cities", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(CitySQLiteRepository.CityTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region Regions
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Regions", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(RegionSQLiteRepository.RegionTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region Municipalities
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Municipalities", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(MunicipalitySQLiteRepository.MunicipalityTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region BusinessPartners
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartners", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerSQLiteRepository.BusinessPartnerTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "InternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "CountryId", "INTEGER NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "CountryIdentifier", "GUID NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "CountryCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "CountryName", "NVARCHAR(2048) NULL");

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "IsInPDVGer", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "TaxAdministrationId", "INTEGER NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "TaxAdministrationIdentifier", "GUID NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "TaxAdministrationCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "TaxAdministrationName", "NVARCHAR(2048) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "IBAN", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "BetriebsNumber", "NVARCHAR(48) NULL");

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "VatDeductionFrom", "DATETIME NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartners", "VatDeductionTo", "DATETIME NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerDocuments", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerDocumentSQLiteRepository.BusinessPartnerDocumentTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerDocuments", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerDocuments", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerNotes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerNoteSQLiteRepository.BusinessPartnerNoteTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerNotes", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerNotes", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    #endregion

                    #region BusinessPartnerLocation
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerLocations", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerLocationSQLiteRepository.BusinessPartnerLocationTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerLocations", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerLocations", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #region BusinessPartnerOrganizationUnit
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerOrganizationUnits", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerOrganizationUnitSQLiteRepository.BusinessPartnerOrganizationUnitTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerOrganizationUnits", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerOrganizationUnits", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #region BusinessPartnerPhone
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerPhones", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerPhoneSQLiteRepository.BusinessPartnerPhoneTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerPhones", "ContactPersonFirstName", "NVARCHAR(2048) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerPhones", "ContactPersonLastName", "NVARCHAR(2048) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerPhones", "Birthday", "DATETIME NULL");

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerPhones", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerPhones", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #region BusinessPartnerInstitution
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerInstitutions", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerInstitutionSQLiteRepository.BusinessPartnerInstitutionTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerInstitutions", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerInstitutions", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    #endregion

                    #region BusinessPartnerBank
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerBanks", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerBankSQLiteRepository.BusinessPartnerBankTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerBanks", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerBanks", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #region BusinessPartnerType
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerTypes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerTypeSQLiteRepository.BusinessPartnerTypeTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region BusinessPartnerBusinessPartnerType
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerBusinessPartnerTypes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerBusinessPartnerTypeSQLiteRepository.BusinessPartnerBusinessPartnerTypeTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region Sectors
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Sectors", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(SectorSQLiteRepository.SectorTableCreatePart, db);
                    createTable.ExecuteReader();

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Agencies", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(AgencySQLiteRepository.AgencyTableCreatePart, db);
                    createTable.ExecuteReader();

                    #endregion

                    #region Profession
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Professions", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(ProfessionSQLiteRepository.ProfessionTableCreatePart, db);
                    createTable.ExecuteReader();
					#endregion

					#region Bank
					if (withTableDrop)
					{
						try
						{
							SqliteCommand dropTable = new SqliteCommand("DROP TABLE Banks", db);
							dropTable.ExecuteNonQuery();
						}
						catch (Exception ex) { }
					}
					createTable = new SqliteCommand(BankSQLiteRepository.BankTableCreatePart, db);
					createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("Banks", "Swift", "NVARCHAR(2048) NULL");
                    #endregion

                    #region LicenceType
                    if (withTableDrop)
					{
						try
						{
							SqliteCommand dropTable = new SqliteCommand("DROP TABLE LicenceTypes", db);
							dropTable.ExecuteNonQuery();
						}
						catch (Exception ex) { }
					}
					createTable = new SqliteCommand(LicenceTypeSQLiteRepository.LicenceTypeTableCreatePart, db);
					createTable.ExecuteReader();
					#endregion
			
                    #region ConstructionSites
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE ConstructionSites", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(ConstructionSiteSQLiteRepository.ConstructionSiteTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "InternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "ProContractDate", "DATETIME NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "ContractStart", "DATETIME NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "CountryId", "INTEGER NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "CountryIdentifier", "GUID NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "CountryCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("ConstructionSites", "CountryName", "NVARCHAR(2048) NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE ConstructionSiteDocuments", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(ConstructionSiteDocumentSQLiteRepository.ConstructionSiteDocumentTableCreatePart, db);
                    createTable.ExecuteReader();

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE ConstructionSiteNotes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(ConstructionSiteNoteSQLiteRepository.ConstructionSiteNoteTableCreatePart, db);
                    createTable.ExecuteReader();

                    #endregion

                    #region ConstructionSiteCalculations
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE ConstructionSiteCalculations", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(ConstructionSiteCalculationSQLiteRepository.ConstructionSiteCalculationTableCreatePart, db);
                    createTable.ExecuteReader();
					#endregion

					#region PhysicalPerson
					if (withTableDrop)
					{
						try
						{
							SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersons", db);
							dropTable.ExecuteNonQuery();
						}
						catch (Exception ex) { }
					}
					createTable = new SqliteCommand(PhysicalPersonSQLiteRepository.PhysicalPersonTableCreatePart, db);
					createTable.ExecuteReader();



                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonItems", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonItemSQLiteRepository.PhysicalPersonItemTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("PhysicalPersonItems", "EmbassyDate", "DATETIME NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonNotes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonNoteSQLiteRepository.PhysicalPersonNoteTableCreatePart, db);
                    createTable.ExecuteReader();


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonDocuments", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonDocumentSQLiteRepository.PhysicalPersonDocumentTableCreatePart, db);
                    createTable.ExecuteReader();


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonCards", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonCardSQLiteRepository.PhysicalPersonCardTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("PhysicalPersonCards", "PlusMinus", "NVARCHAR(48) NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonProfessions", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonProfessionSQLiteRepository.PhysicalPersonItemTableCreatePart, db);

                    SQLiteHelper.AddColumnIfNotExists("PhysicalPersonProfessions", "ProfessionSecondCode", "NVARCHAR(48) NULL");

                    createTable.ExecuteReader();


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE PhysicalPersonLicenceItems", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(PhysicalPersonLicenceSQLiteRepository.PhysicalPersonItemTableCreatePart, db);
                    createTable.ExecuteReader();





                    #endregion

                    #region Employees
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE Employees", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeSQLiteRepository.EmployeeTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("Employees", "ConstructionSiteCode", "NVARCHAR(2048) NULL");
                    SQLiteHelper.AddColumnIfNotExists("Employees", "ConstructionSiteName", "NVARCHAR(2048) NULL");
                    SQLiteHelper.AddColumnIfNotExists("Employees", "ResidenceCountryId", "INTEGER NULL");
                    SQLiteHelper.AddColumnIfNotExists("Employees", "ResidenceCountryIdentifier", "GUID NULL");
                    SQLiteHelper.AddColumnIfNotExists("Employees", "ResidenceCountryCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("Employees", "ResidenceCountryName", "NVARCHAR(2048) NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeItems", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeItemSQLiteRepository.EmployeeItemTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeItems", "EmbassyDate", "DATETIME NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeItems", "EmployeeInternalCode", "NVARCHAR(48) NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeNotes", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeNoteSQLiteRepository.EmployeeNoteTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeNotes", "EmployeeInternalCode", "NVARCHAR(48) NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeDocuments", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeDocumentSQLiteRepository.EmployeeDocumentTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeDocuments", "EmployeeInternalCode", "NVARCHAR(48) NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeCards", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeCardSQLiteRepository.EmployeeCardTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeCards", "PlusMinus", "NVARCHAR(48) NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeCards", "EmployeeInternalCode", "NVARCHAR(48) NULL");


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeProfessionItems", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeProfessionItemSQLiteRepository.EmployeeItemTableCreatePart, db);

                    SQLiteHelper.AddColumnIfNotExists("EmployeeProfessionItems", "EmployeeInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("EmployeeProfessionItems", "ProfessionSecondCode", "NVARCHAR(48) NULL");
                    createTable.ExecuteReader();


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeLicenceItems", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeLicenceItemSQLiteRepository.EmployeeItemTableCreatePart, db);

                    SQLiteHelper.AddColumnIfNotExists("EmployeeLicenceItems", "EmployeeInternalCode", "NVARCHAR(48) NULL");

                    createTable.ExecuteReader();


                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeByConstructionSites", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeByConstructionSiteSQLiteRepository.EmployeeByConstructionSiteTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByConstructionSites", "RealEndDate", "DATETIME NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByConstructionSites", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("EmployeeByConstructionSites", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByConstructionSites", "EmployeeInternalCode", "NVARCHAR(48) NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE EmployeeByBusinessPartners", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(EmployeeByBusinessPartnerSQLiteRepository.EmployeeByBusinessPartnerTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByBusinessPartners", "RealEndDate", "DATETIME NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByBusinessPartners", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("EmployeeByBusinessPartners", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    SQLiteHelper.AddColumnIfNotExists("EmployeeByBusinessPartners", "EmployeeInternalCode", "NVARCHAR(48) NULL");

                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE BusinessPartnerByConstructionSites", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(BusinessPartnerByConstructionSiteSQLiteRepository.BusinessPartnerByConstructionSiteTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerByConstructionSites", "RealEndDate", "DATETIME NULL");

                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerByConstructionSites", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("BusinessPartnerByConstructionSites", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");

                    #endregion

                    #region FamilyMembers
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE FamilyMembers", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(FamilyMemberSQLiteRepository.FamilyMemberTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region TaxAdministrations
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE TaxAdministrations", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(TaxAdministrationSQLiteRepository.TaxAdministrationTableCreatePart, db);
                    createTable.ExecuteReader();
                    #endregion

                    #region Invoices

                    #region OutputInvoices
                    if (withTableDrop)
                    {
                        try
                        {
                            SqliteCommand dropTable = new SqliteCommand("DROP TABLE OutputInvoices", db);
                            dropTable.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                    }
                    createTable = new SqliteCommand(OutputInvoiceSQLiteRepository.OutputInvoiceTableCreatePart, db);
                    createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("OutputInvoices", "Path", "NVARCHAR(2018) NULL");

                    SQLiteHelper.AddColumnIfNotExists("OutputInvoices", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("OutputInvoices", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #region InputInvoices
                    if (withTableDrop)
					{
						try
						{
							SqliteCommand dropTable = new SqliteCommand("DROP TABLE InputInvoices", db);
							dropTable.ExecuteNonQuery();
						}
						catch (Exception ex) { }
					}
					createTable = new SqliteCommand(InputInvoiceSQLiteRepository.InputInvoiceTableCreatePart, db);
					createTable.ExecuteReader();

                    SQLiteHelper.AddColumnIfNotExists("InputInvoices", "Path", "NVARCHAR(2018) NULL");

                    SQLiteHelper.AddColumnIfNotExists("InputInvoices", "BusinessPartnerInternalCode", "NVARCHAR(48) NULL");
                    SQLiteHelper.AddColumnIfNotExists("InputInvoices", "BusinessPartnerNameGer", "NVARCHAR(2048) NULL");
                    #endregion

                    #endregion
                }
			}

            catch (SqliteException e)
            {
                MainWindow.ErrorMessage = e.Message;
            }
        }
    }
}
