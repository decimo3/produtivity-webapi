using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_composicao",
                table: "composicao");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_servico",
                table: "relatorio",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<long>(
                name: "servico",
                table: "relatorio",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "area_trabalho",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bairro_destino",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cidade_destino",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_postal",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "complemento_destino",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "debitos_cliente",
                table: "relatorio",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_destino",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filename",
                table: "relatorio",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "id_ajudante",
                table: "relatorio",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_motorista",
                table: "relatorio",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_tecnico",
                table: "relatorio",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_viatura",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "motivo_indisponibilidade",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nome_do_cliente",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "referencia_destino",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tipo_instalacao",
                table: "relatorio",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "vencimento",
                table: "relatorio",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_composicao",
                table: "composicao",
                columns: new[] { "dia", "recurso" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_composicao",
                table: "composicao");

            migrationBuilder.DropColumn(
                name: "area_trabalho",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "bairro_destino",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "cidade_destino",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "codigo_postal",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "complemento_destino",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "debitos_cliente",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "endereco_destino",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "filename",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "id_ajudante",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "id_motorista",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "id_tecnico",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "id_viatura",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "motivo_indisponibilidade",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "nome_do_cliente",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "referencia_destino",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "tipo_instalacao",
                table: "relatorio");

            migrationBuilder.DropColumn(
                name: "vencimento",
                table: "relatorio");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_servico",
                table: "relatorio",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "servico",
                table: "relatorio",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_composicao",
                table: "composicao",
                columns: new[] { "recurso", "dia" });
        }
    }
}
