using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controle_Enderecos_Controle_Clientes_ClienteModelIdUsuario",
                table: "Controle_Enderecos");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Controle_Enderecos",
                newName: "IdEndereco");

            migrationBuilder.RenameColumn(
                name: "ClienteModelIdUsuario",
                table: "Controle_Enderecos",
                newName: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Controle_Enderecos_Controle_Clientes_IdUsuario",
                table: "Controle_Enderecos",
                column: "IdUsuario",
                principalTable: "Controle_Clientes",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controle_Enderecos_Controle_Clientes_IdUsuario",
                table: "Controle_Enderecos");

            migrationBuilder.RenameColumn(
                name: "IdEndereco",
                table: "Controle_Enderecos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Controle_Enderecos",
                newName: "ClienteModelIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Controle_Enderecos_Controle_Clientes_ClienteModelIdUsuario",
                table: "Controle_Enderecos",
                column: "ClienteModelIdUsuario",
                principalTable: "Controle_Clientes",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
