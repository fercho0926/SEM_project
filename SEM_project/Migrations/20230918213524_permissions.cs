using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAdminPermissions",
                table: "RoleViewModel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasComputerPermissions",
                table: "RoleViewModel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasLicensesPermissions",
                table: "RoleViewModel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSoftwarePermissions",
                table: "RoleViewModel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAdminPermissions",
                table: "RoleViewModel");

            migrationBuilder.DropColumn(
                name: "HasComputerPermissions",
                table: "RoleViewModel");

            migrationBuilder.DropColumn(
                name: "HasLicensesPermissions",
                table: "RoleViewModel");

            migrationBuilder.DropColumn(
                name: "HasSoftwarePermissions",
                table: "RoleViewModel");
        }
    }
}
