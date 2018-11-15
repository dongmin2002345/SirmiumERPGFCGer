using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class InputInvoice_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InputInvoices",
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
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    AmountNet = table.Column<decimal>(nullable: false),
                    PDVPercent = table.Column<int>(nullable: false),
                    PDV = table.Column<decimal>(nullable: false),
                    AmountGross = table.Column<decimal>(nullable: false),
                    Currency = table.Column<decimal>(nullable: false),
                    DateOfPaymet = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    StatusDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputInvoices_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_BusinessPartnerId",
                table: "InputInvoices",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_CompanyId",
                table: "InputInvoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_CreatedById",
                table: "InputInvoices",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputInvoices");
        }
    }
}
