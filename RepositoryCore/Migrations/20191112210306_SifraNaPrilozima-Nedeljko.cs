using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class SifraNaPrilozimaNedeljko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PhysicalPersonAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "EmployeeAttachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "PhysicalPersonAttachments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "EmployeeAttachments");
        }
    }
}
