using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerPhoneView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerPhones";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerPhones AS " +
                "SELECT businessPartnerPhone.Id AS BusinessPartnerPhoneId, businessPartnerPhone.Identifier AS BusinessPartnerPhoneIdentifier, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "businessPartnerPhone.Phone, businessPartnerPhone.Mobile, businessPartnerPhone.Fax, businessPartnerPhone.Email, businessPartnerPhone.ContactPersonFirstName, businessPartnerPhone.ContactPersonLastName, businessPartnerPhone.Birthday, businessPartnerPhone.Description, businessPartnerPhone.ItemStatus, businessPartnerPhone.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerPhone.UpdatedAt), (businessPartner.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerPhones businessPartnerPhone " +
                "LEFT JOIN BusinessPartners businessPartner ON businessPartnerPhone.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerPhone.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerPhone.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
