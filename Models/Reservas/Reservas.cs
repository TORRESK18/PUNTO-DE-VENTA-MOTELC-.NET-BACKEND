using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HabitacionModel = MTLCRISTALVK18BACK.Models.Habitaciones.Habitaciones;
using UsuarioModel = MTLCRISTALVK18BACK.Models.Usersadmin.Usersadmin;

namespace MTLCRISTALVK18BACK.Models.Reservas
{
    public class Reservas
    {
        [Key]
        public int IdResv { get; set; }

        // Identificación de la reserva
        public int Numresv { get; set; }

        [MaxLength(50)]
        public string? Folioordenresv { get; set; }

        // Habitación relacionada
        [Required]
        public int IdHbtn { get; set; }

        [ForeignKey(nameof(IdHbtn))]
        public HabitacionModel Habitacion { get; set; } = null!;

        // Estado y tipo de reserva
        [Required]
        [MaxLength(30)]
        public string Estadoresv { get; set; } = "ACTIVA";

        [MaxLength(50)]
        public string? Tiporesv { get; set; }

        // Fechas de la estancia
        [Required]
        public DateTimeOffset FechaHoraEntrada { get; set; }

        [Required]
        public DateTimeOffset FechaHoraSalidaProgramada { get; set; }

        public DateTimeOffset? FechaHoraSalidaReal { get; set; }

        // Tiempo de renta guardado históricamente
        public int TiempoRentaMinutos { get; set; }

        // Importes
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precioresv { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalConsumos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReserva { get; set; }

        // Estado de pago
        [MaxLength(30)]
        public string Statuspagohabresv { get; set; } = "PENDIENTE";

        // Usuario que registró la estancia
        [Required]
        public int IdUsRegistro { get; set; }

        [ForeignKey(nameof(IdUsRegistro))]
        public UsuarioModel UsuarioRegistro { get; set; } = null!;

        // Usuario que cerró la estancia
        public int? IdUsCierre { get; set; }

        [ForeignKey(nameof(IdUsCierre))]
        public UsuarioModel UsuarioCierre { get; set; } = null!;

        // Turno
        public int TurnoUserMTL { get; set; }

        // Cliente / vehículo
        public ReservaCliente? Cliente { get; set; }

        // Consumos de la estancia
        public ICollection<ReservaConsumo> Consumos { get; set; } = new List<ReservaConsumo>();
    }

    public class ReservaCliente
    {
        [Key]
        public int IdClte { get; set; }

        // Reserva relacionada
        [Required]
        public int IdResv { get; set; }

        [ForeignKey(nameof(IdResv))]
        public Reservas Reserva { get; set; } = null!;

        // Datos del cliente / vehículo
        [MaxLength(30)]
        public string StatusingresoCl { get; set; } = "INGRESADO";

        [MaxLength(150)]
        public string? NombreCliente { get; set; }

        [MaxLength(20)]
        public string? AutPlacasCl { get; set; }

        [MaxLength(80)]
        public string? AutMarcaCl { get; set; }

        [MaxLength(80)]
        public string? AutModeloCl { get; set; }

        [MaxLength(50)]
        public string? AutColorCl { get; set; }

        public int FrecuenciaCl { get; set; }

        [MaxLength(300)]
        public string? AdvertCl { get; set; }
    }

    public class ReservaConsumo
    {
        [Key]
        public int IdCsms { get; set; }

        // Reserva relacionada
        [Required]
        public int IdResv { get; set; }

        [ForeignKey(nameof(IdResv))]
        public Reservas Reserva { get; set; } = null!;

        // Producto relacionado, opcional
        public int? ProductoId { get; set; }

        // Datos históricos del consumo
        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        // Estado del pedido
        [MaxLength(30)]
        public string EstadoPedido { get; set; } = "PENDIENTE";

        // Estado de pago
        [MaxLength(30)]
        public string StatusPagado { get; set; } = "PENDIENTE";

        // Fechas del pedido
        public DateTimeOffset FechaSolicitud { get; set; }

        public DateTimeOffset? FechaEntrega { get; set; }

        // Usuario que registró el consumo
        [Required]
        public int IdUsRegistro { get; set; }

        [ForeignKey(nameof(IdUsRegistro))]
        public UsuarioModel UsuarioRegistro { get; set; } = null!;

        // Usuario que entregó el consumo
        public int? IdUsEntrega { get; set; }

        [ForeignKey(nameof(IdUsEntrega))]
        public UsuarioModel UsuarioEntrega { get; set; } = null!;
    }
}