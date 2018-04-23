using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace xTremeShop.Migrations
{
    public partial class appIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AppIcon",
                table: "MobileApps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppIcon",
                table: "MobileApps");
        }
    }
}
