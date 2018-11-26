using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class ConstructionSiteBusinessPartnerDocument_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPartnerDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerDocuments_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConstructionSiteDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    ConstructionSiteId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionSiteDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteDocuments_ConstructionSites_ConstructionSiteId",
                        column: x => x.ConstructionSiteId,
                        principalTable: "ConstructionSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionSiteDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerDocuments_BusinessPartnerId",
                table: "BusinessPartnerDocuments",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerDocuments_CompanyId",
                table: "BusinessPartnerDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerDocuments_CreatedById",
                table: "BusinessPartnerDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteDocuments_CompanyId",
                table: "ConstructionSiteDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteDocuments_ConstructionSiteId",
                table: "ConstructionSiteDocuments",
                column: "ConstructionSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSiteDocuments_CreatedById",
                table: "ConstructionSiteDocuments",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerDocuments");

            migrationBuilder.DropTable(
                name: "ConstructionSiteDocuments");
        }
    }
}
