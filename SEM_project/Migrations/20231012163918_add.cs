using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ComputerToLicence_ComputerId",
                table: "ComputerToLicence",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerToLicence_Computer_ComputerId",
                table: "ComputerToLicence",
                column: "ComputerId",
                principalTable: "Computer",
                principalColumn: "ComputerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComputerToLicence_Computer_ComputerId",
                table: "ComputerToLicence");

            migrationBuilder.DropIndex(
                name: "IX_ComputerToLicence_ComputerId",
                table: "ComputerToLicence");
        }
    }
}
