using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaConstructionSite_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ConstructionSites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "ConstructionSites",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentValue",
                table: "ConstructionSites",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostingDate",
                table: "ConstructionSiteCalculations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "PaymentValue",
                table: "ConstructionSites");

            migrationBuilder.DropColumn(
                name: "PostingDate",
                table: "ConstructionSiteCalculations");
        }
    }
}
