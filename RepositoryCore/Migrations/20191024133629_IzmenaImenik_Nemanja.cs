using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaImenik_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Phonebooks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Phonebooks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phonebooks_CountryId",
                table: "Phonebooks",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Phonebooks_RegionId",
                table: "Phonebooks",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Phonebooks_Countries_CountryId",
                table: "Phonebooks",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phonebooks_Regions_RegionId",
                table: "Phonebooks",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phonebooks_Countries_CountryId",
                table: "Phonebooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Phonebooks_Regions_RegionId",
                table: "Phonebooks");

            migrationBuilder.DropIndex(
                name: "IX_Phonebooks_CountryId",
                table: "Phonebooks");

            migrationBuilder.DropIndex(
                name: "IX_Phonebooks_RegionId",
                table: "Phonebooks");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Phonebooks");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Phonebooks");
        }
    }
}
