using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class propiedadesacteducativa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiereDifusion",
                table: "ActividadesEducativas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SolicitaFlyer",
                table: "ActividadesEducativas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UrlImagenes",
                table: "ActividadesEducativas",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiereDifusion",
                table: "ActividadesEducativas");

            migrationBuilder.DropColumn(
                name: "SolicitaFlyer",
                table: "ActividadesEducativas");

            migrationBuilder.DropColumn(
                name: "UrlImagenes",
                table: "ActividadesEducativas");
        }
    }
}
