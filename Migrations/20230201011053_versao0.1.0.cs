using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controle_Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Categorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Controle_Clientes",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    CPF = table.Column<string>(type: "char(14)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Endereco_Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_Estado = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_CEP = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco_Referencia = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Clientes", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Controle_Produtos",
                columns: table => new
                {
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Valor = table.Column<double>(type: "REAL", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdCategoria = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Produtos", x => x.IdProduto);
                    table.ForeignKey(
                        name: "FK_Controle_Produtos_Controle_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Controle_Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controle_Produtos_IdCategoria",
                table: "Controle_Produtos",
                column: "IdCategoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controle_Clientes");

            migrationBuilder.DropTable(
                name: "Controle_Produtos");

            migrationBuilder.DropTable(
                name: "Controle_Categorias");
        }
    }
}
