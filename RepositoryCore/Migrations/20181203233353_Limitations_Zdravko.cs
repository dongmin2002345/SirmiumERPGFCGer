using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class Limitations_Zdravko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Limitations",
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
                    ConstructionSiteLimit = table.Column<int>(nullable: false),
                    BusinessPartnerConstructionSiteLimit = table.Column<int>(nullable: false),
                    EmployeeConstructionSiteLimit = table.Column<int>(nullable: false),
                    EmployeeBusinessPartnerLimit = table.Column<int>(nullable: false),
                    EmployeeBirthdayLimit = table.Column<int>(nullable: false),
                    EmployeePassportLimit = table.Column<int>(nullable: false),
                    EmployeeEmbasyLimit = table.Column<int>(nullable: false),
                    EmployeeVisaTakeOffLimit = table.Column<int>(nullable: false),
                    EmployeeVisaLimit = table.Column<int>(nullable: false),
                    EmployeeWorkLicenceLimit = table.Column<int>(nullable: false),
                    EmployeeDriveLicenceLimit = table.Column<int>(nullable: false),
                    EmployeeEmbasyFamilyLimit = table.Column<int>(nullable: false),
                    PersonPassportLimit = table.Column<int>(nullable: false),
                    PersonEmbasyLimit = table.Column<int>(nullable: false),
                    PersonVisaTakeOffLimit = table.Column<int>(nullable: false),
                    PersonVisaLimit = table.Column<int>(nullable: false),
                    PersonWorkLicenceLimit = table.Column<int>(nullable: false),
                    PersonDriveLicenceLimit = table.Column<int>(nullable: false),
                    PersonEmbasyFamilyLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Limitations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Limitations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Limitations_CompanyId",
                table: "Limitations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Limitations_CreatedById",
                table: "Limitations",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Limitations");
        }
    }
}
