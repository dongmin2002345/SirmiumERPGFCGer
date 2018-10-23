using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class IzmenaEmployeeNedeljko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interest",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "License",
                table: "Employees",
                newName: "ResidenceAddress");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportCityId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportCountryId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceCityId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisaDate",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VisaValidFrom",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VisaValidTo",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "BusinessPartnerLocations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CityId",
                table: "Employees",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CountryId",
                table: "Employees",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_MunicipalityId",
                table: "Employees",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PassportCityId",
                table: "Employees",
                column: "PassportCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PassportCountryId",
                table: "Employees",
                column: "PassportCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RegionId",
                table: "Employees",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ResidenceCityId",
                table: "Employees",
                column: "ResidenceCityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerLocations_RegionId",
                table: "BusinessPartnerLocations",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartnerLocations_Regions_RegionId",
                table: "BusinessPartnerLocations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Countries_CountryId",
                table: "Employees",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Municipalities_MunicipalityId",
                table: "Employees",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_PassportCityId",
                table: "Employees",
                column: "PassportCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Countries_PassportCountryId",
                table: "Employees",
                column: "PassportCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Regions_RegionId",
                table: "Employees",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_ResidenceCityId",
                table: "Employees",
                column: "ResidenceCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartnerLocations_Regions_RegionId",
                table: "BusinessPartnerLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_CountryId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Municipalities_MunicipalityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_PassportCityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_PassportCountryId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Regions_RegionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_ResidenceCityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CountryId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_MunicipalityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PassportCityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PassportCountryId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RegionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ResidenceCityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartnerLocations_RegionId",
                table: "BusinessPartnerLocations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PassportCityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PassportCountryId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ResidenceCityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "VisaDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "VisaValidFrom",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "VisaValidTo",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "BusinessPartnerLocations");

            migrationBuilder.RenameColumn(
                name: "ResidenceAddress",
                table: "Employees",
                newName: "License");

            migrationBuilder.AddColumn<string>(
                name: "Interest",
                table: "Employees",
                nullable: true);
        }
    }
}
