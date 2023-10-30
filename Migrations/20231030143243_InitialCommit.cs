using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "composicao",
                columns: table => new
                {
                    dia = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    recurso = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    adesivo = table.Column<int>(type: "INTEGER", nullable: false),
                    placa = table.Column<string>(type: "TEXT", nullable: false),
                    atividade = table.Column<int>(type: "INTEGER", nullable: false),
                    motorista = table.Column<string>(type: "TEXT", nullable: false),
                    id_motorista = table.Column<int>(type: "INTEGER", nullable: false),
                    ajudante = table.Column<string>(type: "TEXT", nullable: false),
                    id_ajudante = table.Column<int>(type: "INTEGER", nullable: false),
                    telefone = table.Column<long>(type: "INTEGER", nullable: false),
                    id_supervisor = table.Column<int>(type: "INTEGER", nullable: false),
                    supervisor = table.Column<string>(type: "TEXT", nullable: false),
                    regional = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_composicao", x => new { x.recurso, x.dia });
                });

            migrationBuilder.CreateTable(
                name: "servico",
                columns: table => new
                {
                    indentificador = table.Column<int>(type: "INTEGER", nullable: false),
                    recurso = table.Column<string>(type: "TEXT", nullable: false),
                    dia = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    hora_final = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    duracao_feito = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    desloca_feito = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    tipo_atividade = table.Column<string>(type: "TEXT", nullable: true),
                    servico = table.Column<long>(type: "INTEGER", nullable: false),
                    codigos = table.Column<string>(type: "TEXT", nullable: true),
                    observacao = table.Column<string>(type: "TEXT", nullable: true),
                    instalacao = table.Column<int>(type: "INTEGER", nullable: true),
                    tipo_servico = table.Column<string>(type: "TEXT", nullable: false),
                    Desloca_estima = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    duracao_estima = table.Column<TimeOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servico", x => x.indentificador);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "composicao");

            migrationBuilder.DropTable(
                name: "servico");
        }
    }
}
