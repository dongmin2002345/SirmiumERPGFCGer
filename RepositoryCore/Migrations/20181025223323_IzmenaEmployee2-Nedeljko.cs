using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaEmployee2Nedeljko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLicences",
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
                    EmployeeId = table.Column<int>(nullable: true),
                    LicenceId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: true),
                    ValidTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLicences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_LicenceTypes_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "LicenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfessions",
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
                    EmployeeId = table.Column<int>(nullable: true),
                    ProfessionId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_CompanyId",
                table: "EmployeeLicences",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_CountryId",
                table: "EmployeeLicences",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_CreatedById",
                table: "EmployeeLicences",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_EmployeeId",
                table: "EmployeeLicences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_LicenceId",
                table: "EmployeeLicences",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_CompanyId",
                table: "EmployeeProfessions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_CountryId",
                table: "EmployeeProfessions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_CreatedById",
                table: "EmployeeProfessions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_EmployeeId",
                table: "EmployeeProfessions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_ProfessionId",
                table: "EmployeeProfessions",
                column: "ProfessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLicences");

            migrationBuilder.DropTable(
                name: "EmployeeProfessions");
        }
    }
}
