using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmeneBussPar_Milanka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialNr",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonGer",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameGer",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNr",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_AgencyId",
                table: "BusinessPartners",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_SectorId",
                table: "BusinessPartners",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Agencies_AgencyId",
                table: "BusinessPartners",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Sectors_SectorId",
                table: "BusinessPartners",
                column: "SectorId",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Agencies_AgencyId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Sectors_SectorId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_AgencyId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_SectorId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CommercialNr",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "ContactPersonGer",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "NameGer",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "TaxNr",
                table: "BusinessPartners");
        }
    }
}
