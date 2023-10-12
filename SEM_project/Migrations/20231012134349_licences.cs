using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class licences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerToLicence",
                columns: table => new
                {
                    ComputerToLicenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComputerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerToLicence", x => x.ComputerToLicenceId);
                });

            migrationBuilder.CreateTable(
                name: "Licence",
                columns: table => new
                {
                    LicenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenceName = table.Column<int>(type: "int", nullable: false),
                    LicenceStatus = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licence", x => x.LicenceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerToLicence");

            migrationBuilder.DropTable(
                name: "Licence");
        }
    }
}
