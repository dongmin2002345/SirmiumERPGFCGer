using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class BusinessPartner_Milanka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPartnerTypes",
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
                    Name = table.Column<string>(nullable: true),
                    IsBuyer = table.Column<bool>(nullable: false),
                    IsSupplier = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartnerBusinessPartnerTypes",
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
                    BusinessPartnerId = table.Column<int>(nullable: false),
                    BusinessPartnerTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerBusinessPartnerTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerBusinessPartnerTypes_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerBusinessPartnerTypes_BusinessPartnerTypes_BusinessPartnerTypeId",
                        column: x => x.BusinessPartnerTypeId,
                        principalTable: "BusinessPartnerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerBusinessPartnerTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerBusinessPartnerTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerBusinessPartnerTypes_BusinessPartnerId",
                table: "BusinessPartnerBusinessPartnerTypes",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerBusinessPartnerTypes_BusinessPartnerTypeId",
                table: "BusinessPartnerBusinessPartnerTypes",
                column: "BusinessPartnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerBusinessPartnerTypes_CompanyId",
                table: "BusinessPartnerBusinessPartnerTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerBusinessPartnerTypes_CreatedById",
                table: "BusinessPartnerBusinessPartnerTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerTypes_CompanyId",
                table: "BusinessPartnerTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerTypes_CreatedById",
                table: "BusinessPartnerTypes",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerBusinessPartnerTypes");

            migrationBuilder.DropTable(
                name: "BusinessPartnerTypes");
        }
    }
}
