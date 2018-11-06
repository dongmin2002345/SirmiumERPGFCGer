using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaRadnicimaPoFirmama_Zdravko2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "EmployeeByConstructionSites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByConstructionSites_BusinessPartnerId",
                table: "EmployeeByConstructionSites",
                column: "BusinessPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeByConstructionSites_BusinessPartners_BusinessPartnerId",
                table: "EmployeeByConstructionSites",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeByConstructionSites_BusinessPartners_BusinessPartnerId",
                table: "EmployeeByConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeByConstructionSites_BusinessPartnerId",
                table: "EmployeeByConstructionSites");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "EmployeeByConstructionSites");
        }
    }
}
