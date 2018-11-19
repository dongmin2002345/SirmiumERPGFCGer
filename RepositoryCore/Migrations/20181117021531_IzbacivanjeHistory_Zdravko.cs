using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzbacivanjeHistory_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerByConstructionSiteHistories");

            migrationBuilder.DropTable(
                name: "EmployeeByBusinessPartnerHistories");

            migrationBuilder.DropTable(
                name: "EmployeeByConstructionSiteHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPartnerByConstructionSiteHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ConstructionSiteId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
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
                name: "EmployeeByBusinessPartnerHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
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
                name: "EmployeeByConstructionSiteHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ConstructionSiteId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeByConstructionSiteHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeByConstructionSiteHistories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByConstructionSiteHistories_ConstructionSites_ConstructionSiteId",
                        column: x => x.ConstructionSiteId,
                        principalTable: "ConstructionSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByConstructionSiteHistories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeByConstructionSiteHistories_Employees_EmployeeId",
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
                name: "IX_EmployeeByConstructionSiteHistories_CompanyId",
                table: "EmployeeByConstructionSiteHistories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByConstructionSiteHistories_ConstructionSiteId",
                table: "EmployeeByConstructionSiteHistories",
                column: "ConstructionSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByConstructionSiteHistories_CreatedById",
                table: "EmployeeByConstructionSiteHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeByConstructionSiteHistories_EmployeeId",
                table: "EmployeeByConstructionSiteHistories",
                column: "EmployeeId");
        }
    }
}
