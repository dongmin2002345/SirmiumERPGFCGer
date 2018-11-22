using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class PutanjeURacunima_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "OutputInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "InputInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "InputInvoices");
        }
    }
}
