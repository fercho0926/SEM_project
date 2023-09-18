using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Users_AppId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Users_AppId",
                table: "AspNetRoles",
                column: "Users_AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Users_App_Users_AppId",
                table: "AspNetRoles",
                column: "Users_AppId",
                principalTable: "Users_App",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Users_App_Users_AppId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_Users_AppId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Users_AppId",
                table: "AspNetRoles");
        }
    }
}
