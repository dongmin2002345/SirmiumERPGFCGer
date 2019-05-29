using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class OutputInoviceInputInvoiceNote_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InputInvoiceNotes",
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
                    Note = table.Column<string>(nullable: true),
                    NoteDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputInvoiceNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputInvoiceNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoiceNotes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoiceNotes_InputInvoices_InputInvoiceId",
                        column: x => x.InputInvoiceId,
                        principalTable: "InputInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutputInvoiceNotes",
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
                    Note = table.Column<string>(nullable: true),
                    NoteDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputInvoiceNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceNotes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoiceNotes_OutputInvoices_OutputInvoiceId",
                        column: x => x.OutputInvoiceId,
                        principalTable: "OutputInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceNotes_CompanyId",
                table: "InputInvoiceNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceNotes_CreatedById",
                table: "InputInvoiceNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoiceNotes_InputInvoiceId",
                table: "InputInvoiceNotes",
                column: "InputInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceNotes_CompanyId",
                table: "OutputInvoiceNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceNotes_CreatedById",
                table: "OutputInvoiceNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoiceNotes_OutputInvoiceId",
                table: "OutputInvoiceNotes",
                column: "OutputInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputInvoiceNotes");

            migrationBuilder.DropTable(
                name: "OutputInvoiceNotes");
        }
    }
}
