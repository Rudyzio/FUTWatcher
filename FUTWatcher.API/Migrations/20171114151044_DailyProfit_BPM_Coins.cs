using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FUTWatcher.API.Migrations
{
    public partial class DailyProfit_BPM_Coins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BronzePacksOpended",
                table: "DailyProfits",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaxCoins",
                table: "DailyProfits",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MinCoins",
                table: "DailyProfits",
                type: "int8",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BronzePacksOpended",
                table: "DailyProfits");

            migrationBuilder.DropColumn(
                name: "MaxCoins",
                table: "DailyProfits");

            migrationBuilder.DropColumn(
                name: "MinCoins",
                table: "DailyProfits");
        }
    }
}
