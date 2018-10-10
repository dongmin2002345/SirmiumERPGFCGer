using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerBusinessPartnerTypeSQLiteRepository
    {
        public static string BusinessPartnerBusinessPartnerTypeTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerBusinessPartnerTypes " +
            "(BusinessPartnerIdentifier GUID, " +
            "BusinessPartnerTypeIdentifier GUID)";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerBusinessPartnerTypes " +
            "(BusinessPartnerIdentifier, BusinessPartnerTypeIdentifier) " +
            "VALUES (@BusinessPartnerIdentifier, @BusinessPartnerTypeIdentifier)";

        public BaseResponse Create(Guid businessPartnerIdentifier, Guid businessPartnerTypeIdentifier)
        {
            BaseResponse response = new BaseResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerTypeIdentifier", businessPartnerTypeIdentifier);

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

        public BaseResponse Delete(Guid businessPartnerIdentifier)
        {
            BaseResponse response = new BaseResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerBusinessPartnerTypes WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier";
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
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

    }
}
