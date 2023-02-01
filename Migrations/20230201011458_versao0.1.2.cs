using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Controle_Enderecos",
                table: "Controle_Enderecos");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Controle_Enderecos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Controle_Enderecos",
                table: "Controle_Enderecos",
                columns: new[] { "ClienteModelIdUsuario", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Controle_Enderecos",
                table: "Controle_Enderecos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Controle_Enderecos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Controle_Enderecos",
                table: "Controle_Enderecos",
                column: "ClienteModelIdUsuario");
        }
    }
}
