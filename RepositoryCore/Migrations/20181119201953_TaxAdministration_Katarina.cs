using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class TaxAdministration_Katarina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxAdministrations",
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
                    CityId = table.Column<int>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Address3 = table.Column<string>(nullable: true),
                    BankId1 = table.Column<int>(nullable: true),
                    Bank1Id = table.Column<int>(nullable: true),
                    BankId2 = table.Column<int>(nullable: true),
                    Bank2Id = table.Column<int>(nullable: true),
                    IBAN1 = table.Column<int>(nullable: false),
                    SWIFT = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxAdministrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxAdministrations_Banks_Bank1Id",
                        column: x => x.Bank1Id,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxAdministrations_Banks_Bank2Id",
                        column: x => x.Bank2Id,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxAdministrations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxAdministrations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxAdministrations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxAdministrations_Bank1Id",
                table: "TaxAdministrations",
                column: "Bank1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaxAdministrations_Bank2Id",
                table: "TaxAdministrations",
                column: "Bank2Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaxAdministrations_CityId",
                table: "TaxAdministrations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxAdministrations_CompanyId",
                table: "TaxAdministrations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxAdministrations_CreatedById",
                table: "TaxAdministrations",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxAdministrations");
        }
    }
}
