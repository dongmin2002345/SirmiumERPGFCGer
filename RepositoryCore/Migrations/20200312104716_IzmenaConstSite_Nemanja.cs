using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaConstSite_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSites_ShipmentId",
                table: "ConstructionSites",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSites_Shipments_ShipmentId",
                table: "ConstructionSites",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSites_Shipments_ShipmentId",
                table: "ConstructionSites");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSites_ShipmentId",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "ConstructionSites");
        }
    }
}
