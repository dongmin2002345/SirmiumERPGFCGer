using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IZmenaPPGer_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressGer",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CityId",
                table: "BusinessPartners",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Cities_CityId",
                table: "BusinessPartners",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Cities_CityId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CityId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "AddressGer",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "BusinessPartners");
        }
    }
}
