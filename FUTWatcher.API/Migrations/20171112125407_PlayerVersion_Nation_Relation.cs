using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FUTWatcher.API.Migrations
{
    public partial class PlayerVersion_Nation_Relation : Migration
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
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions",
                column: "NationId",
                principalTable: "Nations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions");

            migrationBuilder.AlterColumn<long>(
                name: "NationId",
                table: "PlayerVersions",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "int8");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerVersions_Nations_NationId",
                table: "PlayerVersions",
                column: "NationId",
                principalTable: "Nations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
