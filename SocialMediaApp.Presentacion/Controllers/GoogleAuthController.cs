using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using SocialMediaApp.Persistencia.Data;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using SocialMediaApp.Dominio.ViewModels;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class GoogleAuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public GoogleAuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var user = new UserLoginGoogleViewModel
            {
                NombreUsuario = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                Nombre = result.Principal.FindFirstValue(ClaimTypes.GivenName),
                Apellido = result.Principal.FindFirstValue(ClaimTypes.Surname),
                Email = result.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var userDb = await EmailExists(user.Email);

            if (userDb == null)
            {
                return await RegisterGoogle(user);
            }
            else
            {
                if (await GoogleAccountExists(user.NombreUsuario, userDb.NombreUsuario))
                {
                    TempData["Mensaje"] = $"Bienvenido {user.Nombre}.";
                    TempData["TipoMensaje"] = "alert-primary";

                    await RegisterClaims(new UsuarioDto
                    {
                        NombreUsuario = user.NombreUsuario,
                        Nombre = user.Nombre,
                        Email = user.Email,
                        AutenticacionExternal = true
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    TempData["Mensaje"] = "Este correo electrónico está en uso.";
                    TempData["TipoMensaje"] = "alert-danger";

                    return RedirectToAction("Login", "Auth");
                }
            }            
        }

        [HttpGet]
        private async Task<Usuario> EmailExists(string email)
        {
            string url = "http://localhost:5142/api/APIAuth/GetByEmail/" + email;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

            return userResult;
        }

        // Google user exists
        private async Task<bool> GoogleAccountExists(string usernameResponse, string usernameInput)
        {
            return (usernameInput.Equals(usernameResponse));
        }


        public async Task<IActionResult> RegisterGoogle(UserLoginGoogleViewModel user)
        {

            if (ModelState.IsValid)
            {

                string url = "http://localhost:5142/api/APIAuth/RegisterGoogle";

                string jsonData = JsonConvert.SerializeObject(user);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                var test = await _httpClient.PostAsync(url, content);

                HttpResponseMessage response = test;
              
                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = $"Bienvenido {user.Nombre}.";
                    TempData["TipoMensaje"] = "alert-primary";

                    await RegisterClaims(new UsuarioDto
                    {
                        NombreUsuario = user.NombreUsuario,
                        Nombre = user.Nombre,
                        Email = user.Email,
                        AutenticacionExternal = true
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task RegisterClaims(UsuarioDto user)
        {
            int external = 0;

            if (user.AutenticacionExternal)
                external = 1;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Nombre),
                new Claim("ExternalLogin", external.ToString()),
                new Claim("UserName", user.NombreUsuario)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }
    }
}
;