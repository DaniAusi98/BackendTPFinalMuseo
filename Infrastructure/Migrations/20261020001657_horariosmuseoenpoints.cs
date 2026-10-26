using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class horariosmuseoenpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HoraInicio",
                table: "HorariosAperturaMuseo",
                newName: "Hasta");

            migrationBuilder.RenameColumn(
                name: "HoraFin",
                table: "HorariosAperturaMuseo",
                newName: "Desde");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hasta",
                table: "HorariosAperturaMuseo",
                newName: "HoraInicio");

            migrationBuilder.RenameColumn(
                name: "Desde",
                table: "HorariosAperturaMuseo",
                newName: "HoraFin");
        }
    }
}
