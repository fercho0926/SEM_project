using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class isactive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssigned",
                table: "Licence");

            migrationBuilder.RenameColumn(
                name: "LicenceStatus",
                table: "Licence",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Licence",
                newName: "LicenceStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigned",
                table: "Licence",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
