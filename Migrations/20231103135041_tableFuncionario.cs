using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class tableFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_servico",
                table: "servico");

            migrationBuilder.RenameTable(
                name: "servico",
                newName: "relatorio");

            migrationBuilder.AddPrimaryKey(
                name: "PK_relatorio",
                table: "relatorio",
                column: "indentificador");

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    matricula = table.Column<int>(type: "INTEGER", nullable: false),
                    nome_colaborador = table.Column<string>(type: "TEXT", nullable: false),
                    funcao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionario", x => x.matricula);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_relatorio",
                table: "relatorio");

            migrationBuilder.RenameTable(
                name: "relatorio",
                newName: "servico");

            migrationBuilder.AddPrimaryKey(
                name: "PK_servico",
                table: "servico",
                column: "indentificador");
        }
    }
}
