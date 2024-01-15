using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class alteracaoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_composicao_identificador",
                table: "composicao");

            migrationBuilder.AddColumn<string>(
                name: "abreviacao",
                table: "relatorio",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "alteracao",
                columns: table => new
                {
                    identificador = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    responsavel = table.Column<int>(type: "integer", nullable: false),
                    tabela = table.Column<string>(type: "text", nullable: false),
                    verbo = table.Column<string>(type: "text", nullable: false),
                    valorAnterior = table.Column<string>(type: "text", nullable: true),
                    valorPosterior = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alteracao", x => x.identificador);
                });

            migrationBuilder.CreateIndex(
                name: "IX_composicao_identificador",
                table: "composicao",
                column: "identificador",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alteracao");

            migrationBuilder.DropIndex(
                name: "IX_composicao_identificador",
                table: "composicao");

            migrationBuilder.DropColumn(
                name: "abreviacao",
                table: "relatorio");

            migrationBuilder.CreateIndex(
                name: "IX_composicao_identificador",
                table: "composicao",
                column: "identificador");
        }
    }
}
