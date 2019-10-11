using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class PhysicalPersons_Izmene_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonProfessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonLicences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "PhysicalPersonCards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonProfessions");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonNotes");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonLicences");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonItems");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonDocuments");

            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "PhysicalPersonCards");
        }
    }
}
