using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao015 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controle_Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataPedido = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValorPedido = table.Column<double>(type: "REAL", nullable: true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    EnderecoEntrega_Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoEntrega_Numero = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoEntrega_Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoEntrega_Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoEntrega_Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoEntrega_Estado = table.Column<string>(type: "char(2)", nullable: true),
                    EnderecoEntrega_CEP = table.Column<string>(type: "char(9)", nullable: true),
                    EnderecoEntrega_Referencia = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Controle_Pedidos_Controle_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Controle_Clientes",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controle_Pedidos_IdCliente",
                table: "Controle_Pedidos",
                column: "IdCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controle_Pedidos");
        }
    }
}
