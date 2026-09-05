using System.ComponentModel.DataAnnotations;

namespace MTLCRISTALVK18BACK.Models.Habitaciones
{
    public class Habitaciones
    {
        [Key]
        public int IdHbtn { get; set; }

        [Required]
        public int Numhab { get; set; }
        public string? Estadohab { get; set; }
        public string? Tipohab { get; set; }
        public string? Tiemporenthab { get; set; }
        public string? Diasemofinhab { get; set; }
        public string? Preciohab { get; set; }
        public string? Statushab { get; set; }
        public string? Limpiezahab { get; set; }
        public string? Albercahab { get; set; }
        public string? Jacuzzihab { get; set; }
        public string? TipoCamahab { get; set; }
        public string? Folioordenhab { get; set; }
        public string? AcargoUserMTL { get; set; }
        public int? TurnoUserMTL { get; set; }

    }
}
