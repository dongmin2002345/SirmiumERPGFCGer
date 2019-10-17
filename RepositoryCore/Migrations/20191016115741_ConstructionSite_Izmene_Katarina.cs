using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class ConstructionSite_Izmene_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ConstructionSites");

            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSites_BusinessPartnerId",
                table: "ConstructionSites",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSites_StatusId",
                table: "ConstructionSites",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSites_BusinessPartners_BusinessPartnerId",
                table: "ConstructionSites",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSites_Statuses_StatusId",
                table: "ConstructionSites",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSites_BusinessPartners_BusinessPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSites_Statuses_StatusId",
                table: "ConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSites_BusinessPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSites_StatusId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ConstructionSites");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ConstructionSites",
                nullable: false,
                defaultValue: 0);
        }
    }
}
