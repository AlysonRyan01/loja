using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBairro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Enderecos",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Enderecos");
        }
    }
}
