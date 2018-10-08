using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneSector_Bora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Sectors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_CountryId",
                table: "Sectors",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Countries_CountryId",
                table: "Sectors",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Countries_CountryId",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_CountryId",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Sectors");
        }
    }
}
