using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class avancesultimos9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActividadesReprogramadas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActividadId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaReprogramacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadesReprogramadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActividadesReprogramadas_ActividadesMuseo_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "ActividadesMuseo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReprogramacionHorarioAnterior",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActividadReprogramadaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReprogramacionHorarioAnterior", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReprogramacionHorarioAnterior_ActividadesReprogramadas_Activ~",
                        column: x => x.ActividadReprogramadaId,
                        principalTable: "ActividadesReprogramadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReprogramacionHorarioNuevo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActividadReprogramadaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReprogramacionHorarioNuevo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReprogramacionHorarioNuevo_ActividadesReprogramadas_Activida~",
                        column: x => x.ActividadReprogramadaId,
                        principalTable: "ActividadesReprogramadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesReprogramadas_ActividadId",
                table: "ActividadesReprogramadas",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_ReprogramacionHorarioAnterior_ActividadReprogramadaId",
                table: "ReprogramacionHorarioAnterior",
                column: "ActividadReprogramadaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReprogramacionHorarioNuevo_ActividadReprogramadaId",
                table: "ReprogramacionHorarioNuevo",
                column: "ActividadReprogramadaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReprogramacionHorarioAnterior");

            migrationBuilder.DropTable(
                name: "ReprogramacionHorarioNuevo");

            migrationBuilder.DropTable(
                name: "ActividadesReprogramadas");
        }
    }
}
