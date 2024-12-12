using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIAuthController : ControllerBase
    {
        private readonly IAuth _authRep;
        private readonly IAuthService _authService;

        public APIAuthController(IAuth authRep, IAuthService authService)
        {
            _authRep = authRep;
            _authService = authService;
        }

        // POST api/<APIAuthController>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] Usuario user)
        {
            user.Contraseña = _authService.CreateHashPassword(user.Contraseña, out string salt);

            user.SalContraseña = salt;

            if(await _authRep.getByEmail(user.Email) != null && await _authRep.getByUsername(user.NombreUsuario) != null)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "No se pudo crear la cuenta.",
                    Details = "El nombre de usuario y correo electrónico ya existen."
                });
            }
            else if (await _authRep.getByEmail(user.Email) != null){

                return BadRequest(new
                {
                    Status = 400,
                    Message = "No se pudo crear la cuenta.",                    
                    Details = "El correo electrónico ya está registrado."
                });
            }
            else if (await _authRep.getByUsername(user.NombreUsuario) != null){

                return BadRequest(new
                {
                    Status = 400,                    
                    Message = "No se pudo crear la cuenta.",
                    Details = "El nombre de usuario está en uso."
                });
            }
            else
            {
                return Ok(new { result = await _authRep.Register(user) });

            }
        }

        [HttpPost]
        [Route("RegisterGoogle")]
        public async Task<ActionResult> RegisterGoogle([FromBody] Usuario user)
        {
            user.AutenticacionExternal = true;

            return Ok(new { result = await _authRep.Register(user) });
        }

        [HttpGet]
        [Route("GetByEmail/{email}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            var response = await _authRep.getByEmail(email);

            Usuario user = null;

            if (response != null)
            {
                user = new Usuario()
                {
                    NombreUsuario = response.NombreUsuario,
                    Email = response.Email,
                    Contraseña = response.Contraseña
                };
            }

            var jsonResponse = JsonConvert.SerializeObject(user);

            return Content(jsonResponse, "application/json");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginrequest)
        {
            Usuario user = await _authRep.getByUsername(loginrequest.Username);
            
            if (user == null)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Inicio de sesión fallido.",
                    Details = "El nombre de usuario no está registrado."
                });
            }
            else
            {
                if(_authService.VerifyPassword(loginrequest.Password, user.Contraseña, user.SalContraseña))
                {
                    UsuarioDto userDto = new UsuarioDto()
                    {
                        NombreUsuario = user.NombreUsuario,
                        Email = user.Email,
                        Nombre = user.Nombre,
                        Apellido = user.Apellido,
                        FotoPerfil = user.FotoPerfil,
                        Biografia = user.Biografia,
                        Ubicacion = user.Ubicacion,
                        Intereses = user.Intereses,
                        EsPremium = user.EsPremium
                    };

                    var jsonResponse = JsonConvert.SerializeObject(user);

                    return Content(jsonResponse, "application/json");
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = 400,
                        Message = "Contraseña incorrecta.",
                    });
                }
            }

        }

    }
}
