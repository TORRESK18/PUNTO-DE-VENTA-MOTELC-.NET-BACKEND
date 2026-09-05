using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTLCRISTALVK18BACK.Migrations
{
    public partial class AgregarInventarioProductoMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ControlaInventario",
                table: "ProductosMenu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockActual",
                table: "ProductosMenu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockMinimo",
                table: "ProductosMenu",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlaInventario",
                table: "ProductosMenu");

            migrationBuilder.DropColumn(
                name: "StockActual",
                table: "ProductosMenu");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "ProductosMenu");
        }
    }
}
