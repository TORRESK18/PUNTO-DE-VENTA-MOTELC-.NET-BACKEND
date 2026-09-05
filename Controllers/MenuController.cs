using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTLCRISTALVK18BACK.Contexts;
using MTLCRISTALVK18BACK.Models.Menu;
using MTLCRISTALVK18BACK.Models.Menu.DTOs;

namespace MTLCRISTALVK18BACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MTLCRISTALContexts _context;

        public MenuController(MTLCRISTALContexts context)
        {
            _context = context;
        }

        // =====================================================
        // GET: api/Menu
        // DEVUELVE TODO EL MENU ACTIVO
        // CATEGORIAS -> PRODUCTOS -> VARIANTES
        // =====================================================
        [HttpGet]
        public async Task<ActionResult> GetMenu()
        {
            var menu = await _context.CategoriasMenu
                .AsNoTracking()
                .Where(c => c.Activo)
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Nombre)
                .Select(c => new
                {
                    idCategoria = c.IdCategoria,
                    nombre = c.Nombre,
                    descripcion = c.Descripcion,
                    icono = c.Icono,
                    orden = c.Orden,
                    activo = c.Activo,

                    productos = c.Productos
                        .Where(p => p.Activo)
                        .OrderBy(p => p.Orden)
                        .ThenBy(p => p.Nombre)
                        .Select(p => new
                        {
                            idProducto = p.IdProducto,
                            categoriaId = p.CategoriaId,
                            categoria = c.Nombre,
                            nombre = p.Nombre,
                            descripcion = p.Descripcion,
                            precioBase = p.PrecioBase,
                            imagen = p.Imagen,
                            orden = p.Orden,
                            activo = p.Activo,

                            controlaInventario = p.ControlaInventario,
                            stockActual = p.StockActual,
                            stockMinimo = p.StockMinimo,

                            variantes = p.Variantes
                                .Where(v => v.Activo)
                                .OrderBy(v => v.Orden)
                                .ThenBy(v => v.Nombre)
                                .Select(v => new
                                {
                                    idVariante = v.IdVariante,
                                    productoId = v.ProductoId,
                                    nombre = v.Nombre,
                                    precio = v.Precio,
                                    orden = v.Orden,
                                    activo = v.Activo
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(menu);
        }

        // =====================================================
        // GET: api/Menu/categorias
        // =====================================================
        [HttpGet("categorias")]
        public async Task<ActionResult> GetCategorias()
        {
            var categorias = await _context.CategoriasMenu
                .AsNoTracking()
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Nombre)
                .Select(c => new
                {
                    idCategoria = c.IdCategoria,
                    nombre = c.Nombre,
                    descripcion = c.Descripcion,
                    icono = c.Icono,
                    orden = c.Orden,
                    activo = c.Activo
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // =====================================================
        // GET: api/Menu/categoria/1
        // =====================================================
        [HttpGet("categoria/{id:int}")]
        public async Task<ActionResult> GetCategoria(int id)
        {
            var categoria = await _context.CategoriasMenu
                .AsNoTracking()
                .Where(c => c.IdCategoria == id)
                .Select(c => new
                {
                    idCategoria = c.IdCategoria,
                    nombre = c.Nombre,
                    descripcion = c.Descripcion,
                    icono = c.Icono,
                    orden = c.Orden,
                    activo = c.Activo,

                    productos = c.Productos
                        .OrderBy(p => p.Orden)
                        .ThenBy(p => p.Nombre)
                        .Select(p => new
                        {
                            idProducto = p.IdProducto,
                            categoriaId = p.CategoriaId,
                            categoria = c.Nombre,
                            nombre = p.Nombre,
                            descripcion = p.Descripcion,
                            precioBase = p.PrecioBase,
                            imagen = p.Imagen,
                            orden = p.Orden,
                            activo = p.Activo,

                            controlaInventario = p.ControlaInventario,
                            stockActual = p.StockActual,
                            stockMinimo = p.StockMinimo,

                            variantes = p.Variantes
                                .OrderBy(v => v.Orden)
                                .ThenBy(v => v.Nombre)
                                .Select(v => new
                                {
                                    idVariante = v.IdVariante,
                                    productoId = v.ProductoId,
                                    nombre = v.Nombre,
                                    precio = v.Precio,
                                    orden = v.Orden,
                                    activo = v.Activo
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
                return NotFound(new { mensaje = "La categoría no existe." });

            return Ok(categoria);
        }

        // =====================================================
        // POST: api/Menu/categorias
        // =====================================================
        [HttpPost("categorias")]
        public async Task<ActionResult> CrearCategoria([FromBody] CategoriaMenu request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre de la categoría es obligatorio." });

            var existe = await _context.CategoriasMenu
                .AnyAsync(c => c.Nombre.ToUpper() == request.Nombre.Trim().ToUpper());

            if (existe)
                return BadRequest(new { mensaje = "Ya existe una categoría con ese nombre." });

            var categoria = new CategoriaMenu
            {
                Nombre = request.Nombre.Trim().ToUpper(),
                Descripcion = request.Descripcion?.Trim(),
                Icono = request.Icono?.Trim(),
                Orden = request.Orden,
                Activo = request.Activo
            };

            _context.CategoriasMenu.Add(categoria);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Categoría creada correctamente.",
                idCategoria = categoria.IdCategoria
            });
        }

        // =====================================================
        // PUT: api/Menu/categorias/1
        // =====================================================
        [HttpPut("categorias/{id:int}")]
        public async Task<ActionResult> ActualizarCategoria(
            int id,
            [FromBody] CategoriaMenu request)
        {
            var categoria = await _context.CategoriasMenu
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
                return NotFound(new { mensaje = "La categoría no existe." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre de la categoría es obligatorio." });

            categoria.Nombre = request.Nombre.Trim().ToUpper();
            categoria.Descripcion = request.Descripcion?.Trim();
            categoria.Icono = request.Icono?.Trim();
            categoria.Orden = request.Orden;
            categoria.Activo = request.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Categoría actualizada correctamente." });
        }

        // =====================================================
        // POST: api/Menu/productos
        // =====================================================
        [HttpPost("productos")]
        public async Task<ActionResult> CrearProducto(
            [FromBody] ProductoMenuRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre del producto es obligatorio." });

            var categoriaExiste = await _context.CategoriasMenu
                .AnyAsync(c => c.IdCategoria == request.CategoriaId && c.Activo);

            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría seleccionada no existe." });

            if (request.PrecioBase < 0)
                return BadRequest(new { mensaje = "El precio base no puede ser negativo." });

            if (request.StockActual < 0)
                return BadRequest(new { mensaje = "El stock actual no puede ser negativo." });

            if (request.StockMinimo < 0)
                return BadRequest(new { mensaje = "El stock mínimo no puede ser negativo." });

            var producto = new ProductoMenu
            {
                CategoriaId = request.CategoriaId,
                Nombre = request.Nombre.Trim().ToUpper(),
                Descripcion = request.Descripcion?.Trim(),
                PrecioBase = request.PrecioBase,
                Imagen = request.Imagen?.Trim(),
                Orden = request.Orden,
                Activo = request.Activo,

                ControlaInventario = request.ControlaInventario,
                StockActual = request.StockActual,
                StockMinimo = request.StockMinimo
            };

            _context.ProductosMenu.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto creado correctamente.",
                idProducto = producto.IdProducto,
                controlaInventario = producto.ControlaInventario,
                stockActual = producto.StockActual,
                stockMinimo = producto.StockMinimo
            });
        }

        // =====================================================
        // GET: api/Menu/producto/1
        // =====================================================
        [HttpGet("producto/{id:int}")]
        public async Task<ActionResult> GetProducto(int id)
        {
            var producto = await _context.ProductosMenu
                .AsNoTracking()
                .Where(p => p.IdProducto == id)
                .Select(p => new
                {
                    idProducto = p.IdProducto,
                    categoriaId = p.CategoriaId,
                    categoria = p.Categoria.Nombre,
                    nombre = p.Nombre,
                    descripcion = p.Descripcion,
                    precioBase = p.PrecioBase,
                    imagen = p.Imagen,
                    orden = p.Orden,
                    activo = p.Activo,

                    controlaInventario = p.ControlaInventario,
                    stockActual = p.StockActual,
                    stockMinimo = p.StockMinimo,

                    variantes = p.Variantes
                        .OrderBy(v => v.Orden)
                        .ThenBy(v => v.Nombre)
                        .Select(v => new
                        {
                            idVariante = v.IdVariante,
                            productoId = v.ProductoId,
                            nombre = v.Nombre,
                            precio = v.Precio,
                            orden = v.Orden,
                            activo = v.Activo
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (producto == null)
                return NotFound(new { mensaje = "El producto no existe." });

            return Ok(producto);
        }

        // =====================================================
        // PUT: api/Menu/productos/1
        // =====================================================
        [HttpPut("productos/{id:int}")]
        public async Task<ActionResult> ActualizarProducto(
            int id,
            [FromBody] ProductoMenuRequest request)
        {
            var producto = await _context.ProductosMenu
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound(new { mensaje = "El producto no existe." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre del producto es obligatorio." });

            var categoriaExiste = await _context.CategoriasMenu
                .AnyAsync(c => c.IdCategoria == request.CategoriaId && c.Activo);

            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría seleccionada no existe." });

            if (request.PrecioBase < 0)
                return BadRequest(new { mensaje = "El precio base no puede ser negativo." });

            if (request.StockActual < 0)
                return BadRequest(new { mensaje = "El stock actual no puede ser negativo." });

            if (request.StockMinimo < 0)
                return BadRequest(new { mensaje = "El stock mínimo no puede ser negativo." });

            producto.CategoriaId = request.CategoriaId;
            producto.Nombre = request.Nombre.Trim().ToUpper();
            producto.Descripcion = request.Descripcion?.Trim();
            producto.PrecioBase = request.PrecioBase;
            producto.Imagen = request.Imagen?.Trim();
            producto.Orden = request.Orden;
            producto.Activo = request.Activo;

            producto.ControlaInventario = request.ControlaInventario;
            producto.StockActual = request.StockActual;
            producto.StockMinimo = request.StockMinimo;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto actualizado correctamente.",
                idProducto = producto.IdProducto,
                controlaInventario = producto.ControlaInventario,
                stockActual = producto.StockActual,
                stockMinimo = producto.StockMinimo
            });
        }

        // =====================================================
        // POST: api/Menu/productos/1/variantes
        // =====================================================
        [HttpPost("productos/{idProducto:int}/variantes")]
        public async Task<ActionResult> CrearVariante(
            int idProducto,
            [FromBody] VarianteProductoMenu request)
        {
            var producto = await _context.ProductosMenu
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            if (producto == null)
                return NotFound(new { mensaje = "El producto no existe." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre de la variante es obligatorio." });

            if (request.Precio < 0)
                return BadRequest(new { mensaje = "El precio no puede ser negativo." });

            var variante = new VarianteProductoMenu
            {
                ProductoId = idProducto,
                Nombre = request.Nombre.Trim().ToUpper(),
                Precio = request.Precio,
                Orden = request.Orden,
                Activo = request.Activo
            };

            _context.VariantesProductoMenu.Add(variante);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Variante creada correctamente.",
                idVariante = variante.IdVariante
            });
        }

        // =====================================================
        // PUT: api/Menu/variantes/1
        // =====================================================
        [HttpPut("variantes/{id:int}")]
        public async Task<ActionResult> ActualizarVariante(
            int id,
            [FromBody] VarianteProductoMenu request)
        {
            var variante = await _context.VariantesProductoMenu
                .FirstOrDefaultAsync(v => v.IdVariante == id);

            if (variante == null)
                return NotFound(new { mensaje = "La variante no existe." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre de la variante es obligatorio." });

            if (request.Precio < 0)
                return BadRequest(new { mensaje = "El precio no puede ser negativo." });

            variante.Nombre = request.Nombre.Trim().ToUpper();
            variante.Precio = request.Precio;
            variante.Orden = request.Orden;
            variante.Activo = request.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Variante actualizada correctamente." });
        }

        // =====================================================
        // DELETE LOGICO CATEGORIA
        // =====================================================
        [HttpDelete("categorias/{id:int}")]
        public async Task<ActionResult> DesactivarCategoria(int id)
        {
            var categoria = await _context.CategoriasMenu
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
                return NotFound(new { mensaje = "La categoría no existe." });

            categoria.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Categoría desactivada correctamente." });
        }

        // =====================================================
        // DELETE LOGICO PRODUCTO
        // =====================================================
        [HttpDelete("productos/{id:int}")]
        public async Task<ActionResult> DesactivarProducto(int id)
        {
            var producto = await _context.ProductosMenu
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound(new { mensaje = "El producto no existe." });

            producto.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto desactivado correctamente." });
        }

        // =====================================================
        // DELETE LOGICO VARIANTE
        // =====================================================
        [HttpDelete("variantes/{id:int}")]
        public async Task<ActionResult> DesactivarVariante(int id)
        {
            var variante = await _context.VariantesProductoMenu
                .FirstOrDefaultAsync(v => v.IdVariante == id);

            if (variante == null)
                return NotFound(new { mensaje = "La variante no existe." });

            variante.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Variante desactivada correctamente." });
        }
    }
}
