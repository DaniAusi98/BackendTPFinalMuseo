using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cambiosgrandes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TurnosVisitasGuiadas");

            migrationBuilder.DropColumn(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitasGrupalesGuiadas");

            migrationBuilder.DropColumn(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.DropColumn(
                name: "DiasApertura",
                table: "CalendariosMuseo");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "CalendariosMuseo");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "CalendariosMuseo");

            migrationBuilder.CreateTable(
                name: "HorariosAperturaMuseo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    CalendarioMuseoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosAperturaMuseo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosAperturaMuseo_CalendariosMuseo_CalendarioMuseoId",
                        column: x => x.CalendarioMuseoId,
                        principalTable: "CalendariosMuseo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HorariosDisponiblesAutoguiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    Desde = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Hasta = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    ConfiguracionHorarioAutoguiadaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosDisponiblesAutoguiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosDisponiblesAutoguiadas_ConfiguracionVisitaAutoguiada~",
                        column: x => x.ConfiguracionHorarioAutoguiadaId,
                        principalTable: "ConfiguracionVisitaAutoguiada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HorariosDisponiblesVisitasGuiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    Desde = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Hasta = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    ConfiguracionVisitasGrupalesGuiadasId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosDisponiblesVisitasGuiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosDisponiblesVisitasGuiadas_ConfiguracionVisitasGrupal~",
                        column: x => x.ConfiguracionVisitasGrupalesGuiadasId,
                        principalTable: "ConfiguracionVisitasGrupalesGuiadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosAperturaMuseo_CalendarioMuseoId",
                table: "HorariosAperturaMuseo",
                column: "CalendarioMuseoId");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosDisponiblesAutoguiadas_ConfiguracionHorarioAutoguiad~",
                table: "HorariosDisponiblesAutoguiadas",
                column: "ConfiguracionHorarioAutoguiadaId");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosDisponiblesVisitasGuiadas_ConfiguracionVisitasGrupal~",
                table: "HorariosDisponiblesVisitasGuiadas",
                column: "ConfiguracionVisitasGrupalesGuiadasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorariosAperturaMuseo");

            migrationBuilder.DropTable(
                name: "HorariosDisponiblesAutoguiadas");

            migrationBuilder.DropTable(
                name: "HorariosDisponiblesVisitasGuiadas");

            migrationBuilder.AddColumn<string>(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitasGrupalesGuiadas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitaAutoguiada",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFin",
                table: "ConfiguracionVisitaAutoguiada",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "ConfiguracionVisitaAutoguiada",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiasApertura",
                table: "CalendariosMuseo",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFin",
                table: "CalendariosMuseo",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "CalendariosMuseo",
                type: "time(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TurnosVisitasGuiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConfiguracionVisitasGrupalesGuiadasId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnosVisitasGuiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurnosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadas_Con~",
                        column: x => x.ConfiguracionVisitasGrupalesGuiadasId,
                        principalTable: "ConfiguracionVisitasGrupalesGuiadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TurnosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadasId",
                table: "TurnosVisitasGuiadas",
                column: "ConfiguracionVisitasGrupalesGuiadasId");
        }
    }
}
