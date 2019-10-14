using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.ConstructionSites
{
    public class ConstructionSiteNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vConstructionSiteNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vConstructionSiteNotes AS " +
                "SELECT constructionSiteNote.Id AS ConstructionSiteNoteId, constructionSiteNote.Identifier AS ConstructionSiteNoteIdentifier, " +
                "constructionSite.Id AS ConstructionSiteId, constructionSite.Identifier AS ConstructionSiteIdentifier, constructionSite.Code AS ConstructionSiteCode, constructionSite.Name AS ConstructionSiteName, " +
                "constructionSiteNote.Note, constructionSiteNote.NoteDate, constructionSiteNote.ItemStatus, constructionSiteNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (constructionSiteNote.UpdatedAt), (constructionSite.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM ConstructionSiteNotes constructionSiteNote " +
                "LEFT JOIN ConstructionSites constructionSite ON constructionSiteNote.ConstructionSiteId = constructionSite.Id " +
                "LEFT JOIN Users createdBy ON constructionSiteNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON constructionSiteNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
