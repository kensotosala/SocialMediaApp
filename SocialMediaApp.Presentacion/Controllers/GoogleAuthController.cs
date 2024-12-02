using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using SocialMediaApp.Persistencia.Data;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using SocialMediaApp.Dominio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            if (await EmailExists(user.Email))
            {
                TempData["Mensaje"] = $"Bienvenido {user.Nombre}.";
                TempData["TipoMensaje"] = "alert-primary";

                await RegisterClaims(new UsuarioDto
                {
                    NombreUsuario = user.NombreUsuario,
                    Nombre = user.Nombre,
                    Email = user.Email
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return await RegisterGoogle(user);
            }            
        }

        [HttpGet]
        private async Task<bool> EmailExists(string email)
        {
            string url = "http://localhost:5142/api/APIAuth/GetByEmail/" + email;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

            bool result = (userResult != null);
            return result;
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
                        Email = user.Email
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task RegisterClaims(UsuarioDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Nombre)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }
    }
}
;