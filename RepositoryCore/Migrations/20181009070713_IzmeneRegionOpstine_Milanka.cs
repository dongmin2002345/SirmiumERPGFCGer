using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneRegionOpstine_Milanka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Municipalities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryId",
                table: "Regions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_CountryId",
                table: "Municipalities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_Countries_CountryId",
                table: "Municipalities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Countries_CountryId",
                table: "Regions",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_Countries_CountryId",
                table: "Municipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Countries_CountryId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_CountryId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_CountryId",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Municipalities");
        }
    }
}
