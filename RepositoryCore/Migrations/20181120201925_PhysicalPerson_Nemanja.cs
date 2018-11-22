using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class PhysicalPerson_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhysicalPersons",
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
                    PhysicalPersonCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    ConstructionSiteCode = table.Column<string>(nullable: true),
                    ConstructionSiteName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    RegionId = table.Column<int>(nullable: true),
                    MunicipalityId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PassportCountryId = table.Column<int>(nullable: true),
                    PassportCityId = table.Column<int>(nullable: true),
                    Passport = table.Column<string>(nullable: true),
                    VisaFrom = table.Column<DateTime>(nullable: true),
                    VisaTo = table.Column<DateTime>(nullable: true),
                    ResidenceCountryId = table.Column<int>(nullable: true),
                    ResidenceCityId = table.Column<int>(nullable: true),
                    ResidenceAddress = table.Column<string>(nullable: true),
                    EmbassyDate = table.Column<DateTime>(nullable: true),
                    VisaDate = table.Column<DateTime>(nullable: true),
                    VisaValidFrom = table.Column<DateTime>(nullable: true),
                    VisaValidTo = table.Column<DateTime>(nullable: true),
                    WorkPermitFrom = table.Column<DateTime>(nullable: true),
                    WorkPermitTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Cities_PassportCityId",
                        column: x => x.PassportCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Countries_PassportCountryId",
                        column: x => x.PassportCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Cities_ResidenceCityId",
                        column: x => x.ResidenceCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersons_Countries_ResidenceCountryId",
                        column: x => x.ResidenceCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_CityId",
                table: "PhysicalPersons",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_CompanyId",
                table: "PhysicalPersons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_CountryId",
                table: "PhysicalPersons",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_CreatedById",
                table: "PhysicalPersons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_MunicipalityId",
                table: "PhysicalPersons",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_PassportCityId",
                table: "PhysicalPersons",
                column: "PassportCityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_PassportCountryId",
                table: "PhysicalPersons",
                column: "PassportCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_RegionId",
                table: "PhysicalPersons",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_ResidenceCityId",
                table: "PhysicalPersons",
                column: "ResidenceCityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersons_ResidenceCountryId",
                table: "PhysicalPersons",
                column: "ResidenceCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhysicalPersons");
        }
    }
}
