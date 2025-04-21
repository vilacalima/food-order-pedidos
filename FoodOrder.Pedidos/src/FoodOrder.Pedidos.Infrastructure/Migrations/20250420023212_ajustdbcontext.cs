using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrder.Pedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ajustdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PedidoStatusId",
                table: "pedidos",
                newName: "PedidoStatus");

            migrationBuilder.RenameColumn(
                name: "PagamentoId",
                table: "pedidos",
                newName: "PagamentoStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PedidoStatus",
                table: "pedidos",
                newName: "PedidoStatusId");

            migrationBuilder.RenameColumn(
                name: "PagamentoStatus",
                table: "pedidos",
                newName: "PagamentoId");
        }
    }
}
