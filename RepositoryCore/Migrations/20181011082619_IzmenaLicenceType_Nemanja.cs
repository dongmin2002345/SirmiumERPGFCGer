using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaLicenceType_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LicenceTypes",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "LicenceTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "LicenceTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenceTypes_CountryId",
                table: "LicenceTypes",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenceTypes_Countries_CountryId",
                table: "LicenceTypes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenceTypes_Countries_CountryId",
                table: "LicenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_LicenceTypes_CountryId",
                table: "LicenceTypes");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "LicenceTypes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "LicenceTypes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "LicenceTypes",
                newName: "Name");
        }
    }
}
