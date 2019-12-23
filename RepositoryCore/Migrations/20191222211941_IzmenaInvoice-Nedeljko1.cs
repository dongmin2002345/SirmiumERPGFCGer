using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaInvoiceNedeljko1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_BusinessPartners_BusinessPartnerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsInPDV",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "PIB",
                table: "Invoices",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DateOfSupplyOfGoods",
                table: "Invoices",
                newName: "StatusDate");

            migrationBuilder.RenameColumn(
                name: "Customer",
                table: "Invoices",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Invoices",
                newName: "DateOfPayment");

            migrationBuilder.RenameColumn(
                name: "BusinessPartnerId",
                table: "Invoices",
                newName: "BuyerId");

            migrationBuilder.RenameColumn(
                name: "BPName",
                table: "Invoices",
                newName: "BuyerName");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_BusinessPartnerId",
                table: "Invoices",
                newName: "IX_Invoices_BuyerId");

            migrationBuilder.AddColumn<double>(
                name: "CurrencyExchangeRate",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Invoices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_BusinessPartners_BuyerId",
                table: "Invoices",
                column: "BuyerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_BusinessPartners_BuyerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CurrencyExchangeRate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "StatusDate",
                table: "Invoices",
                newName: "DateOfSupplyOfGoods");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Invoices",
                newName: "PIB");

            migrationBuilder.RenameColumn(
                name: "DateOfPayment",
                table: "Invoices",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Invoices",
                newName: "Customer");

            migrationBuilder.RenameColumn(
                name: "BuyerName",
                table: "Invoices",
                newName: "BPName");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Invoices",
                newName: "BusinessPartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_BuyerId",
                table: "Invoices",
                newName: "IX_Invoices_BusinessPartnerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsInPDV",
                table: "Invoices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_BusinessPartners_BusinessPartnerId",
                table: "Invoices",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
