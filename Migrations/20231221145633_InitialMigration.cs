using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "composicao",
                columns: table => new
                {
                    dia = table.Column<DateOnly>(type: "date", nullable: false),
                    recurso = table.Column<string>(type: "text", nullable: false),
                    adesivo = table.Column<int>(type: "integer", nullable: true),
                    placa = table.Column<string>(type: "text", nullable: true),
                    atividade = table.Column<int>(type: "integer", nullable: false),
                    motorista = table.Column<string>(type: "text", nullable: true),
                    id_motorista = table.Column<int>(type: "integer", nullable: false),
                    ajudante = table.Column<string>(type: "text", nullable: true),
                    id_ajudante = table.Column<int>(type: "integer", nullable: false),
                    telefone = table.Column<long>(type: "bigint", nullable: false),
                    id_supervisor = table.Column<int>(type: "integer", nullable: false),
                    supervisor = table.Column<string>(type: "text", nullable: true),
                    regional = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_composicao", x => new { x.dia, x.recurso });
                });

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    matricula = table.Column<int>(type: "integer", nullable: false),
                    nome_colaborador = table.Column<string>(type: "text", nullable: false),
                    palavra = table.Column<string>(type: "text", nullable: true),
                    admissao = table.Column<DateOnly>(type: "date", nullable: true),
                    demissao = table.Column<DateOnly>(type: "date", nullable: true),
                    situacao = table.Column<int>(type: "integer", nullable: false),
                    regional = table.Column<int>(type: "integer", nullable: true),
                    atividade = table.Column<int>(type: "integer", nullable: true),
                    id_superior = table.Column<int>(type: "integer", nullable: true),
                    funcao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionario", x => x.matricula);
                });

            migrationBuilder.CreateTable(
                name: "relatorio",
                columns: table => new
                {
                    indentificador = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(type: "text", nullable: false),
                    recurso = table.Column<string>(type: "text", nullable: false),
                    dia = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    nome_do_cliente = table.Column<string>(type: "text", nullable: true),
                    endereco_destino = table.Column<string>(type: "text", nullable: true),
                    cidade_destino = table.Column<string>(type: "text", nullable: true),
                    codigo_postal = table.Column<string>(type: "text", nullable: true),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    hora_final = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    vencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duracao_feito = table.Column<TimeSpan>(type: "interval", nullable: true),
                    desloca_feito = table.Column<TimeSpan>(type: "interval", nullable: true),
                    tipo_atividade = table.Column<string>(type: "text", nullable: true),
                    servico = table.Column<long>(type: "bigint", nullable: true),
                    area_trabalho = table.Column<string>(type: "text", nullable: true),
                    codigos = table.Column<string>(type: "text", nullable: true),
                    id_viatura = table.Column<string>(type: "text", nullable: true),
                    id_motorista = table.Column<int>(type: "integer", nullable: true),
                    id_ajudante = table.Column<int>(type: "integer", nullable: true),
                    id_tecnico = table.Column<int>(type: "integer", nullable: true),
                    observacao = table.Column<string>(type: "text", nullable: true),
                    bairro_destino = table.Column<string>(type: "text", nullable: true),
                    instalacao = table.Column<int>(type: "integer", nullable: true),
                    complemento_destino = table.Column<string>(type: "text", nullable: true),
                    referencia_destino = table.Column<string>(type: "text", nullable: true),
                    tipo_servico = table.Column<string>(type: "text", nullable: true),
                    debitos_cliente = table.Column<double>(type: "double precision", nullable: true),
                    tipo_instalacao = table.Column<int>(type: "integer", nullable: true),
                    desloca_estima = table.Column<TimeSpan>(type: "interval", nullable: true),
                    duracao_estima = table.Column<TimeSpan>(type: "interval", nullable: true),
                    motivo_indisponibilidade = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorio", x => x.indentificador);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "composicao");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "relatorio");
        }
    }
}
