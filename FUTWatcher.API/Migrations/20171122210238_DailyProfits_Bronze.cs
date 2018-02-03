using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FUTWatcher.API.Migrations
{
    public partial class DailyProfits_Bronze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BronzePacksEarned",
                table: "DailyProfits",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "BronzePacksSpent",
                table: "DailyProfits",
                type: "int8",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BronzePacksEarned",
                table: "DailyProfits");

            migrationBuilder.DropColumn(
                name: "BronzePacksSpent",
                table: "DailyProfits");
        }
    }
}
