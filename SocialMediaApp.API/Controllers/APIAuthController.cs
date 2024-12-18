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


        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] SocialMediaApp.Persistencia.Data.ForgotPasswordRequest forgotPassRequest)
        {
            var userEmail = await _authRep.getByEmail(forgotPassRequest.Email);

            if (userEmail != null) 
            {
                if ((userEmail.AutenticacionExternal == true))
                {
                    return BadRequest(new
                    {
                        Message = "No se pudo recuperar la contraseña.",
                        Details = "Debes recuperar tu contraseña en tu cuenta de Google.."
                    });
                }
            }

            var usernameUser = await _authRep.getByUsername(forgotPassRequest.Username);

            if (usernameUser != null)
            {
                if (usernameUser.Email.Equals(forgotPassRequest.Email))
                {
                    var newUnhashedPw = GenerateSecurePassword();

                    usernameUser.Contraseña = _authService.CreateHashPassword(newUnhashedPw, out string salt);

                    usernameUser.SalContraseña = salt;

                    var resultado = await _authRep.ChangePassword(usernameUser);

                    if (resultado == 1)
                    {
                        return Ok(new
                        {
                            Message = newUnhashedPw,
                            Details = "Recuperación de contraseña exitosa"

                        });
                    }
                }
            }

            return BadRequest(new
            {
                Message = "No se pudo recuperar la contraseña.",
                Details = "Revise los datos ingresados."
            });
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

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] SocialMediaApp.Persistencia.Data.LoginRequest loginRequest)
        {
            Usuario newUser = new Usuario();

            newUser.NombreUsuario = loginRequest.Username;

            newUser.Contraseña = _authService.CreateHashPassword(loginRequest.Password, out string salt);

            newUser.SalContraseña = salt;

            return Ok(new { resultado = await _authRep.ChangePassword(newUser) });
        }
        public static string GenerateSecurePassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var random = Random.Shared; 
            var password = new char[16];

            password[0] = upperCase[random.Next(upperCase.Length)];
            password[1] = lowerCase[random.Next(lowerCase.Length)];
            password[2] = numbers[random.Next(numbers.Length)];
            password[3] = symbols[random.Next(symbols.Length)];

            string allChars = upperCase + lowerCase + numbers + symbols;
            for (int i = 4; i < 16; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }

            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (password[j], password[i]) = (password[i], password[j]);
            }

            return new string(password);
        }
    }
}
