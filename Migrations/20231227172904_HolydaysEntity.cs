using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class HolydaysEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feriado",
                columns: table => new
                {
                    dia = table.Column<DateOnly>(type: "date", nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: false),
                    tipo_feriado = table.Column<int>(type: "integer", nullable: false),
                    regional = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feriado", x => x.dia);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feriado");
        }
    }
}
