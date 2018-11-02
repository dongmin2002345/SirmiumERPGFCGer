using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class EmployeeByBusinessPartnerHistoryBusinessPartnerByConstructonSiteHistory_Sasa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPartnerByConstructionSiteHistories",
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
                    Code = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    ConstructionSiteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerByConstructionSiteHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSiteHistories_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSiteHistories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSiteHistories_ConstructionSites_ConstructionSiteId",
                        column: x => x.ConstructionSiteId,
                        principalTable: "ConstructionSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSiteHistories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartnerByConstructionSites",
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
                    Code = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    ConstructionSiteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerByConstructionSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSites_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSites_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSites_ConstructionSites_ConstructionSiteId",
                        column: x => x.ConstructionSiteId,
                        principalTable: "ConstructionSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerByConstructionSites_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeByBusinessPartnerHistories",
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
                    Code = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    BusinessPartnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeByBusinessPartnerHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartnerHistories_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartnerHistories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartnerHistories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartnerHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeByBusinessPartners",
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
                    Code = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    BusinessPartnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeByBusinessPartners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartners_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartners_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartners_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByBusinessPartners_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSiteHistories_BusinessPartnerId",
                table: "BusinessPartnerByConstructionSiteHistories",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSiteHistories_CompanyId",
                table: "BusinessPartnerByConstructionSiteHistories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSiteHistories_ConstructionSiteId",
                table: "BusinessPartnerByConstructionSiteHistories",
                column: "ConstructionSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSiteHistories_CreatedById",
                table: "BusinessPartnerByConstructionSiteHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSites_BusinessPartnerId",
                table: "BusinessPartnerByConstructionSites",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSites_CompanyId",
                table: "BusinessPartnerByConstructionSites",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSites_ConstructionSiteId",
                table: "BusinessPartnerByConstructionSites",
                column: "ConstructionSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerByConstructionSites_CreatedById",
                table: "BusinessPartnerByConstructionSites",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartnerHistories_BusinessPartnerId",
                table: "EmployeeByBusinessPartnerHistories",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartnerHistories_CompanyId",
                table: "EmployeeByBusinessPartnerHistories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartnerHistories_CreatedById",
                table: "EmployeeByBusinessPartnerHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartnerHistories_EmployeeId",
                table: "EmployeeByBusinessPartnerHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartners_BusinessPartnerId",
                table: "EmployeeByBusinessPartners",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartners_CompanyId",
                table: "EmployeeByBusinessPartners",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartners_CreatedById",
                table: "EmployeeByBusinessPartners",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByBusinessPartners_EmployeeId",
                table: "EmployeeByBusinessPartners",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerByConstructionSiteHistories");

            migrationBuilder.DropTable(
                name: "BusinessPartnerByConstructionSites");

            migrationBuilder.DropTable(
                name: "EmployeeByBusinessPartnerHistories");

            migrationBuilder.DropTable(
                name: "EmployeeByBusinessPartners");
        }
    }
}
