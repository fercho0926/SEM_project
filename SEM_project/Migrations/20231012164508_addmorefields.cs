using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class addmorefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ComputerToLicence_LicenceId",
                table: "ComputerToLicence",
                column: "LicenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerToLicence_Licence_LicenceId",
                table: "ComputerToLicence",
                column: "LicenceId",
                principalTable: "Licence",
                principalColumn: "LicenceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComputerToLicence_Licence_LicenceId",
                table: "ComputerToLicence");

            migrationBuilder.DropIndex(
                name: "IX_ComputerToLicence_LicenceId",
                table: "ComputerToLicence");
        }
    }
}
