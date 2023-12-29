using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class calculateStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "relatorioEstatisticas",
                columns: table => new
                {
                    filename = table.Column<string>(type: "text", nullable: false),
                    dia = table.Column<DateOnly>(type: "date", nullable: false),
                    recursos = table.Column<int>(type: "integer", nullable: false),
                    servicos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorioEstatisticas", x => x.filename);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "relatorioEstatisticas");
        }
    }
}
