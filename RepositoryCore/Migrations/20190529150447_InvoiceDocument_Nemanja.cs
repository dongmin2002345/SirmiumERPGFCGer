using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class InvoiceDocument_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InputInvoiceDocuments",
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
                    InputInvoiceId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputInvoiceDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputInvoiceDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoiceDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoiceDocuments_InputInvoices_InputInvoiceId",
                        column: x => x.InputInvoiceId,
                        principalTable: "InputInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutputInvoiceDocuments",
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
                    OutputInvoiceId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputInvoiceDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceDocuments_OutputInvoices_OutputInvoiceId",
                        column: x => x.OutputInvoiceId,
                        principalTable: "OutputInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceDocuments_CompanyId",
                table: "InputInvoiceDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceDocuments_CreatedById",
                table: "InputInvoiceDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceDocuments_InputInvoiceId",
                table: "InputInvoiceDocuments",
                column: "InputInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceDocuments_CompanyId",
                table: "OutputInvoiceDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceDocuments_CreatedById",
                table: "OutputInvoiceDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceDocuments_OutputInvoiceId",
                table: "OutputInvoiceDocuments",
                column: "OutputInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputInvoiceDocuments");

            migrationBuilder.DropTable(
                name: "OutputInvoiceDocuments");
        }
    }
}
