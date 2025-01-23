using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carrinho_UserIdentity_UserId",
                table: "Carrinho");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_UserIdentity_UserIdentityId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "UserIdentity");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserIdentityId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Carrinho_UserId",
                table: "Carrinho");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserIdentityId",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserIdentity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarrinhoId = table.Column<long>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIdentity_Carrinho_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalTable: "Carrinho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserIdentityId",
                table: "Roles",
                column: "UserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Carrinho_UserId",
                table: "Carrinho",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentity_CarrinhoId",
                table: "UserIdentity",
                column: "CarrinhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carrinho_UserIdentity_UserId",
                table: "Carrinho",
                column: "UserId",
                principalTable: "UserIdentity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_UserIdentity_UserIdentityId",
                table: "Roles",
                column: "UserIdentityId",
                principalTable: "UserIdentity",
                principalColumn: "Id");
        }
    }
}
