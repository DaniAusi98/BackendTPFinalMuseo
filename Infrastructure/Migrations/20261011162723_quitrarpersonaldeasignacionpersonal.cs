using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class quitrarpersonaldeasignacionpersonal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesPersonal_Personal_PersonalId",
                table: "AsignacionesPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesPersonal_Personal_PersonalId",
                table: "AsignacionesPersonal",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesPersonal_Personal_PersonalId",
                table: "AsignacionesPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesPersonal_Personal_PersonalId",
                table: "AsignacionesPersonal",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
