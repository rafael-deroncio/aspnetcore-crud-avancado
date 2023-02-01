using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "DataUltimaAtualizacao",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Controle_Clientes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Produtos",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'locatime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Controle_Enderecos",
                type: "char(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Controle_Enderecos",
                type: "char(9)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Selecionado",
                table: "Controle_Enderecos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Categorias",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'locatime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Controle_Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now', 'locatime', 'start of day')"),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Controle_Clientes_Controle_Usuarios_IdUsuario",
                table: "Controle_Clientes",
                column: "IdUsuario",
                principalTable: "Controle_Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controle_Clientes_Controle_Usuarios_IdUsuario",
                table: "Controle_Clientes");

            migrationBuilder.DropTable(
                name: "Controle_Usuarios");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Controle_Enderecos");

            migrationBuilder.DropColumn(
                name: "Selecionado",
                table: "Controle_Enderecos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Produtos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'locatime', 'start of day')");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(9)");

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "Controle_Enderecos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimaAtualizacao",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Controle_Clientes",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Controle_Clientes",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Controle_Clientes",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Categorias",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'locatime', 'start of day')");
        }
    }
}
