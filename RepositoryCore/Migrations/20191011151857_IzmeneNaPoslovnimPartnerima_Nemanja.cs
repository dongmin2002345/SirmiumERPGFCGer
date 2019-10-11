using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneNaPoslovnimPartnerima_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerPhones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerInstitutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "BusinessPartnerBanks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerTypes");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerPhones");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerNotes");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerLocations");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerInstitutions");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerDocuments");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "BusinessPartnerBanks");
        }
    }
}
