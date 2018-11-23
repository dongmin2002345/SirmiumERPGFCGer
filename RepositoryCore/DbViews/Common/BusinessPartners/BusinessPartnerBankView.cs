using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerBankView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerBanks";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerBanks AS " +
                "SELECT businessPartnerBank.Id AS BusinessPartnerBankId, businessPartnerBank.Identifier AS BusinessPartnerBankIdentifier, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "bank.Id AS BankId, bank.Identifier AS BankIdentifier, bank.Code AS BankCode, bank.Name AS BankName,  " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName,  " +
                "businessPartnerBank.AccountNumber, businessPartnerBank.Active AS Active," +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerBank.UpdatedAt), (businessPartner.UpdatedAt), (bank.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerBanks businessPartnerBank " +
                "LEFT JOIN BusinessPartners businessPartner ON businessPartnerBank.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Banks bank ON businessPartnerBank.BankId = bank.Id " +
                "LEFT JOIN Countries country ON businessPartnerBank.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerBank.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerBank.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
