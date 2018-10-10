using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class Employee_Bora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
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
                    EmployeeCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Passport = table.Column<string>(nullable: true),
                    Interest = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    EmbassyDate = table.Column<DateTime>(nullable: false),
                    VisaFrom = table.Column<DateTime>(nullable: false),
                    VisaTo = table.Column<DateTime>(nullable: false),
                    WorkPermitFrom = table.Column<DateTime>(nullable: false),
                    WorkPermitTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeItems",
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
                    FamilyMemberId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeItems_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeItems_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeItems_FamilyMembers_FamilyMemberId",
                        column: x => x.FamilyMemberId,
                        principalTable: "FamilyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_CompanyId",
                table: "EmployeeItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_CreatedById",
                table: "EmployeeItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_EmployeeId",
                table: "EmployeeItems",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_FamilyMemberId",
                table: "EmployeeItems",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeItems");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
