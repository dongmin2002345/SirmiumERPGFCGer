using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneProfession_Bora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Professions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_CountryId",
                table: "Professions",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professions_Countries_CountryId",
                table: "Professions",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professions_Countries_CountryId",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Professions_CountryId",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Professions");
        }
    }
}
