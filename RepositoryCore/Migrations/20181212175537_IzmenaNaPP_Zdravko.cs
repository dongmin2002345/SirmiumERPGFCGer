using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaNaPP_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VatDeductionFrom",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VatDeductionTo",
                table: "BusinessPartners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatDeductionFrom",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "VatDeductionTo",
                table: "BusinessPartners");
        }
    }
}
