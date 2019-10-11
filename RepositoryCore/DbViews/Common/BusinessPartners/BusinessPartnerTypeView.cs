using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerTypeView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerTypes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerTypes AS " +
                "SELECT businessPartnerType.Id AS BusinessPartnerTypeId, businessPartnerType.Identifier AS BusinessPartnerTypeIdentifier, businessPartnerType.Code AS BusinessPartnerTypeCode, businessPartnerType.Name AS BusinessPartnerTypeName, " +
                "businessPartnerType.IsBuyer, businessPartnerType.IsSupplier, businessPartnerType.ItemStatus, businessPartnerType.Active AS Active, businessPartnerType.UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerTypes businessPartnerType " +
                "LEFT JOIN Users createdBy ON businessPartnerType.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerType.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
