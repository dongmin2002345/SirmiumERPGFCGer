using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaRadnicimaResidenceCountry_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResidenceCountryId",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ResidenceCountryId",
                table: "Employees",
                column: "ResidenceCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Countries_ResidenceCountryId",
                table: "Employees",
                column: "ResidenceCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_ResidenceCountryId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ResidenceCountryId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ResidenceCountryId",
                table: "Employees");
        }
    }
}
