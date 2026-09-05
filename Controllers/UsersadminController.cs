using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTLCRISTALVK18BACK.Contexts;
using MTLCRISTALVK18BACK.Models.Usersadmin;

namespace MTLCRISTALVK18BACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersadminController : ControllerBase
    {

        private readonly MTLCRISTALContexts _context;

        public UsersadminController(MTLCRISTALContexts context)
        {
            _context = context;
        }


        // GET: api/Usersadmin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usersadmin>>> GetUsersadmin()
        {
            return await _context.Usersadmin.ToListAsync();
        }

        // GET: api/Usersadmin/{NtrabajadorUs}
        [HttpGet("{ntrabajadorUs}")]
        public async Task<ActionResult<Usersadmin>> GetUsersadmin(int NtrabajadorUs)
        {
            var usersadmin = await _context.Usersadmin.FirstOrDefaultAsync(u => u.NtrabajadorUs == NtrabajadorUs);

            if (usersadmin == null)
            {
                return NotFound();
            }

            return usersadmin;
        }

        // PUT: api/Usersadmin/{idAdmi}
        [HttpPut("{idUs}")]
        public async Task<IActionResult> PutUsersadmin(int idUs, Usersadmin usersadmin)
        {
            if (idUs != usersadmin.IdUs)
            {
                return BadRequest();
            }

            _context.Entry(usersadmin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersadminExists(idUs))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usersadmin
        [HttpPost]
        public async Task<ActionResult<Usersadmin>> PostUsersadmin(Usersadmin usersadmin)
        {
            _context.Usersadmin.Add(usersadmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersadmin", new { NtrabajadorUs = usersadmin.NtrabajadorUs }, usersadmin);
        }

        // DELETE: api/Usersadmin/{idUs}
        [HttpDelete("{idUs}")]
        public async Task<IActionResult> DeleteUsersadmin(int idUs)
        {
            var usersadmin = await _context.Usersadmin.FindAsync(idUs);
            if (usersadmin == null)
            {
                return NotFound();
            }

            _context.Usersadmin.Remove(usersadmin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersadminExists(int idUs)
        {
            return _context.Usersadmin.Any(e => e.IdUs == idUs);
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAdmin([FromBody] Usersadmin loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UsernameUs) && string.IsNullOrEmpty(loginRequest.EmailUs))
            {
                return BadRequest(new { message = "Debes proporcionar el Username o el Email." });
            }

            if (string.IsNullOrEmpty(loginRequest.PasswordUs))
            {
                return BadRequest(new { message = "El campo de contraseña es obligatorio." });
            }

            // Convertir a minúsculas para comparación (solo email)
            string? email = loginRequest.EmailUs?.ToLower();
            string? username = loginRequest.UsernameUs;

            // Buscar al usuario por Username o Email
            var admin = await _context.Usersadmin.FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(email) && u.EmailUs != null && u.EmailUs.ToLower() == email) ||
                (!string.IsNullOrEmpty(username) && u.UsernameUs != null && u.UsernameUs == username)
            );

            if (admin == null)
            {
                return Unauthorized(new { message = "Usuario no encontrado con ese Email o Username." });
            }

            if (admin.PasswordUs != loginRequest.PasswordUs)
            {
                return Unauthorized(new { message = "La contraseña es incorrecta." });
            }

            return Ok(new
            {
                success = true,
                message = "Autenticación exitosa.",
                nombreUs = admin.NombreUs,
                ntrabajadorUs = admin.NtrabajadorUs,
                departamentoUs = admin.DepartamentoUs,
                puestoUs = admin.PuestoUs
            });
        }




    }
}
