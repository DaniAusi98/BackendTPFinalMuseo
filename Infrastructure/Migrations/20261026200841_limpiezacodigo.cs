using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class limpiezacodigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalidadesMundial");

            migrationBuilder.DropTable(
                name: "PropuestaRecursosReservados");

            migrationBuilder.DropTable(
                name: "PropuestaSalas");

            migrationBuilder.DropTable(
                name: "DivisionesAdministrativas");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropColumn(
                name: "EmailResponsable",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "FechaHasta",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "NombreApellidoResponsable",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "RRule",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "TelefonoResponsable",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "TipoEvento",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "TituloPropuesta",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "MuestrasTemporales");

            migrationBuilder.DropColumn(
                name: "FundamentacionObjetivoMuestra",
                table: "MuestrasTemporales");

            migrationBuilder.RenameColumn(
                name: "NombreyApellidoSolicitante",
                table: "Eventos",
                newName: "NombreyApellidoResponsable");

            migrationBuilder.AddColumn<string>(
                name: "EventoId",
                table: "PropuestaEvento",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Publico",
                table: "MuestrasTemporales",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionMuestra",
                table: "MuestrasTemporales",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FundamentacionMuestra",
                table: "MuestrasTemporales",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TipoPublico",
                table: "Eventos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TipoEvento",
                table: "Eventos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Institucion",
                table: "Eventos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropuestaMuestra",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MuestraTemporalId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormularioCompletado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormularioEnviado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaEnvioFormulario = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropuestaMuestra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropuestaMuestra_MuestrasTemporales_MuestraTemporalId",
                        column: x => x.MuestraTemporalId,
                        principalTable: "MuestrasTemporales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaEvento_EventoId",
                table: "PropuestaEvento",
                column: "EventoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaMuestra_MuestraTemporalId",
                table: "PropuestaMuestra",
                column: "MuestraTemporalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropuestaEvento_Eventos_EventoId",
                table: "PropuestaEvento",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropuestaEvento_Eventos_EventoId",
                table: "PropuestaEvento");

            migrationBuilder.DropTable(
                name: "PropuestaMuestra");

            migrationBuilder.DropIndex(
                name: "IX_PropuestaEvento_EventoId",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "EventoId",
                table: "PropuestaEvento");

            migrationBuilder.DropColumn(
                name: "DescripcionMuestra",
                table: "MuestrasTemporales");

            migrationBuilder.DropColumn(
                name: "FundamentacionMuestra",
                table: "MuestrasTemporales");

            migrationBuilder.RenameColumn(
                name: "NombreyApellidoResponsable",
                table: "Eventos",
                newName: "NombreyApellidoSolicitante");

            migrationBuilder.AddColumn<string>(
                name: "EmailResponsable",
                table: "PropuestaEvento",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "PropuestaEvento",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHasta",
                table: "PropuestaEvento",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NombreApellidoResponsable",
                table: "PropuestaEvento",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RRule",
                table: "PropuestaEvento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TelefonoResponsable",
                table: "PropuestaEvento",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TipoEvento",
                table: "PropuestaEvento",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TituloPropuesta",
                table: "PropuestaEvento",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "MuestrasTemporales",
                keyColumn: "Publico",
                keyValue: null,
                column: "Publico",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Publico",
                table: "MuestrasTemporales",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "MuestrasTemporales",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FundamentacionObjetivoMuestra",
                table: "MuestrasTemporales",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "TipoPublico",
                keyValue: null,
                column: "TipoPublico",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TipoPublico",
                table: "Eventos",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TipoEvento",
                table: "Eventos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Institucion",
                keyValue: null,
                column: "Institucion",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Institucion",
                table: "Eventos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    Fin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DivisionesAdministrativas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PadreId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaisId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionesAdministrativas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionesAdministrativas_DivisionesAdministrativas_PadreId",
                        column: x => x.PadreId,
                        principalTable: "DivisionesAdministrativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DivisionesAdministrativas_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LocalidadesMundial",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DivisionAdministrativaId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoPostal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitud = table.Column<double>(type: "double", nullable: true),
                    Longitud = table.Column<double>(type: "double", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalidadesMundial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalidadesMundial_DivisionesAdministrativas_DivisionAdminis~",
                        column: x => x.DivisionAdministrativaId,
                        principalTable: "DivisionesAdministrativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionesAdministrativas_PadreId",
                table: "DivisionesAdministrativas",
                column: "PadreId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionesAdministrativas_PaisId",
                table: "DivisionesAdministrativas",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalidadesMundial_DivisionAdministrativaId",
                table: "LocalidadesMundial",
                column: "DivisionAdministrativaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaRecursosReservados_PropuestaId_RecursoId",
                table: "PropuestaRecursosReservados",
                columns: new[] { "PropuestaId", "RecursoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaRecursosReservados_RecursoId",
                table: "PropuestaRecursosReservados",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropuestaSalas_SalaId",
                table: "PropuestaSalas",
                column: "SalaId");
        }
    }
}
