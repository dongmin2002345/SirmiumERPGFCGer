using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class ImzmeneNaInstitucijama_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "BusinessPartnerInstitutions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "BusinessPartnerInstitutions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "BusinessPartnerInstitutions");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "BusinessPartnerInstitutions");
        }
    }
}
