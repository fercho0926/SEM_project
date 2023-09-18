using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEM_project.Migrations
{
    public partial class rolews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RoleViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Users_AppId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleViewModel_Users_App_Users_AppId",
                        column: x => x.Users_AppId,
                        principalTable: "Users_App",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleViewModel_Users_AppId",
                table: "RoleViewModel",
                column: "Users_AppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleViewModel");

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
    }
}
