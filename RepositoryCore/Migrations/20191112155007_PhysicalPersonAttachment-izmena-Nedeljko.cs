using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class PhysicalPersonAttachmentizmenaNedeljko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalPersonAttachments_Employees_EmployeeId",
                table: "PhysicalPersonAttachments");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "PhysicalPersonAttachments",
                newName: "PhysicalPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalPersonAttachments_EmployeeId",
                table: "PhysicalPersonAttachments",
                newName: "IX_PhysicalPersonAttachments_PhysicalPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalPersonAttachments_PhysicalPersons_PhysicalPersonId",
                table: "PhysicalPersonAttachments",
                column: "PhysicalPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalPersonAttachments_PhysicalPersons_PhysicalPersonId",
                table: "PhysicalPersonAttachments");

            migrationBuilder.RenameColumn(
                name: "PhysicalPersonId",
                table: "PhysicalPersonAttachments",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalPersonAttachments_PhysicalPersonId",
                table: "PhysicalPersonAttachments",
                newName: "IX_PhysicalPersonAttachments_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalPersonAttachments_Employees_EmployeeId",
                table: "PhysicalPersonAttachments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
