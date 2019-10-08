using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Base;
using System;


namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerBusinessPartnerTypeSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerBusinessPartnerTypeTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerBusinessPartnerTypes " +
            "(BusinessPartnerIdentifier GUID, " +
            "BusinessPartnerTypeIdentifier GUID)";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerBusinessPartnerTypes " +
            "(BusinessPartnerIdentifier, BusinessPartnerTypeIdentifier) " +
            "VALUES (@BusinessPartnerIdentifier, @BusinessPartnerTypeIdentifier)";

        #endregion

        #region Create

        public BaseResponse Create(Guid businessPartnerIdentifier, Guid businessPartnerTypeIdentifier)
        {
            BaseResponse response = new BaseResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerTypeIdentifier", businessPartnerTypeIdentifier);

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

        #endregion

        #region Delete

        public BaseResponse Delete(Guid businessPartnerIdentifier)
        {
            BaseResponse response = new BaseResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerBusinessPartnerTypes WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier";
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                
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

        #endregion
    }
}
