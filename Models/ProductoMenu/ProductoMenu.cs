using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTLCRISTALVK18BACK.Models.Menu
{
    public class ProductoMenu
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public CategoriaMenu Categoria { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioBase { get; set; }

        [MaxLength(500)]
        public string? Imagen { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; } = true;

        public bool ControlaInventario { get; set; } = true;

        public int StockActual { get; set; } = 0;

        public int StockMinimo { get; set; } = 0;

        public ICollection<VarianteProductoMenu> Variantes { get; set; }
            = new List<VarianteProductoMenu>();
    }
}
