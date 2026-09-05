using System.ComponentModel.DataAnnotations;

namespace MTLCRISTALVK18BACK.Models.Menu
{
    public class CategoriaMenu
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Descripcion { get; set; }

        [MaxLength(100)]
        public string? Icono { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<ProductoMenu> Productos { get; set; }
            = new List<ProductoMenu>();
    }
}