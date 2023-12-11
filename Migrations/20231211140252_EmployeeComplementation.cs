using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeComplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "admissao",
                table: "funcionario",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "atividade",
                table: "funcionario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "demissao",
                table: "funcionario",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "id_superior",
                table: "funcionario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "palavra",
                table: "funcionario",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "regional",
                table: "funcionario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "situacao",
                table: "funcionario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admissao",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "atividade",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "demissao",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "id_superior",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "palavra",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "regional",
                table: "funcionario");

            migrationBuilder.DropColumn(
                name: "situacao",
                table: "funcionario");
        }
    }
}
