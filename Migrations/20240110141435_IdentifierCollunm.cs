using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class IdentifierCollunm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "indentificador",
                table: "relatorio",
                newName: "serial");

            migrationBuilder.AddColumn<string>(
                name: "identificador",
                table: "relatorio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "abreviacao",
                table: "composicao",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "identificador",
                table: "composicao",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_identificador",
                table: "relatorio",
                column: "identificador");

            migrationBuilder.CreateIndex(
                name: "IX_composicao_identificador",
                table: "composicao",
                column: "identificador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_relatorio_identificador",
                table: "relatorio");

            migrationBuilder.DropIndex(
                name: "IX_composicao_identificador",
                table: "composicao");

            migrationBuilder.DropColumn(
                name: "identificador",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "abreviacao",
                table: "composicao");

            migrationBuilder.DropColumn(
                name: "identificador",
                table: "composicao");

            migrationBuilder.RenameColumn(
                name: "serial",
                table: "relatorio",
                newName: "indentificador");
        }
    }
}
