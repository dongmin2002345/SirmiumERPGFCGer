using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class BusinessPartner_Izmene_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rebate",
                table: "BusinessPartners");

            migrationBuilder.RenameColumn(
                name: "PDV",
                table: "BusinessPartners",
                newName: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VatId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_DiscountId",
                table: "BusinessPartners",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_VatId",
                table: "BusinessPartners",
                column: "VatId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Discounts_DiscountId",
                table: "BusinessPartners",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Vats_VatId",
                table: "BusinessPartners",
                column: "VatId",
                principalTable: "Vats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Discounts_DiscountId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Vats_VatId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_DiscountId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_VatId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "VatId",
                table: "BusinessPartners");

            migrationBuilder.RenameColumn(
                name: "Customer",
                table: "BusinessPartners",
                newName: "PDV");

            migrationBuilder.AddColumn<decimal>(
                name: "Rebate",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
