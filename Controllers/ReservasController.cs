using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTLCRISTALVK18BACK.Contexts;
using MTLCRISTALVK18BACK.Models.Reservas;
using System.Globalization;

namespace MTLCRISTALVK18BACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly MTLCRISTALContexts _context;

        public ReservasController(MTLCRISTALContexts context)
        {
            _context = context;
        }

        // Obtener todas las reservas
        [HttpGet]
        public async Task<IActionResult> GetReservas()
        {
            var reservas = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Habitacion)
                .Include(r => r.UsuarioRegistro)
                .Include(r => r.UsuarioCierre)
                .Include(r => r.Cliente)
                .Include(r => r.Consumos)
                .OrderByDescending(r => r.FechaHoraEntrada)
                .Select(r => new
                {
                    r.IdResv,
                    r.Numresv,
                    r.Folioordenresv,
                    r.IdHbtn,
                    Numhab = r.Habitacion.Numhab,
                    r.Estadoresv,
                    r.Tiporesv,
                    r.FechaHoraEntrada,
                    r.FechaHoraSalidaProgramada,
                    r.FechaHoraSalidaReal,
                    r.TiempoRentaMinutos,
                    r.Precioresv,
                    r.TotalConsumos,
                    r.TotalReserva,
                    r.Statuspagohabresv,
                    r.IdUsRegistro,
                    UsuarioRegistro = r.UsuarioRegistro.NombreUs,
                    r.IdUsCierre,
                    UsuarioCierre = r.UsuarioCierre != null ? r.UsuarioCierre.NombreUs : null,
                    r.TurnoUserMTL,
                    Cliente = r.Cliente,
                    Consumos = r.Consumos
                })
                .ToListAsync();

            return Ok(reservas);
        }

        // Obtener una reserva
        [HttpGet("{idResv:int}")]
        public async Task<IActionResult> GetReserva(int idResv)
        {
            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Habitacion)
                .Include(r => r.UsuarioRegistro)
                .Include(r => r.UsuarioCierre)
                .Include(r => r.Cliente)
                .Include(r => r.Consumos)
                    .ThenInclude(c => c.UsuarioRegistro)
                .Include(r => r.Consumos)
                    .ThenInclude(c => c.UsuarioEntrega)
                .FirstOrDefaultAsync(r => r.IdResv == idResv);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            return Ok(new
            {
                reserva.IdResv,
                reserva.Numresv,
                reserva.Folioordenresv,
                reserva.IdHbtn,
                Numhab = reserva.Habitacion.Numhab,
                reserva.Estadoresv,
                reserva.Tiporesv,
                reserva.FechaHoraEntrada,
                reserva.FechaHoraSalidaProgramada,
                reserva.FechaHoraSalidaReal,
                reserva.TiempoRentaMinutos,
                reserva.Precioresv,
                reserva.TotalConsumos,
                reserva.TotalReserva,
                reserva.Statuspagohabresv,
                reserva.IdUsRegistro,
                UsuarioRegistro = reserva.UsuarioRegistro.NombreUs,
                reserva.IdUsCierre,
                UsuarioCierre = reserva.UsuarioCierre?.NombreUs,
                reserva.TurnoUserMTL,
                reserva.Cliente,
                Consumos = reserva.Consumos.Select(c => new
                {
                    c.IdCsms,
                    c.ProductoId,
                    c.Descripcion,
                    c.Cantidad,
                    c.PrecioUnit,
                    c.TotalLinea,
                    c.EstadoPedido,
                    c.StatusPagado,
                    c.FechaSolicitud,
                    c.FechaEntrega,
                    c.IdUsRegistro,
                    UsuarioRegistro = c.UsuarioRegistro.NombreUs,
                    c.IdUsEntrega,
                    UsuarioEntrega = c.UsuarioEntrega != null ? c.UsuarioEntrega.NombreUs : null
                })
            });
        }

        [HttpGet("activa/habitacion/{num}")]
        public async Task<IActionResult> GetReservaActivaHabitacion(int num)
        {
            var reserva = await _context.Reservas
                .AsNoTracking()
                .Where(r =>
                    r.Habitacion.Numhab == num &&
                    r.Estadoresv == "ACTIVA")
                .Select(r => new
                {
                    idResv = r.IdResv,
                    numresv = r.Numresv,
                    folioordenresv = r.Folioordenresv,

                    idHbtn = r.IdHbtn,
                    numhab = r.Habitacion.Numhab,

                    estadoresv = r.Estadoresv,
                    tiporesv = r.Tiporesv,

                    fechaHoraEntrada = r.FechaHoraEntrada,
                    fechaHoraSalidaProgramada = r.FechaHoraSalidaProgramada,
                    fechaHoraSalidaReal = r.FechaHoraSalidaReal,

                    tiempoRentaMinutos = r.TiempoRentaMinutos,

                    precioresv = r.Precioresv,
                    totalConsumos = r.TotalConsumos,
                    totalReserva = r.TotalReserva,

                    statuspagohabresv = r.Statuspagohabresv,

                    idUsRegistro = r.IdUsRegistro,

                    usuarioRegistro =
                        r.UsuarioRegistro.NombreUs ??
                        r.UsuarioRegistro.UsernameUs,

                    turnoUserMTL = r.TurnoUserMTL,

                    cliente = r.Cliente == null
                        ? null
                        : new
                        {
                            idClte = r.Cliente.IdClte,
                            idResv = r.Cliente.IdResv,

                            statusingresoCl =
                                r.Cliente.StatusingresoCl,

                            nombreCliente =
                                r.Cliente.NombreCliente,

                            autPlacasCl =
                                r.Cliente.AutPlacasCl,

                            autMarcaCl =
                                r.Cliente.AutMarcaCl,

                            autModeloCl =
                                r.Cliente.AutModeloCl,

                            autColorCl =
                                r.Cliente.AutColorCl,

                            frecuenciaCl =
                                r.Cliente.FrecuenciaCl,

                            advertCl =
                                r.Cliente.AdvertCl
                        },

                    consumos = r.Consumos
                        .OrderBy(c => c.FechaSolicitud)
                        .Select(c => new
                        {
                            idCsms = c.IdCsms,
                            productoId = c.ProductoId,

                            descripcion =
                                c.Descripcion,

                            cantidad =
                                c.Cantidad,

                            precioUnit =
                                c.PrecioUnit,

                            totalLinea =
                                c.TotalLinea,

                            estadoPedido =
                                c.EstadoPedido,

                            statusPagado =
                                c.StatusPagado,

                            fechaSolicitud =
                                c.FechaSolicitud,

                            fechaEntrega =
                                c.FechaEntrega,

                            idUsRegistro =
                                c.IdUsRegistro,

                            idUsEntrega =
                                c.IdUsEntrega
                        })
                        .ToList(),

                    tienePedidoPendiente =
                        r.Consumos.Any(c =>
                            c.EstadoPedido == "PENDIENTE"),

                    estadoOperativo =
                        r.Habitacion.Statushab ??
                        r.Habitacion.Estadohab ??
                        "OCUPADA",

                    estadoVisual =
                        r.Consumos.Any(c =>
                            c.EstadoPedido == "PENDIENTE")
                            ? "PEDIDO_PENDIENTE"
                            : (
                                r.Habitacion.Statushab ??
                                r.Habitacion.Estadohab ??
                                "OCUPADA"
                            )
                })
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                return NotFound(new
                {
                    mensaje =
                        $"No existe una estancia activa para la habitación {num}."
                });
            }

            return Ok(reserva);
        }

        // Obtener reservas activas
        [HttpGet("activas")]
        public async Task<IActionResult> GetReservasActivas()
        {
            var reservas = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Habitacion)
                .Include(r => r.Cliente)
                .Where(r => r.Estadoresv == "ACTIVA")
                .OrderBy(r => r.Habitacion.Numhab)
                .Select(r => new
                {
                    r.IdResv,
                    r.Numresv,
                    r.Folioordenresv,
                    r.IdHbtn,
                    Numhab = r.Habitacion.Numhab,
                    r.Estadoresv,
                    r.FechaHoraEntrada,
                    r.FechaHoraSalidaProgramada,
                    r.Precioresv,
                    r.TotalConsumos,
                    r.TotalReserva,
                    r.Statuspagohabresv,
                    r.Cliente,
                    TienePedidoPendiente = r.Consumos.Any(c => c.EstadoPedido == "PENDIENTE"),
                    EstadoVisual = r.Consumos.Any(c => c.EstadoPedido == "PENDIENTE") ? "PEDIDO_PENDIENTE" : "OCUPADA"
                })
                .ToListAsync();

            return Ok(reservas);
        }



        // Iniciar una nueva estancia
        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarReserva([FromBody] CrearReservaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usersadmin.FindAsync(request.IdUsRegistro);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            if (!PuedeRegistrarReserva(usuario.DepartamentoUs, usuario.PuestoUs))
                return StatusCode(403, new { mensaje = "El usuario no tiene permisos para iniciar estancias." });

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Numhab == request.Numhab);

            if (habitacion == null)
                return NotFound(new { mensaje = $"La habitación {request.Numhab} no existe." });

            string estadoHabitacion = (habitacion.Statushab ?? habitacion.Estadohab ?? "").Trim().ToUpper();

            if (estadoHabitacion != "DISPONIBLE")
                return BadRequest(new { mensaje = $"La habitación {request.Numhab} no está disponible.", estadoActual = estadoHabitacion });

            bool existeActiva = await _context.Reservas
                .AnyAsync(r => r.IdHbtn == habitacion.IdHbtn && r.Estadoresv == "ACTIVA");

            if (existeActiva)
                return Conflict(new { mensaje = $"La habitación {request.Numhab} ya tiene una estancia activa." });

            int minutos = request.TiempoRentaMinutos.HasValue && request.TiempoRentaMinutos.Value > 0
                ? request.TiempoRentaMinutos.Value
                : ObtenerMinutosRenta(habitacion.Tiemporenthab);

            decimal precio = request.Precio.HasValue && request.Precio.Value >= 0
                ? request.Precio.Value
                : ObtenerPrecio(habitacion.Preciohab);

            var ahora = DateTimeOffset.Now;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var reserva = new Reservas
                {
                    IdHbtn = habitacion.IdHbtn,
                    Estadoresv = "ACTIVA",
                    Tiporesv = request.Tiporesv ?? habitacion.Tipohab,
                    FechaHoraEntrada = ahora,
                    FechaHoraSalidaProgramada = ahora.AddMinutes(minutos),
                    TiempoRentaMinutos = minutos,
                    Precioresv = precio,
                    TotalConsumos = 0,
                    TotalReserva = precio,
                    Statuspagohabresv = "PENDIENTE",
                    IdUsRegistro = request.IdUsRegistro,
                    TurnoUserMTL = request.TurnoUserMTL,
                    Cliente = new ReservaCliente
                    {
                        StatusingresoCl = "INGRESADO",
                        NombreCliente = request.NombreCliente,
                        AutPlacasCl = request.AutPlacasCl,
                        AutMarcaCl = request.AutMarcaCl,
                        AutModeloCl = request.AutModeloCl,
                        AutColorCl = request.AutColorCl,
                        FrecuenciaCl = request.FrecuenciaCl,
                        AdvertCl = request.AdvertCl
                    }
                };

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                reserva.Numresv = reserva.IdResv;
                reserva.Folioordenresv = $"RES-{ahora:yyyyMMdd}-{reserva.IdResv:D6}";

                habitacion.Statushab = "OCUPADA";
                habitacion.Estadohab = "OCUPADA";
                habitacion.Limpiezahab = "NO";
                habitacion.Folioordenhab = reserva.Folioordenresv;
                habitacion.AcargoUserMTL = usuario.NombreUs ?? usuario.UsernameUs ?? request.IdUsRegistro.ToString();
                habitacion.TurnoUserMTL = request.TurnoUserMTL;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Estancia iniciada correctamente.",
                    reserva.IdResv,
                    reserva.Numresv,
                    reserva.Folioordenresv,
                    Numhab = habitacion.Numhab,
                    reserva.FechaHoraEntrada,
                    reserva.FechaHoraSalidaProgramada,
                    reserva.Precioresv,
                    reserva.TotalReserva,
                    EstadoHabitacion = "OCUPADA"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Ocurrió un error al iniciar la estancia.", detalle = ex.Message });
            }
        }

        // Actualizar datos de una reserva activa
        [HttpPut("{idResv:int}")]
        public async Task<IActionResult> ActualizarReserva(int idResv, [FromBody] ActualizarReservaRequest request)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .FirstOrDefaultAsync(r => r.IdResv == idResv);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            if (reserva.Estadoresv != "ACTIVA")
                return BadRequest(new { mensaje = "Solo se pueden modificar reservas activas." });

            if (request.Tiporesv != null)
                reserva.Tiporesv = request.Tiporesv;

            if (request.FechaHoraSalidaProgramada.HasValue)
                reserva.FechaHoraSalidaProgramada = request.FechaHoraSalidaProgramada.Value;

            if (request.Statuspagohabresv != null)
                reserva.Statuspagohabresv = request.Statuspagohabresv.Trim().ToUpper();

            if (reserva.Cliente == null)
            {
                reserva.Cliente = new ReservaCliente { StatusingresoCl = "INGRESADO" };
            }

            if (request.NombreCliente != null) reserva.Cliente.NombreCliente = request.NombreCliente;
            if (request.AutPlacasCl != null) reserva.Cliente.AutPlacasCl = request.AutPlacasCl;
            if (request.AutMarcaCl != null) reserva.Cliente.AutMarcaCl = request.AutMarcaCl;
            if (request.AutModeloCl != null) reserva.Cliente.AutModeloCl = request.AutModeloCl;
            if (request.AutColorCl != null) reserva.Cliente.AutColorCl = request.AutColorCl;
            if (request.AdvertCl != null) reserva.Cliente.AdvertCl = request.AdvertCl;
            if (request.FrecuenciaCl.HasValue) reserva.Cliente.FrecuenciaCl = request.FrecuenciaCl.Value;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Reserva actualizada correctamente." });
        }

        // Agregar consumo a una estancia
        [HttpPost("{idResv:int}/consumos")]
        public async Task<IActionResult> AgregarConsumo(int idResv, [FromBody] AgregarConsumoRequest request)
        {
            if (request.Cantidad <= 0)
                return BadRequest(new { mensaje = "La cantidad debe ser mayor a 0." });

            if (request.PrecioUnit < 0)
                return BadRequest(new { mensaje = "El precio no puede ser negativo." });

            if (string.IsNullOrWhiteSpace(request.Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            var usuario = await _context.Usersadmin.FindAsync(request.IdUsRegistro);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.IdResv == idResv);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            if (reserva.Estadoresv != "ACTIVA")
                return BadRequest(new { mensaje = "No se pueden agregar consumos a una estancia finalizada o cancelada." });

            decimal totalLinea = request.Cantidad * request.PrecioUnit;

            var consumo = new ReservaConsumo
            {
                IdResv = reserva.IdResv,
                ProductoId = request.ProductoId,
                Descripcion = request.Descripcion.Trim(),
                Cantidad = request.Cantidad,
                PrecioUnit = request.PrecioUnit,
                TotalLinea = totalLinea,
                EstadoPedido = "PENDIENTE",
                StatusPagado = "PENDIENTE",
                FechaSolicitud = DateTimeOffset.Now,
                IdUsRegistro = request.IdUsRegistro
            };

            _context.ReservaConsumos.Add(consumo);
            reserva.TotalConsumos += totalLinea;
            reserva.TotalReserva = reserva.Precioresv + reserva.TotalConsumos;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Consumo agregado correctamente.",
                consumo.IdCsms,
                consumo.Descripcion,
                consumo.Cantidad,
                consumo.PrecioUnit,
                consumo.TotalLinea,
                consumo.EstadoPedido,
                reserva.TotalConsumos,
                reserva.TotalReserva,
                EstadoVisual = "PEDIDO_PENDIENTE"
            });
        }

        // Marcar un consumo como entregado
        [HttpPost("consumos/{idConsumo:int}/entregar")]
        public async Task<IActionResult> EntregarConsumo(int idConsumo, [FromBody] EntregarConsumoRequest request)
        {
            var usuario = await _context.Usersadmin.FindAsync(request.IdUsEntrega);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            var consumo = await _context.ReservaConsumos
                .Include(c => c.Reserva)
                .FirstOrDefaultAsync(c => c.IdCsms == idConsumo);

            if (consumo == null)
                return NotFound(new { mensaje = "Consumo no encontrado." });

            if (consumo.Reserva.Estadoresv != "ACTIVA")
                return BadRequest(new { mensaje = "La estancia ya no está activa." });

            if (consumo.EstadoPedido == "ENTREGADO")
                return BadRequest(new { mensaje = "El consumo ya fue entregado." });

            consumo.EstadoPedido = "ENTREGADO";
            consumo.FechaEntrega = DateTimeOffset.Now;
            consumo.IdUsEntrega = request.IdUsEntrega;

            await _context.SaveChangesAsync();

            bool quedanPendientes = await _context.ReservaConsumos
                .AnyAsync(c => c.IdResv == consumo.IdResv && c.EstadoPedido == "PENDIENTE");

            return Ok(new
            {
                mensaje = "Consumo entregado correctamente.",
                consumo.IdCsms,
                consumo.EstadoPedido,
                consumo.FechaEntrega,
                EstadoVisual = quedanPendientes ? "PEDIDO_PENDIENTE" : "OCUPADA"
            });
        }

        // Cambiar estado de pago
        [HttpPut("{idResv:int}/pago")]
        public async Task<IActionResult> CambiarEstadoPago(int idResv, [FromBody] CambiarPagoRequest request)
        {
            var usuario = await _context.Usersadmin.FindAsync(request.IdUs);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            if (!PuedeGestionarPago(usuario.DepartamentoUs, usuario.PuestoUs))
                return StatusCode(403, new { mensaje = "El usuario no tiene permisos para modificar pagos." });

            string estado = (request.EstadoPago ?? "").Trim().ToUpper();

            if (estado != "PENDIENTE" && estado != "PARCIAL" && estado != "PAGADO")
                return BadRequest(new { mensaje = "Estado de pago inválido. Use PENDIENTE, PARCIAL o PAGADO." });

            var reserva = await _context.Reservas.FindAsync(idResv);
            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            reserva.Statuspagohabresv = estado;

            if (estado == "PAGADO")
            {
                var consumos = await _context.ReservaConsumos
                    .Where(c => c.IdResv == idResv)
                    .ToListAsync();

                foreach (var consumo in consumos)
                    consumo.StatusPagado = "PAGADO";
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Estado de pago actualizado correctamente.",
                EstadoPago = reserva.Statuspagohabresv
            });
        }

        // Finalizar estancia y mandar habitación a limpieza
        [HttpPost("{idResv:int}/finalizar")]
        public async Task<IActionResult> FinalizarReserva(int idResv, [FromBody] FinalizarReservaRequest request)
        {
            var usuario = await _context.Usersadmin.FindAsync(request.IdUsCierre);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            if (!PuedeCerrarReserva(usuario.DepartamentoUs, usuario.PuestoUs))
                return StatusCode(403, new { mensaje = "El usuario no tiene permisos para finalizar estancias." });

            var reserva = await _context.Reservas
                .Include(r => r.Habitacion)
                .Include(r => r.Consumos)
                .FirstOrDefaultAsync(r => r.IdResv == idResv);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            if (reserva.Estadoresv != "ACTIVA")
                return BadRequest(new { mensaje = "La estancia ya fue finalizada o cancelada." });

            bool pedidosPendientes = reserva.Consumos.Any(c => c.EstadoPedido == "PENDIENTE");

            if (pedidosPendientes)
                return BadRequest(new { mensaje = "No se puede finalizar la estancia mientras existan pedidos pendientes." });

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                reserva.Estadoresv = "FINALIZADA";
                reserva.FechaHoraSalidaReal = DateTimeOffset.Now;
                reserva.IdUsCierre = request.IdUsCierre;

                reserva.Habitacion.Statushab = "LIMPIEZA";
                reserva.Habitacion.Estadohab = "LIMPIEZA";
                reserva.Habitacion.Limpiezahab = "PENDIENTE";
                reserva.Habitacion.Folioordenhab = null;
                reserva.Habitacion.AcargoUserMTL = usuario.NombreUs ?? usuario.UsernameUs ?? request.IdUsCierre.ToString();

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Estancia finalizada correctamente. La habitación pasó a limpieza.",
                    reserva.IdResv,
                    reserva.FechaHoraSalidaReal,
                    EstadoReserva = reserva.Estadoresv,
                    EstadoHabitacion = "LIMPIEZA"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Ocurrió un error al finalizar la estancia.", detalle = ex.Message });
            }
        }

        // Cancelar una reserva
        [HttpPost("{idResv:int}/cancelar")]
        public async Task<IActionResult> CancelarReserva(int idResv, [FromBody] CancelarReservaRequest request)
        {
            var usuario = await _context.Usersadmin.FindAsync(request.IdUs);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario indicado no existe." });

            if (!PuedeCancelarReserva(usuario.DepartamentoUs, usuario.PuestoUs))
                return StatusCode(403, new { mensaje = "Solo administración o gerencia puede cancelar una reserva." });

            var reserva = await _context.Reservas
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.IdResv == idResv);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            if (reserva.Estadoresv != "ACTIVA")
                return BadRequest(new { mensaje = "La reserva ya fue finalizada o cancelada." });

            reserva.Estadoresv = "CANCELADA";
            reserva.FechaHoraSalidaReal = DateTimeOffset.Now;
            reserva.IdUsCierre = request.IdUs;

            reserva.Habitacion.Statushab = "LIMPIEZA";
            reserva.Habitacion.Estadohab = "LIMPIEZA";
            reserva.Habitacion.Limpiezahab = "PENDIENTE";
            reserva.Habitacion.Folioordenhab = null;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Reserva cancelada. La habitación pasó a limpieza.",
                EstadoReserva = "CANCELADA",
                EstadoHabitacion = "LIMPIEZA"
            });
        }

        // Obtener consumos de una reserva
        [HttpGet("{idResv:int}/consumos")]
        public async Task<IActionResult> GetConsumos(int idResv)
        {
            bool existe = await _context.Reservas.AnyAsync(r => r.IdResv == idResv);

            if (!existe)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            var consumos = await _context.ReservaConsumos
                .AsNoTracking()
                .Where(c => c.IdResv == idResv)
                .OrderByDescending(c => c.FechaSolicitud)
                .Select(c => new
                {
                    c.IdCsms,
                    c.ProductoId,
                    c.Descripcion,
                    c.Cantidad,
                    c.PrecioUnit,
                    c.TotalLinea,
                    c.EstadoPedido,
                    c.StatusPagado,
                    c.FechaSolicitud,
                    c.FechaEntrega,
                    c.IdUsRegistro,
                    c.IdUsEntrega
                })
                .ToListAsync();

            return Ok(consumos);
        }

        // Convierte el precio configurado en Habitación a decimal
        private static decimal ObtenerPrecio(string? precio)
        {
            if (string.IsNullOrWhiteSpace(precio))
                return 0;

            string valor = precio.Trim().Replace("$", "").Replace(" ", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
                return resultado;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.GetCultureInfo("es-MX"), out resultado))
                return resultado;

            return 0;
        }

        // Convierte el tiempo configurado de la habitación a minutos
        private static int ObtenerMinutosRenta(string? tiempo)
        {
            if (string.IsNullOrWhiteSpace(tiempo))
                return 240;

            string valor = tiempo.Trim().ToUpper();

            if (int.TryParse(valor, out int minutos))
                return minutos;

            string numero = new string(valor.TakeWhile(c => char.IsDigit(c)).ToArray());

            if (!int.TryParse(numero, out int cantidad))
                return 240;

            if (valor.Contains("HORA") || valor.Contains("HR"))
                return cantidad * 60;

            if (valor.Contains("MIN"))
                return cantidad;

            return cantidad;
        }

        // Permisos para iniciar una estancia
        private static bool PuedeRegistrarReserva(string? departamento, string? puesto)
        {
            string rol = ObtenerRol(departamento, puesto);

            return rol.Contains("ADMIN") ||
                   rol.Contains("GEREN") ||
                   rol.Contains("RECEPC") ||
                   rol.Contains("CAJA");
        }

        // Permisos para cerrar una estancia
        private static bool PuedeCerrarReserva(string? departamento, string? puesto)
        {
            string rol = ObtenerRol(departamento, puesto);

            return rol.Contains("ADMIN") ||
                   rol.Contains("GEREN") ||
                   rol.Contains("RECEPC") ||
                   rol.Contains("CAJA");
        }

        // Permisos para gestionar pagos
        private static bool PuedeGestionarPago(string? departamento, string? puesto)
        {
            string rol = ObtenerRol(departamento, puesto);

            return rol.Contains("ADMIN") ||
                   rol.Contains("GEREN") ||
                   rol.Contains("RECEPC") ||
                   rol.Contains("CAJA");
        }

        // Solo administración y gerencia pueden cancelar
        private static bool PuedeCancelarReserva(string? departamento, string? puesto)
        {
            string rol = ObtenerRol(departamento, puesto);

            return rol.Contains("ADMIN") || rol.Contains("GEREN");
        }

        // Obtiene el rol desde departamento o puesto
        private static string ObtenerRol(string? departamento, string? puesto)
        {
            if (!string.IsNullOrWhiteSpace(departamento))
                return departamento.Trim().ToUpper();

            return (puesto ?? "").Trim().ToUpper();
        }
    }

    // Datos necesarios para iniciar una estancia
    public class CrearReservaRequest
    {
        public int Numhab { get; set; }
        public int IdUsRegistro { get; set; }
        public int TurnoUserMTL { get; set; }
        public string? Tiporesv { get; set; }
        public int? TiempoRentaMinutos { get; set; }
        public decimal? Precio { get; set; }
        public string? NombreCliente { get; set; }
        public string? AutPlacasCl { get; set; }
        public string? AutMarcaCl { get; set; }
        public string? AutModeloCl { get; set; }
        public string? AutColorCl { get; set; }
        public int FrecuenciaCl { get; set; }
        public string? AdvertCl { get; set; }
    }

    // Datos que pueden modificarse durante la estancia
    public class ActualizarReservaRequest
    {
        public string? Tiporesv { get; set; }
        public DateTimeOffset? FechaHoraSalidaProgramada { get; set; }
        public string? Statuspagohabresv { get; set; }
        public string? NombreCliente { get; set; }
        public string? AutPlacasCl { get; set; }
        public string? AutMarcaCl { get; set; }
        public string? AutModeloCl { get; set; }
        public string? AutColorCl { get; set; }
        public int? FrecuenciaCl { get; set; }
        public string? AdvertCl { get; set; }
    }

    // Datos para agregar un consumo
    public class AgregarConsumoRequest
    {
        public int? ProductoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal PrecioUnit { get; set; }
        public int IdUsRegistro { get; set; }
    }

    // Datos para entregar un consumo
    public class EntregarConsumoRequest
    {
        public int IdUsEntrega { get; set; }
    }

    // Datos para modificar el estado de pago
    public class CambiarPagoRequest
    {
        public string EstadoPago { get; set; } = string.Empty;
        public int IdUs { get; set; }
    }

    // Datos para finalizar una estancia
    public class FinalizarReservaRequest
    {
        public int IdUsCierre { get; set; }
    }

    // Datos para cancelar una reserva
    public class CancelarReservaRequest
    {
        public int IdUs { get; set; }
        public string? Motivo { get; set; }
    }
}