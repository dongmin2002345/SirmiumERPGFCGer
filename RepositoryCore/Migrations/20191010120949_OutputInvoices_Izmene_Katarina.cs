using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class OutputInvoices_Izmene_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "OutputInvoiceNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "OutputInvoiceDocuments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "OutputInvoiceNotes");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "OutputInvoiceDocuments");
        }
    }
}
