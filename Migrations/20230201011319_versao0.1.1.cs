using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDAvancado.Migrations
{
    public partial class versao011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco_Bairro",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_CEP",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Complemento",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Estado",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Logradouro",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Numero",
                table: "Controle_Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco_Referencia",
                table: "Controle_Clientes");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "Controle_Clientes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "Controle_Enderecos",
                columns: table => new
                {
                    ClienteModelIdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Estado = table.Column<string>(type: "TEXT", nullable: true),
                    CEP = table.Column<string>(type: "TEXT", nullable: true),
                    Referencia = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controle_Enderecos", x => x.ClienteModelIdUsuario);
                    table.ForeignKey(
                        name: "FK_Controle_Enderecos_Controle_Clientes_ClienteModelIdUsuario",
                        column: x => x.ClienteModelIdUsuario,
                        principalTable: "Controle_Clientes",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controle_Enderecos");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "Controle_Clientes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Bairro",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_CEP",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Complemento",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Estado",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Logradouro",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Numero",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Referencia",
                table: "Controle_Clientes",
                type: "TEXT",
                nullable: true);
        }
    }
}
