using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pryAutoGestionFinanzasBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddCreadoPorToMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "Movimientos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Movimientos");
        }
    }
}
