using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class Izmene_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "ServiceDeliverys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "Agencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "ServiceDeliverys");

            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "Agencies");
        }
    }
}
