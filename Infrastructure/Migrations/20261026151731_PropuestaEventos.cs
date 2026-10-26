using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PropuestaEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropuestaEventoBloqueosSala");

            migrationBuilder.CreateTable(
                name: "PropuestaRecursosReservados",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropuestaId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecursoId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadAsignada = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropuestaRecursosReservados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropuestaRecursosReservados_PropuestaEvento_PropuestaId",
                        column: x => x.PropuestaId,
                        principalTable: "PropuestaEvento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropuestaRecursosReservados_Recursos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaRecursosReservados_PropuestaId_RecursoId",
                table: "PropuestaRecursosReservados",
                columns: new[] { "PropuestaId", "RecursoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaRecursosReservados_RecursoId",
                table: "PropuestaRecursosReservados",
                column: "RecursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropuestaRecursosReservados");

            migrationBuilder.CreateTable(
                name: "PropuestaEventoBloqueosSala",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BloqueoSalaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropuestaEventoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropuestaEventoBloqueosSala", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropuestaEventoBloqueosSala_BloqueoSala_BloqueoSalaId",
                        column: x => x.BloqueoSalaId,
                        principalTable: "BloqueoSala",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropuestaEventoBloqueosSala_PropuestaEvento_PropuestaEventoId",
                        column: x => x.PropuestaEventoId,
                        principalTable: "PropuestaEvento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaEventoBloqueosSala_BloqueoSalaId",
                table: "PropuestaEventoBloqueosSala",
                column: "BloqueoSalaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaEventoBloqueosSala_PropuestaEventoId",
                table: "PropuestaEventoBloqueosSala",
                column: "PropuestaEventoId");
        }
    }
}
