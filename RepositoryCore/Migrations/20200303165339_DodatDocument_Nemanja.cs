using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class DodatDocument_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ConstructionSiteNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "BusinessPartners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "ConstructionSiteNotes");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "BusinessPartners");
        }
    }
}
