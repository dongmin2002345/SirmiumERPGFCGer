using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaRadnicimaPoGradilistima_Zdravko2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RealEndDate",
                table: "EmployeeByConstructionSites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealEndDate",
                table: "BusinessPartnerByConstructionSites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealEndDate",
                table: "EmployeeByConstructionSites");

            migrationBuilder.DropColumn(
                name: "RealEndDate",
                table: "BusinessPartnerByConstructionSites");
        }
    }
}
