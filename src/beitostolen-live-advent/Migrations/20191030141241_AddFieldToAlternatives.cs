﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace beitostolen_live_api.Migrations
{
    public partial class AddFieldToAlternatives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Alternatives",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Alternatives");
        }
    }
}
