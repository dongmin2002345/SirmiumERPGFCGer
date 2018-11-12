using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class KalkulacijeNaGradilistima_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConstructionSiteCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ConstructionSiteId = table.Column<int>(nullable: true),
                    NumOfEmployees = table.Column<int>(nullable: false),
                    EmployeePrice = table.Column<decimal>(nullable: false),
                    NumOfMonths = table.Column<int>(nullable: false),
                    OldValue = table.Column<decimal>(nullable: false),
                    NewValue = table.Column<decimal>(nullable: false),
                    ValueDifference = table.Column<decimal>(nullable: false),
                    PlusMinus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionSiteCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteCalculations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteCalculations_ConstructionSites_ConstructionSiteId",
                        column: x => x.ConstructionSiteId,
                        principalTable: "ConstructionSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteCalculations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteCalculations_CompanyId",
                table: "ConstructionSiteCalculations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteCalculations_ConstructionSiteId",
                table: "ConstructionSiteCalculations",
                column: "ConstructionSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteCalculations_CreatedById",
                table: "ConstructionSiteCalculations",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstructionSiteCalculations");
        }
    }
}
