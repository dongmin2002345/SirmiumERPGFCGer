using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class BusinessPartnerIzmene_Milanka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityCode",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "BranchOpeningDate",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "OpeningDate",
                table: "BusinessPartners");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "BusinessPartners",
                newName: "WebSite");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "BusinessPartners",
                newName: "PIO");

            migrationBuilder.RenameColumn(
                name: "MatCode",
                table: "BusinessPartners",
                newName: "PDV");

            migrationBuilder.RenameColumn(
                name: "InoAddress",
                table: "BusinessPartners",
                newName: "JBKJS");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "BusinessPartners",
                newName: "IndustryCode");

            migrationBuilder.RenameColumn(
                name: "Director",
                table: "BusinessPartners",
                newName: "IdentificationNumber");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "BusinessPartners",
                newName: "ContactPerson");

            migrationBuilder.AddColumn<int>(
                name: "DueDate",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsInPDV",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Rebate",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BusinessPartnerLocations",
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
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    MunicipalityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerLocations_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartnerOrganizationUnits",
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
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    MunicipalityId = table.Column<int>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerOrganizationUnits_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartnerPhones",
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
                    BusinessPartnerId = table.Column<int>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerPhones_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerPhones_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerPhones_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_BusinessPartnerId",
                table: "BusinessPartnerLocations",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_CityId",
                table: "BusinessPartnerLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_CompanyId",
                table: "BusinessPartnerLocations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_CountryId",
                table: "BusinessPartnerLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_CreatedById",
                table: "BusinessPartnerLocations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_MunicipalityId",
                table: "BusinessPartnerLocations",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_BusinessPartnerId",
                table: "BusinessPartnerOrganizationUnits",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_CityId",
                table: "BusinessPartnerOrganizationUnits",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_CompanyId",
                table: "BusinessPartnerOrganizationUnits",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_CountryId",
                table: "BusinessPartnerOrganizationUnits",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_CreatedById",
                table: "BusinessPartnerOrganizationUnits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerOrganizationUnits_MunicipalityId",
                table: "BusinessPartnerOrganizationUnits",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerPhones_BusinessPartnerId",
                table: "BusinessPartnerPhones",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerPhones_CompanyId",
                table: "BusinessPartnerPhones",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerPhones_CreatedById",
                table: "BusinessPartnerPhones",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerLocations");

            migrationBuilder.DropTable(
                name: "BusinessPartnerOrganizationUnits");

            migrationBuilder.DropTable(
                name: "BusinessPartnerPhones");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IsInPDV",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Rebate",
                table: "BusinessPartners");

            migrationBuilder.RenameColumn(
                name: "WebSite",
                table: "BusinessPartners",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "PIO",
                table: "BusinessPartners",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "PDV",
                table: "BusinessPartners",
                newName: "MatCode");

            migrationBuilder.RenameColumn(
                name: "JBKJS",
                table: "BusinessPartners",
                newName: "InoAddress");

            migrationBuilder.RenameColumn(
                name: "IndustryCode",
                table: "BusinessPartners",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "IdentificationNumber",
                table: "BusinessPartners",
                newName: "Director");

            migrationBuilder.RenameColumn(
                name: "ContactPerson",
                table: "BusinessPartners",
                newName: "BankAccountNumber");

            migrationBuilder.AddColumn<string>(
                name: "ActivityCode",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BranchOpeningDate",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningDate",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
