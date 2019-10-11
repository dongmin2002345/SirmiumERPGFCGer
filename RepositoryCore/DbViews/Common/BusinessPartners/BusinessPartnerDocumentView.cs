using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerDocuments AS " +
                "SELECT employeeDocument.Id AS BusinessPartnerDocumentId, employeeDocument.Identifier AS BusinessPartnerDocumentIdentifier, " +
                "employee.Id AS BusinessPartnerId, employee.Identifier AS BusinessPartnerIdentifier, employee.Code AS BusinessPartnerCode, employee.Name AS BusinessPartnerName, " +
                "employeeDocument.Name, employeeDocument.CreateDate, employeeDocument.Path, employeeDocument.ItemStatus, " +
                "employeeDocument.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeDocument.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerDocuments employeeDocument " +
                "LEFT JOIN BusinessPartners employee ON employeeDocument.BusinessPartnerId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
