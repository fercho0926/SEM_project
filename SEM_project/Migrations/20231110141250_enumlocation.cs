﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class enumlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnumLocation",
                table: "Computer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnumLocation",
                table: "Computer");
        }
    }
}
