using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace xTremeShop.Migrations
{
    public partial class userlibraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLibraries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryApps",
                columns: table => new
                {
                    LibaryId = table.Column<int>(nullable: false),
                    AppId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryApps", x => new { x.LibaryId, x.AppId });
                    table.ForeignKey(
                        name: "FK_LibraryApps_MobileApps_AppId",
                        column: x => x.AppId,
                        principalTable: "MobileApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryApps_UserLibraries_LibaryId",
                        column: x => x.LibaryId,
                        principalTable: "UserLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryApps_AppId",
                table: "LibraryApps",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibraries_UserId",
                table: "UserLibraries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryApps");

            migrationBuilder.DropTable(
                name: "UserLibraries");
        }
    }
}
