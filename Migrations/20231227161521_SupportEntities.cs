using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SupportEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tipo_viatura",
                table: "composicao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "contrato",
                columns: table => new
                {
                    contrato = table.Column<string>(type: "text", nullable: false),
                    inicio_vigencia = table.Column<DateOnly>(type: "date", nullable: false),
                    final_vigencia = table.Column<DateOnly>(type: "date", nullable: false),
                    regional = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contrato", x => x.contrato);
                });

            migrationBuilder.CreateTable(
                name: "objetivo",
                columns: table => new
                {
                    regional = table.Column<int>(type: "integer", nullable: false),
                    tipo_viatura = table.Column<int>(type: "integer", nullable: false),
                    atividade = table.Column<int>(type: "integer", nullable: false),
                    contrato = table.Column<string>(type: "text", nullable: true),
                    meta_producao = table.Column<decimal>(type: "money", nullable: false),
                    meta_apresentacao = table.Column<int>(type: "integer", nullable: false),
                    meta_apresentacao_feriado = table.Column<int>(type: "integer", nullable: false),
                    meta_execucoes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objetivo", x => new { x.regional, x.tipo_viatura, x.atividade });
                });

            migrationBuilder.CreateTable(
                name: "valoracao",
                columns: table => new
                {
                    regional = table.Column<int>(type: "integer", nullable: false),
                    tipo_viatura = table.Column<int>(type: "integer", nullable: false),
                    atividade = table.Column<int>(type: "integer", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    contrato = table.Column<string>(type: "text", nullable: true),
                    valor = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valoracao", x => new { x.regional, x.tipo_viatura, x.atividade, x.codigo });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contrato");

            migrationBuilder.DropTable(
                name: "objetivo");

            migrationBuilder.DropTable(
                name: "valoracao");

            migrationBuilder.DropColumn(
                name: "tipo_viatura",
                table: "composicao");
        }
    }
}
