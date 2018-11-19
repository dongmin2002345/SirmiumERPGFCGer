using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaBrojuRadnikaPoGradilistu_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerCount",
                table: "EmployeeByConstructionSites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCount",
                table: "EmployeeByConstructionSites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCount",
                table: "EmployeeByBusinessPartners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerCount",
                table: "BusinessPartnerByConstructionSites",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessPartnerCount",
                table: "EmployeeByConstructionSites");

            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "EmployeeByConstructionSites");

            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "EmployeeByBusinessPartners");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerCount",
                table: "BusinessPartnerByConstructionSites");
        }
    }
}
