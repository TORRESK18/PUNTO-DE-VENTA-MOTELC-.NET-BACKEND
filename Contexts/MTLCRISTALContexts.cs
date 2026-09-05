using Microsoft.EntityFrameworkCore;
using MTLCRISTALVK18BACK.Models.Habitaciones;
using MTLCRISTALVK18BACK.Models.Menu;
using MTLCRISTALVK18BACK.Models.Reservas;
using MTLCRISTALVK18BACK.Models.Usersadmin;

namespace MTLCRISTALVK18BACK.Contexts
{
    public class MTLCRISTALContexts : DbContext
    {
        public MTLCRISTALContexts(DbContextOptions<MTLCRISTALContexts> options) : base(options) { }

        // Tabla de usuarios administrativos
        public DbSet<Usersadmin> Usersadmin { get; set; } = null!;

        // Tabla principal de habitaciones
        public DbSet<Habitaciones> Habitaciones { get; set; } = null!;

        // Tabla principal de reservas / estancias
        public DbSet<Reservas> Reservas { get; set; } = null!;

        // Tabla de clientes y vehículos relacionados con reservas
        public DbSet<ReservaCliente> ReservaClientes { get; set; } = null!;

        // Tabla de consumos relacionados con reservas
        public DbSet<ReservaConsumo> ReservaConsumos { get; set; } = null!;

        public DbSet<CategoriaMenu> CategoriasMenu { get; set; }
        public DbSet<ProductoMenu> ProductosMenu { get; set; }
        public DbSet<VarianteProductoMenu> VariantesProductoMenu { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación: Habitación → Reservas
            modelBuilder.Entity<Reservas>()
                .HasOne(r => r.Habitacion)
                .WithMany()
                .HasForeignKey(r => r.IdHbtn)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Reserva → Cliente
            modelBuilder.Entity<ReservaCliente>()
                .HasOne(c => c.Reserva)
                .WithOne(r => r.Cliente)
                .HasForeignKey<ReservaCliente>(c => c.IdResv)
                .OnDelete(DeleteBehavior.Cascade);

            // Una reserva solo puede tener un registro de cliente / vehículo
            modelBuilder.Entity<ReservaCliente>()
                .HasIndex(c => c.IdResv)
                .IsUnique();

            // Relación: Reserva → Consumos
            modelBuilder.Entity<ReservaConsumo>()
                .HasOne(c => c.Reserva)
                .WithMany(r => r.Consumos)
                .HasForeignKey(c => c.IdResv)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Reserva → Usuario que registró la estancia
            modelBuilder.Entity<Reservas>()
                .HasOne(r => r.UsuarioRegistro)
                .WithMany()
                .HasForeignKey(r => r.IdUsRegistro)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación opcional: Reserva → Usuario que cerró la estancia
            modelBuilder.Entity<Reservas>()
                .HasOne(r => r.UsuarioCierre)
                .WithMany()
                .HasForeignKey(r => r.IdUsCierre)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Consumo → Usuario que registró el pedido
            modelBuilder.Entity<ReservaConsumo>()
                .HasOne(c => c.UsuarioRegistro)
                .WithMany()
                .HasForeignKey(c => c.IdUsRegistro)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación opcional: Consumo → Usuario que entregó el pedido
            modelBuilder.Entity<ReservaConsumo>()
                .HasOne(c => c.UsuarioEntrega)
                .WithMany()
                .HasForeignKey(c => c.IdUsEntrega)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            modelBuilder.Entity<Reservas>()
                .HasIndex(r => r.IdHbtn);

            modelBuilder.Entity<Reservas>()
                .HasIndex(r => r.Estadoresv);

            modelBuilder.Entity<ReservaConsumo>()
                .HasIndex(c => new { c.IdResv, c.EstadoPedido });

            // Precisión de importes de reserva
            modelBuilder.Entity<Reservas>()
                .Property(r => r.Precioresv)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservas>()
                .Property(r => r.TotalConsumos)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservas>()
                .Property(r => r.TotalReserva)
                .HasPrecision(18, 2);

            // Precisión de cantidades e importes de consumos
            modelBuilder.Entity<ReservaConsumo>()
                .Property(c => c.Cantidad)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReservaConsumo>()
                .Property(c => c.PrecioUnit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReservaConsumo>()
                .Property(c => c.TotalLinea)
                .HasPrecision(18, 2);


            modelBuilder.Entity<ProductoMenu>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VarianteProductoMenu>()
                .HasOne(v => v.Producto)
                .WithMany(p => p.Variantes)
                .HasForeignKey(v => v.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductoMenu>()
                .Property(p => p.PrecioBase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<VarianteProductoMenu>()
                .Property(v => v.Precio)
                .HasPrecision(18, 2);
        }
    }
}