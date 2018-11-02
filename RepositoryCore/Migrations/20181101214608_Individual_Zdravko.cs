using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class Individual_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Individuals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    EmbassyDate = table.Column<DateTime>(nullable: false),
                    Family = table.Column<bool>(nullable: false),
                    Identifier = table.Column<Guid>(nullable: false),
                    Interest = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Passport = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    VisaFrom = table.Column<DateTime>(nullable: false),
                    VisaTo = table.Column<DateTime>(nullable: false),
                    WorkPermitFrom = table.Column<DateTime>(nullable: false),
                    WorkPermitTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Individuals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_CompanyId",
                table: "Individuals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_CreatedById",
                table: "Individuals",
                column: "CreatedById");
        }
    }
}
