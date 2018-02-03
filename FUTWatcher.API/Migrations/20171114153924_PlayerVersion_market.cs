using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FUTWatcher.API.Migrations
{
    public partial class PlayerVersion_market : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions");

            migrationBuilder.AlterColumn<long>(
                name: "NationId",
                table: "PlayerVersions",
                type: "int8",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "MarketCost",
                table: "PlayerVersions",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UpdateDate",
                table: "PlayerVersions",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions",
                column: "NationId",
                principalTable: "Nations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions");

            migrationBuilder.DropColumn(
                name: "MarketCost",
                table: "PlayerVersions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "PlayerVersions");

            migrationBuilder.AlterColumn<long>(
                name: "NationId",
                table: "PlayerVersions",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "int8",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions",
                column: "NationId",
                principalTable: "Nations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
