using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class OutputInvoice_Izmene_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OutputInvoices");

            migrationBuilder.RenameColumn(
                name: "PDV",
                table: "OutputInvoices",
                newName: "Pdv");

            migrationBuilder.RenameColumn(
                name: "TrafficDate",
                table: "OutputInvoices",
                newName: "StatusDate");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OutputInvoices",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "RebateValue",
                table: "OutputInvoices",
                newName: "AmountNet");

            migrationBuilder.RenameColumn(
                name: "Rebate",
                table: "OutputInvoices",
                newName: "AmountGross");

            migrationBuilder.RenameColumn(
                name: "InvoiceType",
                table: "OutputInvoices",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "Construction",
                table: "OutputInvoices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "BusinessPartner",
                table: "OutputInvoices",
                newName: "InvoiceNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "OutputInvoices",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OutputInvoices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "OutputInvoices",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfPayment",
                table: "OutputInvoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OutputInvoices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PdvPercent",
                table: "OutputInvoices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_BusinessPartnerId",
                table: "OutputInvoices",
                column: "BusinessPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_BusinessPartners_BusinessPartnerId",
                table: "OutputInvoices",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_BusinessPartners_BusinessPartnerId",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_BusinessPartnerId",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "DateOfPayment",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "PdvPercent",
                table: "OutputInvoices");

            migrationBuilder.RenameColumn(
                name: "Pdv",
                table: "OutputInvoices",
                newName: "PDV");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "OutputInvoices",
                newName: "InvoiceType");

            migrationBuilder.RenameColumn(
                name: "StatusDate",
                table: "OutputInvoices",
                newName: "TrafficDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OutputInvoices",
                newName: "Construction");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "OutputInvoices",
                newName: "BusinessPartner");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "OutputInvoices",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "AmountNet",
                table: "OutputInvoices",
                newName: "RebateValue");

            migrationBuilder.RenameColumn(
                name: "AmountGross",
                table: "OutputInvoices",
                newName: "Rebate");

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "OutputInvoices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Base",
                table: "OutputInvoices",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OutputInvoices",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "OutputInvoices",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
