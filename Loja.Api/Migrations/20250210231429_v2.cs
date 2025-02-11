using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Garantia",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Marca",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tamanho",
                table: "Produto",
                type: "NVARCHAR",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Garantia",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Marca",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Tamanho",
                table: "Produto");
        }
    }
}
