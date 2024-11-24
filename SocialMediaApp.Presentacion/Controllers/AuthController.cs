using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Entidades;
using SocialMediaApp.Dominio.ViewModels;
using SocialMediaApp.Persistencia.Data;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                string url = "http://localhost:5142/api/APIAuth/Login";

                var loginRequest = new LoginRequest
                {
                    Username = user.NombreUsuario,
                    Password = user.Contraseña
                };

                var response = await _httpClient.PostAsJsonAsync(url, loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    UsuarioDto userDto = await response.Content.ReadFromJsonAsync<UsuarioDto>();

                    TempData["Mensaje"] = $"Bienvenido {user.NombreUsuario}.";
                    TempData["TipoMensaje"] = "alert-primary";

                    RegisterClaims(new UsuarioDto
                    {
                        NombreUsuario = userDto.NombreUsuario,
                        Email = userDto.Email
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                    TempData["Mensaje"] = errorResponse.Message + " " + errorResponse.Details;
                    TempData["TipoMensaje"] = "alert-danger";
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private async void RegisterClaims(UsuarioDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.NombreUsuario)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel user)
        {
            if (ModelState.IsValid)
            {                
                //TODO De vista
                user.Nombre = "";
                user.Apellido = "";

                string url = "http://localhost:5142/api/APIAuth/Register";

                string jsonData = JsonConvert.SerializeObject(user);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<List<int>>();

                    TempData["Mensaje"] = $"Bienvenido {user.NombreUsuario}.";
                    TempData["TipoMensaje"] = "alert-primary";

                    RegisterClaims(new UsuarioDto
                    {
                        NombreUsuario = user.NombreUsuario,
                        Email = user.Email
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                    TempData["Mensaje"] = errorResponse.Message + " " + errorResponse.Details;
                    TempData["TipoMensaje"] = "alert-danger";
                }        
            }
            return RedirectToAction("Register", "Auth");
        }

        public async Task<IActionResult> Logout(string username, string password)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
