using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaInvoiceItemNedeljko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "InvoiceItems",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyPriceWithPDV",
                table: "InvoiceItems",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "InvoiceItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "CurrencyPriceWithPDV",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "InvoiceItems");
        }
    }
}
