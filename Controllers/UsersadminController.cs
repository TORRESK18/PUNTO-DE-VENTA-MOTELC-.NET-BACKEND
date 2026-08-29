using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;

using MTLCRISTALVK18BACK.Contexts;
using MTLCRISTALVK18BACK.Models.Usersadmin;


namespace MTLCRISTALVK18BACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersadminController : ControllerBase
    {
        private readonly MTLCRISTALContexts _context;

        private readonly PasswordHasher<Usersadmin> _passwordHasher;


        public UsersadminController(
            MTLCRISTALContexts context)
        {
            _context = context;

            _passwordHasher =
                new PasswordHasher<Usersadmin>();
        }



        /* ================================================= */
        /* GET: api/Usersadmin */
        /* ================================================= */

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAdminDto>>>
            GetUsersadmin()
        {
            var usuarios = await _context.Usersadmin
                .AsNoTracking()
                .OrderBy(x => x.NombreUs)
                .Select(x => new UserAdminDto
                {
                    IdUs = x.IdUs,

                    NtrabajadorUs =
                        x.NtrabajadorUs,

                    NombreUs =
                        x.NombreUs,

                    PuestoUs =
                        x.PuestoUs,

                    DepartamentoUs =
                        x.DepartamentoUs,

                    EmailUs =
                        x.EmailUs,

                    UsernameUs =
                        x.UsernameUs
                })
                .ToListAsync();


            return Ok(usuarios);
        }



        /* ================================================= */
        /* GET POR NÚMERO TRABAJADOR */
        /* ================================================= */

        [HttpGet("trabajador/{ntrabajadorUs:int}")]
        public async Task<ActionResult<UserAdminDto>>
            GetByTrabajador(
                int ntrabajadorUs)
        {
            var usuario =
                await _context.Usersadmin
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.NtrabajadorUs ==
                            ntrabajadorUs
                    );


            if (usuario == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Usuario no encontrado."
                    }
                );
            }


            return Ok(ToDto(usuario));
        }



        /* ================================================= */
        /* POST: api/Usersadmin */
        /* ================================================= */

        [HttpPost]
        public async Task<IActionResult>
            PostUsersadmin(
                [FromBody]
                CreateUserAdminRequest request)
        {
            if (
                request.NtrabajadorUs <= 0 ||
                string.IsNullOrWhiteSpace(
                    request.NombreUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.PuestoUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.DepartamentoUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.UsernameUs
                )
            )
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Completa todos los campos obligatorios."
                    }
                );
            }


            string username =
                request.UsernameUs.Trim();


            string? email =
                string.IsNullOrWhiteSpace(
                    request.EmailUs
                )
                ? null
                : request.EmailUs
                    .Trim()
                    .ToLowerInvariant();



            /* ============================================= */
            /* VALIDAR NÚMERO TRABAJADOR */
            /* ============================================= */

            bool trabajadorExiste =
                await _context.Usersadmin
                    .AnyAsync(
                        x =>
                            x.NtrabajadorUs ==
                            request.NtrabajadorUs
                    );


            if (trabajadorExiste)
            {
                return Conflict(
                    new
                    {
                        message =
                            "Ya existe un usuario con ese número de trabajador."
                    }
                );
            }



            /* ============================================= */
            /* VALIDAR USERNAME */
            /* ============================================= */

            bool usernameExiste =
                await _context.Usersadmin
                    .AnyAsync(
                        x =>
                            x.UsernameUs != null &&
                            x.UsernameUs.ToLower() ==
                            username.ToLower()
                    );


            if (usernameExiste)
            {
                return Conflict(
                    new
                    {
                        message =
                            "El nombre de usuario ya está registrado."
                    }
                );
            }



            /* ============================================= */
            /* VALIDAR EMAIL */
            /* ============================================= */

            if (email != null)
            {
                bool emailExiste =
                    await _context.Usersadmin
                        .AnyAsync(
                            x =>
                                x.EmailUs != null &&
                                x.EmailUs.ToLower() ==
                                email
                        );


                if (emailExiste)
                {
                    return Conflict(
                        new
                        {
                            message =
                                "El correo electrónico ya está registrado."
                        }
                    );
                }
            }



            /* ============================================= */
            /* CONTRASEÑA TEMPORAL */
            /* ============================================= */

            string temporaryPassword =
                GenerateTemporaryPassword();



            /* ============================================= */
            /* CREAR ENTIDAD */
            /* ============================================= */

            var usuario =
                new Usersadmin
                {
                    NtrabajadorUs =
                        request.NtrabajadorUs,

                    NombreUs =
                        request.NombreUs
                            .Trim()
                            .ToUpperInvariant(),

                    PuestoUs =
                        request.PuestoUs
                            .Trim()
                            .ToUpperInvariant(),

                    DepartamentoUs =
                        request.DepartamentoUs
                            .Trim()
                            .ToUpperInvariant(),

                    EmailUs =
                        email,

                    UsernameUs =
                        username
                };



            /* ============================================= */
            /* HASH PASSWORD */
            /* ============================================= */

            usuario.PasswordUs =
                _passwordHasher.HashPassword(
                    usuario,
                    temporaryPassword
                );


            _context.Usersadmin.Add(
                usuario
            );


            await _context.SaveChangesAsync();



            var dto =
                ToDto(usuario);


            return CreatedAtAction(
                nameof(GetByTrabajador),
                new
                {
                    ntrabajadorUs =
                        usuario.NtrabajadorUs
                },
                new
                {
                    message =
                        "Usuario creado correctamente.",

                    usuario =
                        dto,

                    temporaryPassword =
                        temporaryPassword
                }
            );
        }



        /* ================================================= */
        /* PUT: api/Usersadmin/{idUs} */
        /* ================================================= */

        [HttpPut("{idUs:int}")]
        public async Task<ActionResult<UserAdminDto>>
            PutUsersadmin(
                int idUs,
                [FromBody]
                UpdateUserAdminRequest request)
        {
            var usuario =
                await _context.Usersadmin
                    .FirstOrDefaultAsync(
                        x =>
                            x.IdUs == idUs
                    );


            if (usuario == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Usuario no encontrado."
                    }
                );
            }



            if (
                request.NtrabajadorUs <= 0 ||
                string.IsNullOrWhiteSpace(
                    request.NombreUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.PuestoUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.DepartamentoUs
                ) ||
                string.IsNullOrWhiteSpace(
                    request.UsernameUs
                )
            )
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Completa todos los campos obligatorios."
                    }
                );
            }



            string username =
                request.UsernameUs.Trim();


            string? email =
                string.IsNullOrWhiteSpace(
                    request.EmailUs
                )
                ? null
                : request.EmailUs
                    .Trim()
                    .ToLowerInvariant();



            /* ============================================= */
            /* DUPLICADO TRABAJADOR */
            /* ============================================= */

            bool trabajadorExiste =
                await _context.Usersadmin
                    .AnyAsync(
                        x =>
                            x.IdUs != idUs &&
                            x.NtrabajadorUs ==
                            request.NtrabajadorUs
                    );


            if (trabajadorExiste)
            {
                return Conflict(
                    new
                    {
                        message =
                            "Otro usuario ya utiliza ese número de trabajador."
                    }
                );
            }



            /* ============================================= */
            /* DUPLICADO USERNAME */
            /* ============================================= */

            bool usernameExiste =
                await _context.Usersadmin
                    .AnyAsync(
                        x =>
                            x.IdUs != idUs &&
                            x.UsernameUs != null &&
                            x.UsernameUs.ToLower() ==
                            username.ToLower()
                    );


            if (usernameExiste)
            {
                return Conflict(
                    new
                    {
                        message =
                            "Otro usuario ya utiliza ese nombre de usuario."
                    }
                );
            }



            /* ============================================= */
            /* DUPLICADO EMAIL */
            /* ============================================= */

            if (email != null)
            {
                bool emailExiste =
                    await _context.Usersadmin
                        .AnyAsync(
                            x =>
                                x.IdUs != idUs &&
                                x.EmailUs != null &&
                                x.EmailUs.ToLower() ==
                                email
                        );


                if (emailExiste)
                {
                    return Conflict(
                        new
                        {
                            message =
                                "Otro usuario ya utiliza ese correo electrónico."
                        }
                    );
                }
            }



            /* ============================================= */
            /* ACTUALIZAR */
            /* ============================================= */

            usuario.NtrabajadorUs =
                request.NtrabajadorUs;


            usuario.NombreUs =
                request.NombreUs
                    .Trim()
                    .ToUpperInvariant();


            usuario.PuestoUs =
                request.PuestoUs
                    .Trim()
                    .ToUpperInvariant();


            usuario.DepartamentoUs =
                request.DepartamentoUs
                    .Trim()
                    .ToUpperInvariant();


            usuario.EmailUs =
                email;


            usuario.UsernameUs =
                username;



            /*
             * IMPORTANTE:
             *
             * PasswordUs NO se toca.
             *
             * Editar el perfil de un usuario
             * nunca debe reemplazar accidentalmente
             * su contraseña.
             */


            await _context.SaveChangesAsync();


            return Ok(
                ToDto(usuario)
            );
        }



        /* ================================================= */
        /* DELETE */
        /* ================================================= */

        [HttpDelete("{idUs:int}")]
        public async Task<IActionResult>
            DeleteUsersadmin(
                int idUs)
        {
            var usuario =
                await _context.Usersadmin
                    .FindAsync(idUs);


            if (usuario == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Usuario no encontrado."
                    }
                );
            }


            _context.Usersadmin.Remove(
                usuario
            );


            await _context.SaveChangesAsync();


            return NoContent();
        }



        /* ================================================= */
        /* RESET PASSWORD */
        /* ================================================= */

        [HttpPost("{idUs:int}/reset-password")]
        public async Task<IActionResult>
            ResetPassword(
                int idUs)
        {
            var usuario =
                await _context.Usersadmin
                    .FirstOrDefaultAsync(
                        x =>
                            x.IdUs == idUs
                    );


            if (usuario == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Usuario no encontrado."
                    }
                );
            }


            string temporaryPassword =
                GenerateTemporaryPassword();


            usuario.PasswordUs =
                _passwordHasher.HashPassword(
                    usuario,
                    temporaryPassword
                );


            await _context.SaveChangesAsync();


            return Ok(
                new
                {
                    message =
                        "Contraseña restablecida correctamente.",

                    temporaryPassword =
                        temporaryPassword
                }
            );
        }



        /* ================================================= */
        /* LOGIN */
        /* ================================================= */

        [HttpPost("authenticate")]
        public async Task<IActionResult>
            AuthenticateAdmin(
                [FromBody]
                AuthenticateUserRequest request)
        {
            if (
                string.IsNullOrWhiteSpace(
                    request.UsernameUs
                ) &&
                string.IsNullOrWhiteSpace(
                    request.EmailUs
                )
            )
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Debes proporcionar Username o Email."
                    }
                );
            }


            if (
                string.IsNullOrWhiteSpace(
                    request.PasswordUs
                )
            )
            {
                return BadRequest(
                    new
                    {
                        message =
                            "La contraseña es obligatoria."
                    }
                );
            }



            string? username =
                request.UsernameUs?.Trim();


            string? email =
                request.EmailUs?
                    .Trim()
                    .ToLowerInvariant();



            /* ============================================= */
            /* BUSCAR USUARIO */
            /* ============================================= */

            var usuario =
                await _context.Usersadmin
                    .FirstOrDefaultAsync(
                        x =>

                            (
                                email != null &&
                                x.EmailUs != null &&
                                x.EmailUs.ToLower() ==
                                email
                            )

                            ||

                            (
                                username != null &&
                                x.UsernameUs != null &&
                                x.UsernameUs.ToLower() ==
                                username.ToLower()
                            )
                    );


            if (usuario == null)
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "Usuario no encontrado."
                    }
                );
            }



            bool passwordValida =
                false;



            /* ============================================= */
            /* HASH MODERNO */
            /* ============================================= */

            if (
                IsPasswordHash(
                    usuario.PasswordUs
                )
            )
            {
                var resultado =
                    _passwordHasher
                        .VerifyHashedPassword(
                            usuario,
                            usuario.PasswordUs,
                            request.PasswordUs
                        );


                passwordValida =
                    resultado ==
                    PasswordVerificationResult.Success

                    ||

                    resultado ==
                    PasswordVerificationResult.SuccessRehashNeeded;



                if (
                    resultado ==
                    PasswordVerificationResult.SuccessRehashNeeded
                )
                {
                    usuario.PasswordUs =
                        _passwordHasher.HashPassword(
                            usuario,
                            request.PasswordUs
                        );


                    await _context.SaveChangesAsync();
                }
            }



            /* ============================================= */
            /* COMPATIBILIDAD CONTRASEÑAS VIEJAS */
            /* ============================================= */

            else
            {
                /*
                 * Tu sistema viejo guardaba
                 * PasswordUs como texto.
                 *
                 * Si coincide, permitimos el login
                 * una sola vez y automáticamente
                 * convertimos la contraseña a hash.
                 */

                passwordValida =
                    usuario.PasswordUs ==
                    request.PasswordUs;


                if (passwordValida)
                {
                    usuario.PasswordUs =
                        _passwordHasher.HashPassword(
                            usuario,
                            request.PasswordUs
                        );


                    await _context.SaveChangesAsync();
                }
            }



            if (!passwordValida)
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "La contraseña es incorrecta."
                    }
                );
            }



            /* ============================================= */
            /* RESPUESTA */
            /* ============================================= */

            return Ok(
                new
                {
                    success = true,

                    message =
                        "Autenticación exitosa.",

                    idUs =
                        usuario.IdUs,

                    nombreUs =
                        usuario.NombreUs,

                    ntrabajadorUs =
                        usuario.NtrabajadorUs,

                    departamentoUs =
                        usuario.DepartamentoUs,

                    puestoUs =
                        usuario.PuestoUs,

                    usernameUs =
                        usuario.UsernameUs
                }
            );
        }



        /* ================================================= */
        /* DTO */
        /* ================================================= */

        private static UserAdminDto
            ToDto(
                Usersadmin usuario)
        {
            return new UserAdminDto
            {
                IdUs =
                    usuario.IdUs,

                NtrabajadorUs =
                    usuario.NtrabajadorUs,

                NombreUs =
                    usuario.NombreUs,

                PuestoUs =
                    usuario.PuestoUs,

                DepartamentoUs =
                    usuario.DepartamentoUs,

                EmailUs =
                    usuario.EmailUs,

                UsernameUs =
                    usuario.UsernameUs
            };
        }



        /* ================================================= */
        /* DETECTAR HASH */
        /* ================================================= */

        private static bool
            IsPasswordHash(
                string? password)
        {
            if (
                string.IsNullOrWhiteSpace(
                    password
                )
            )
            {
                return false;
            }


            /*
             * PasswordHasher de ASP.NET Core
             * actualmente genera cadenas Base64
             * que normalmente comienzan AQAAAA.
             */

            return password.StartsWith(
                "AQAAAA",
                StringComparison.Ordinal
            );
        }



        /* ================================================= */
        /* GENERAR PASSWORD */
        /* ================================================= */

        private static string
            GenerateTemporaryPassword()
        {
            const string uppercase =
                "ABCDEFGHJKLMNPQRSTUVWXYZ";

            const string lowercase =
                "abcdefghijkmnopqrstuvwxyz";

            const string numbers =
                "23456789";

            const string specials =
                "@#$%";


            string all =
                uppercase +
                lowercase +
                numbers +
                specials;


            var chars =
                new List<char>
                {
                    uppercase[
                        RandomNumberGenerator
                            .GetInt32(
                                uppercase.Length
                            )
                    ],

                    lowercase[
                        RandomNumberGenerator
                            .GetInt32(
                                lowercase.Length
                            )
                    ],

                    numbers[
                        RandomNumberGenerator
                            .GetInt32(
                                numbers.Length
                            )
                    ],

                    specials[
                        RandomNumberGenerator
                            .GetInt32(
                                specials.Length
                            )
                    ]
                };


            while (chars.Count < 12)
            {
                chars.Add(
                    all[
                        RandomNumberGenerator
                            .GetInt32(
                                all.Length
                            )
                    ]
                );
            }



            /*
             * Mezclar caracteres
             */

            for (
                int i =
                    chars.Count - 1;

                i > 0;

                i--
            )
            {
                int j =
                    RandomNumberGenerator
                        .GetInt32(
                            i + 1
                        );


                (
                    chars[i],
                    chars[j]
                )
                =
                (
                    chars[j],
                    chars[i]
                );
            }


            return new string(
                chars.ToArray()
            );
        }
    }



    /* ===================================================== */
    /* DTO QUE SE ENVÍA AL FRONTEND */
    /* ===================================================== */

    public class UserAdminDto
    {
        public int IdUs { get; set; }

        public int NtrabajadorUs { get; set; }

        public string? NombreUs { get; set; }

        public string? PuestoUs { get; set; }

        public string? DepartamentoUs { get; set; }

        public string? EmailUs { get; set; }

        public string? UsernameUs { get; set; }
    }



    /* ===================================================== */
    /* CREATE REQUEST */
    /* ===================================================== */

    public class CreateUserAdminRequest
    {
        public int NtrabajadorUs { get; set; }

        public string NombreUs { get; set; } =
            string.Empty;

        public string PuestoUs { get; set; } =
            string.Empty;

        public string DepartamentoUs { get; set; } =
            string.Empty;

        public string? EmailUs { get; set; }

        public string UsernameUs { get; set; } =
            string.Empty;
    }



    /* ===================================================== */
    /* UPDATE REQUEST */
    /* ===================================================== */

    public class UpdateUserAdminRequest
    {
        public int NtrabajadorUs { get; set; }

        public string NombreUs { get; set; } =
            string.Empty;

        public string PuestoUs { get; set; } =
            string.Empty;

        public string DepartamentoUs { get; set; } =
            string.Empty;

        public string? EmailUs { get; set; }

        public string UsernameUs { get; set; } =
            string.Empty;
    }



    /* ===================================================== */
    /* LOGIN REQUEST */
    /* ===================================================== */

    public class AuthenticateUserRequest
    {
        public string? UsernameUs { get; set; }

        public string? EmailUs { get; set; }

        public string PasswordUs { get; set; } =
            string.Empty;
    }
}