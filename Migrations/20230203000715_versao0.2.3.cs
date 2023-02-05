using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Usuarios",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'localtime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'locatime', 'start of day')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Produtos",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'localtime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'locatime', 'start of day')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Categorias",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'localtime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'locatime', 'start of day')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Usuarios",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'locatime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'localtime', 'start of day')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Produtos",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'locatime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'localtime', 'start of day')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Controle_Categorias",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now', 'locatime', 'start of day')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now', 'localtime', 'start of day')");
        }
    }
}
