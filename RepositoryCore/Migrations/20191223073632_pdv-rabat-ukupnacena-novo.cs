using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class pdvrabatukupnacenanovo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfPayment",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<double>(
                name: "TotalPDV",
                table: "Invoices",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Invoices",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRebate",
                table: "Invoices",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPDV",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalRebate",
                table: "Invoices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfPayment",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
