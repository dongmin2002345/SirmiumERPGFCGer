using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaPP_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CitySrbId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountrySrbId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CitySrbId",
                table: "BusinessPartners",
                column: "CitySrbId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CountrySrbId",
                table: "BusinessPartners",
                column: "CountrySrbId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Cities_CitySrbId",
                table: "BusinessPartners",
                column: "CitySrbId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Countries_CountrySrbId",
                table: "BusinessPartners",
                column: "CountrySrbId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Cities_CitySrbId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Countries_CountrySrbId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CitySrbId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CountrySrbId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CitySrbId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CountrySrbId",
                table: "BusinessPartners");
        }
    }
}
