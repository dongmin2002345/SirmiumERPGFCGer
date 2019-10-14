using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneRadnici_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeProfessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeLicences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "EmployeeCards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeProfessions");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeNotes");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeLicences");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeItems");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "EmployeeCards");
        }
    }
}
