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
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Produto",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Produto",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "CHAR", maxLength: 8, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Gateway = table.Column<int>(type: "SMALLINT", nullable: false),
                    ExternalReference = table.Column<string>(type: "VARCHAR", maxLength: 60, nullable: true),
                    Status = table.Column<int>(type: "SMALLINT", nullable: false),
                    UserId = table.Column<string>(type: "VARCHAR", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PedidoId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<long>(type: "INTEGER", nullable: false),
                    NomeProduto = table.Column<string>(type: "NVARCHAR", maxLength: 200, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_PedidoId",
                table: "PedidoItem",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Produto");
        }
    }
}
