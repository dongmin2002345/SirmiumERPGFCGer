using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaPP_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactPerson",
                table: "BusinessPartnerPhones",
                newName: "ContactPersonLastName");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonFirstName",
                table: "BusinessPartnerPhones",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPersonFirstName",
                table: "BusinessPartnerPhones");

            migrationBuilder.RenameColumn(
                name: "ContactPersonLastName",
                table: "BusinessPartnerPhones",
                newName: "ContactPerson");
        }
    }
}
