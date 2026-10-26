using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cambiosavances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloqueosSala_Salas_SalaId",
                table: "BloqueosSala");

            migrationBuilder.DropTable(
                name: "Localidades");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Provincias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BloqueosSala",
                table: "BloqueosSala");

            migrationBuilder.RenameTable(
                name: "BloqueosSala",
                newName: "BloqueoSala");

            migrationBuilder.RenameIndex(
                name: "IX_BloqueosSala_SalaId",
                table: "BloqueoSala",
                newName: "IX_BloqueoSala_SalaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloqueoSala",
                table: "BloqueoSala",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PropuestaEvento",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreApellidoResponsable = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoResponsable = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailResponsable = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TituloPropuesta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoEvento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FormularioEventoCompletado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RRule = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormularioEnviado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaEnvioFormulario = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropuestaEvento", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropuestaEventoBloqueosSala",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropuestaEventoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BloqueoSalaId = table.Column<string>(type: "varchar(255)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PropuestaSalas",
                columns: table => new
                {
                    PropuestaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropuestaSalas", x => new { x.PropuestaId, x.SalaId });
                    table.ForeignKey(
                        name: "FK_PropuestaSalas_PropuestaEvento_PropuestaId",
                        column: x => x.PropuestaId,
                        principalTable: "PropuestaEvento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropuestaSalas_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
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

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaSalas_SalaId",
                table: "PropuestaSalas",
                column: "SalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloqueoSala_Salas_SalaId",
                table: "BloqueoSala",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloqueoSala_Salas_SalaId",
                table: "BloqueoSala");

            migrationBuilder.DropTable(
                name: "PropuestaEventoBloqueosSala");

            migrationBuilder.DropTable(
                name: "PropuestaSalas");

            migrationBuilder.DropTable(
                name: "PropuestaEvento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BloqueoSala",
                table: "BloqueoSala");

            migrationBuilder.RenameTable(
                name: "BloqueoSala",
                newName: "BloqueosSala");

            migrationBuilder.RenameIndex(
                name: "IX_BloqueoSala_SalaId",
                table: "BloqueosSala",
                newName: "IX_BloqueosSala_SalaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloqueosSala",
                table: "BloqueosSala",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamentos_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartamentoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localidades_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Localidades_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_ProvinciaId_Nombre",
                table: "Departamentos",
                columns: new[] { "ProvinciaId", "Nombre" });

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_DepartamentoId",
                table: "Localidades",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_ProvinciaId_DepartamentoId_Nombre",
                table: "Localidades",
                columns: new[] { "ProvinciaId", "DepartamentoId", "Nombre" });

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_Nombre",
                table: "Provincias",
                column: "Nombre");

            migrationBuilder.AddForeignKey(
                name: "FK_BloqueosSala_Salas_SalaId",
                table: "BloqueosSala",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
