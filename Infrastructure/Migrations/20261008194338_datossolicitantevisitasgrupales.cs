using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class datossolicitantevisitasgrupales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidoSolicitante",
                table: "VisitasGrupalesGuiadas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EmailSolicitante",
                table: "VisitasGrupalesGuiadas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NombreSolicitante",
                table: "VisitasGrupalesGuiadas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TelefonoSolicitante",
                table: "VisitasGrupalesGuiadas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidoSolicitante",
                table: "VisitasGrupalesGuiadas");

            migrationBuilder.DropColumn(
                name: "EmailSolicitante",
                table: "VisitasGrupalesGuiadas");

            migrationBuilder.DropColumn(
                name: "NombreSolicitante",
                table: "VisitasGrupalesGuiadas");

            migrationBuilder.DropColumn(
                name: "TelefonoSolicitante",
                table: "VisitasGrupalesGuiadas");
        }
    }
}
