using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerInstitutionView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerInstitutions";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerInstitutions AS " +
                "SELECT businessPartnerInstitution.Id AS BusinessPartnerInstitutionId, businessPartnerInstitution.Identifier AS BusinessPartnerInstitutionIdentifier, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "businessPartnerInstitution.Institution, businessPartnerInstitution.Username, businessPartnerInstitution.Password, businessPartnerInstitution.ContactPerson, businessPartnerInstitution.Phone, businessPartnerInstitution.Fax, businessPartnerInstitution.Email, businessPartnerInstitution.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerInstitution.UpdatedAt), (businessPartner.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerInstitutions businessPartnerInstitution " +
                "LEFT JOIN BusinessPartners businessPartner ON businessPartnerInstitution.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerInstitution.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerInstitution.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
