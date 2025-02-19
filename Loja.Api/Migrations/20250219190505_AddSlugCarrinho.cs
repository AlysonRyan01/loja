using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugCarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Carrinho",
                type: "NVARCHAR",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Carrinho");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Produto",
                type: "VARCHAR",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 80);
        }
    }
}
