using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_IdentityUser_UserId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "IdentityRole");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_UserId",
                table: "IdentityRole",
                newName: "IX_IdentityRole_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_NormalizedName",
                table: "IdentityRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRole_IdentityUser_UserId",
                table: "IdentityRole",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRole_IdentityUser_UserId",
                table: "IdentityRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole");

            migrationBuilder.DropIndex(
                name: "IX_IdentityRole_NormalizedName",
                table: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityRole_UserId",
                table: "Roles",
                newName: "IX_Roles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_IdentityUser_UserId",
                table: "Roles",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
