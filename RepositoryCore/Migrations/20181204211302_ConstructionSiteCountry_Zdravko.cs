using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class ConstructionSiteCountry_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSites_CountryId",
                table: "ConstructionSites",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSites_Countries_CountryId",
                table: "ConstructionSites",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSites_Countries_CountryId",
                table: "ConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSites_CountryId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ConstructionSites");
        }
    }
}
