using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoverColunaValor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoTotal",
                table: "CarrinhoItem");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "Carrinho");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecoUnitario",
                table: "PedidoItem",
                type: "MONEY",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "NomeProduto",
                table: "CarrinhoItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "CarrinhoItem",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeProduto",
                table: "CarrinhoItem");

            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "CarrinhoItem");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecoUnitario",
                table: "PedidoItem",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "MONEY");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoTotal",
                table: "CarrinhoItem",
                type: "MONEY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "Carrinho",
                type: "MONEY",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
