using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class FizickaLica_Milanka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhysicalPersonCards",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    CardDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PlusMinus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonCards_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonCards_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonCards_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPersonDocuments",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonDocuments_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPersonItems",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    FamilyMemberId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    EmbassyDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonItems_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonItems_FamilyMembers_FamilyMemberId",
                        column: x => x.FamilyMemberId,
                        principalTable: "FamilyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonItems_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPersonLicences",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    LicenceId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: true),
                    ValidTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonLicences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonLicences_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonLicences_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonLicences_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonLicences_LicenceTypes_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "LicenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonLicences_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPersonNotes",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    NoteDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonNotes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonNotes_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPersonProfessions",
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
                    PhysicalPersonId = table.Column<int>(nullable: true),
                    ProfessionId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPersonProfessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonProfessions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonProfessions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonProfessions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonProfessions_PhysicalPersons_PhysicalPersonId",
                        column: x => x.PhysicalPersonId,
                        principalTable: "PhysicalPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalPersonProfessions_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonCards_CompanyId",
                table: "PhysicalPersonCards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonCards_CreatedById",
                table: "PhysicalPersonCards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonCards_PhysicalPersonId",
                table: "PhysicalPersonCards",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonDocuments_CompanyId",
                table: "PhysicalPersonDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonDocuments_CreatedById",
                table: "PhysicalPersonDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonDocuments_PhysicalPersonId",
                table: "PhysicalPersonDocuments",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonItems_CompanyId",
                table: "PhysicalPersonItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonItems_CreatedById",
                table: "PhysicalPersonItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonItems_FamilyMemberId",
                table: "PhysicalPersonItems",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonItems_PhysicalPersonId",
                table: "PhysicalPersonItems",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonLicences_CompanyId",
                table: "PhysicalPersonLicences",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonLicences_CountryId",
                table: "PhysicalPersonLicences",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonLicences_CreatedById",
                table: "PhysicalPersonLicences",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonLicences_LicenceId",
                table: "PhysicalPersonLicences",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonLicences_PhysicalPersonId",
                table: "PhysicalPersonLicences",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonNotes_CompanyId",
                table: "PhysicalPersonNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonNotes_CreatedById",
                table: "PhysicalPersonNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonNotes_PhysicalPersonId",
                table: "PhysicalPersonNotes",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonProfessions_CompanyId",
                table: "PhysicalPersonProfessions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonProfessions_CountryId",
                table: "PhysicalPersonProfessions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonProfessions_CreatedById",
                table: "PhysicalPersonProfessions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonProfessions_PhysicalPersonId",
                table: "PhysicalPersonProfessions",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalPersonProfessions_ProfessionId",
                table: "PhysicalPersonProfessions",
                column: "ProfessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhysicalPersonCards");

            migrationBuilder.DropTable(
                name: "PhysicalPersonDocuments");

            migrationBuilder.DropTable(
                name: "PhysicalPersonItems");

            migrationBuilder.DropTable(
                name: "PhysicalPersonLicences");

            migrationBuilder.DropTable(
                name: "PhysicalPersonNotes");

            migrationBuilder.DropTable(
                name: "PhysicalPersonProfessions");
        }
    }
}
