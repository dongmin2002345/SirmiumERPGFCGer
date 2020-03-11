using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IZmenaConsSite_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostingDate",
                table: "ConstructionSiteCalculations",
                newName: "DateCondition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCondition",
                table: "ConstructionSiteCalculations",
                newName: "PostingDate");
        }
    }
}
