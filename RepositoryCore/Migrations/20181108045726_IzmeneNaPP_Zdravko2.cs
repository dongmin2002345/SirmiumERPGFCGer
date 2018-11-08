using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaPP_Zdravko2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CountryId",
                table: "BusinessPartners",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Countries_CountryId",
                table: "BusinessPartners",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Countries_CountryId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CountryId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "BusinessPartners");
        }
    }
}
