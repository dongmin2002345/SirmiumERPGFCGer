using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Sectors
{
    public class AgencyView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vAgencies";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vAgencies AS " +
                "SELECT agency.Id AS AgencyId, agency.Identifier AS AgencyIdentifier, agency.Code AS AgencyCode, agency.Name AS AgencyName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "sector.Id AS SectorId, sector.Identifier AS SectorIdentifier, sector.Code AS SectorCode, sector.Name AS SectorName, " +
                "agency.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (agency.UpdatedAt), (country.UpdatedAt), (sector.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Agencies agency " +
                "LEFT JOIN Countries country ON agency.CountryId = country.Id " +
                "LEFT JOIN Sectors sector ON agency.SectorId = sector.Id " +
                "LEFT JOIN Users createdBy ON agency.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON agency.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
