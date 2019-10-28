﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryCore.Migrations
{
    public partial class DodatCallCentar_Nemanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallCentars",
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
                    Code = table.Column<string>(nullable: true),
                    ReceivingDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    EndingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallCentars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallCentars_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CallCentars_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CallCentars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallCentars_CompanyId",
                table: "CallCentars",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CallCentars_CreatedById",
                table: "CallCentars",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CallCentars_UserId",
                table: "CallCentars",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallCentars");
        }
    }
}
