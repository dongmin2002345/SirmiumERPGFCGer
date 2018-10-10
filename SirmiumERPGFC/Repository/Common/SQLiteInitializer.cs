using Microsoft.Data.Sqlite;
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

                }
            }

            catch (SqliteException e)
            {
                MainWindow.ErrorMessage = e.Message;
            }
        }
    }
}
