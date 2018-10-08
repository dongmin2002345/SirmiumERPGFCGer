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
                }
            }

            catch (SqliteException e)
            {
                MainWindow.ErrorMessage = e.Message;
            }
        }
    }
}
