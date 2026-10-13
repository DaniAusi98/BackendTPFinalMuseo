using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reprogramaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitasReprogramadas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VisitaReprogramadaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NuevaVisitaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HorarioAnteriorInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    HorarioAnteriorFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    Institucion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelEducativo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnioGrado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaisInstitucion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaInstitucion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocalidadInstitucion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadPersonas = table.Column<int>(type: "int", nullable: true),
                    NuevoHorarioInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    NuevoHorarioFin = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasReprogramadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitasReprogramadas_ActividadesMuseo_NuevaVisitaId",
                        column: x => x.NuevaVisitaId,
                        principalTable: "ActividadesMuseo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitasReprogramadas_ActividadesMuseo_VisitaReprogramadaId",
                        column: x => x.VisitaReprogramadaId,
                        principalTable: "ActividadesMuseo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasReprogramadas_NuevaVisitaId",
                table: "VisitasReprogramadas",
                column: "NuevaVisitaId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasReprogramadas_VisitaReprogramadaId",
                table: "VisitasReprogramadas",
                column: "VisitaReprogramadaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitasReprogramadas");
        }
    }
}
