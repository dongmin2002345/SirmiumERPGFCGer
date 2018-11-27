using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class BusinessPartnerGerIzmene_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BetriebsNumber",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInPDVGer",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TaxAdministrationId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_TaxAdministrationId",
                table: "BusinessPartners",
                column: "TaxAdministrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_TaxAdministrations_TaxAdministrationId",
                table: "BusinessPartners",
                column: "TaxAdministrationId",
                principalTable: "TaxAdministrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_TaxAdministrations_TaxAdministrationId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_TaxAdministrationId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "BetriebsNumber",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IsInPDVGer",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "TaxAdministrationId",
                table: "BusinessPartners");
        }
    }
}
