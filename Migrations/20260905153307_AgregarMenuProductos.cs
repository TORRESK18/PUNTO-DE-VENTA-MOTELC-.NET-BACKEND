using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTLCRISTALVK18BACK.Migrations
{
    public partial class AgregarMenuProductos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasMenu",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Icono = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasMenu", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "ProductosMenu",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioBase = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosMenu", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_ProductosMenu_CategoriasMenu_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasMenu",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariantesProductoMenu",
                columns: table => new
                {
                    IdVariante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantesProductoMenu", x => x.IdVariante);
                    table.ForeignKey(
                        name: "FK_VariantesProductoMenu_ProductosMenu_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "ProductosMenu",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductosMenu_CategoriaId",
                table: "ProductosMenu",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantesProductoMenu_ProductoId",
                table: "VariantesProductoMenu",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VariantesProductoMenu");

            migrationBuilder.DropTable(
                name: "ProductosMenu");

            migrationBuilder.DropTable(
                name: "CategoriasMenu");
        }
    }
}
