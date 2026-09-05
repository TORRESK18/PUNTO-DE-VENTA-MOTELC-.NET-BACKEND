using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTLCRISTALVK18BACK.Models.Menu
{
    public class VarianteProductoMenu
    {
        [Key]
        public int IdVariante { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey(nameof(ProductoId))]
        public ProductoMenu Producto { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; } = true;
    }
}