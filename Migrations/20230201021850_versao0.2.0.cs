using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controle_Itens_Pedido",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "INTEGER", nullable: false),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Itens_Pedido", x => new { x.IdPedido, x.IdProduto });
                    table.ForeignKey(
                        name: "FK_Controle_Itens_Pedido_Controle_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Controle_Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Controle_Itens_Pedido_Controle_Produtos_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Controle_Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controle_Itens_Pedido_IdProduto",
                table: "Controle_Itens_Pedido",
                column: "IdProduto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controle_Itens_Pedido");
        }
    }
}
