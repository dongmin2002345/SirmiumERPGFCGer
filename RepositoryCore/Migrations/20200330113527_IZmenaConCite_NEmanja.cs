using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IZmenaConCite_NEmanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressPartner",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityPartnerId",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NamePartner",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSites_CityPartnerId",
                table: "ConstructionSites",
                column: "CityPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSites_Cities_CityPartnerId",
                table: "ConstructionSites",
                column: "CityPartnerId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSites_Cities_CityPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSites_CityPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "AddressPartner",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "CityPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "NamePartner",
                table: "ConstructionSites");
        }
    }
}
