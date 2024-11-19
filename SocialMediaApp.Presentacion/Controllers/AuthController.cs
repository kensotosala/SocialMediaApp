using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Persistencia.Data;
using System.Linq.Expressions;
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
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                string url = "http://localhost:5142/api/APIAuth/GetByUsername/" + username;

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                UserDto userResult = await response.Content.ReadFromJsonAsync<UserDto>();

                if (userResult == null)
                {
                    TempData["Mensaje"] = "El nombre de usuario no está registrado.";
                    TempData["TipoMensaje"] = "alert-danger";
                }
                else
                {
                    if (userResult.Contraseña == password)
                    {
                        TempData["Mensaje"] = $"Bienvenido de nuevo {username}.";
                        TempData["TipoMensaje"] = "alert-primary";

                        RegisterClaims(userResult);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Mensaje"] = "Contraseña incorrecta.";
                        TempData["TipoMensaje"] = "alert-danger";
                    }
                }
            }
            return RedirectToAction("Login", "Auth");
        }

        private async void RegisterClaims(UserDto userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.NombreUsuario)
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
        public async Task<IActionResult> Register(Usuario user)
        {
            if (ModelState.IsValid)
            {
                bool usernameExists = await UsernameExists(user.NombreUsuario);

                bool emailExists = await EmailExists(user.Email);

                if (!usernameExists && !emailExists)
                {
                    string url = "http://localhost:5142/api/APIAuth/Register";

                    string jsonData = JsonConvert.SerializeObject(user);

                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                    HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Mensaje"] = $"Bienvenido {user.NombreUsuario}.";
                        TempData["TipoMensaje"] = "alert-primary";

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Mensaje"] = "No se pudo crear la cuenta.";
                        TempData["TipoMensaje"] = "alert-danger";
                    }
                }
                else
                {
                    if (usernameExists && emailExists)
                        TempData["Mensaje"] = "El nombre de usuario y correo electrónico ya existen.";
                    else if (usernameExists)
                        TempData["Mensaje"] = "El nombre de usuario está en uso.";
                    else if (emailExists)
                        TempData["Mensaje"] = "El correo electrónico ya está registrado.";

                    TempData["TipoMensaje"] = "alert-danger";
                }            
            }
            return RedirectToAction("Register", "Auth");
        }

        [HttpGet]
        private async Task<bool> UsernameExists(string username)
        {
            string url = "http://localhost:5142/api/APIAuth/GetByUsername/" + username;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            UserDto userResult = await response.Content.ReadFromJsonAsync<UserDto>();

            return (userResult != null);
        }

        [HttpGet]
        private async Task<bool> EmailExists(string email)
        {
            string url = "http://localhost:5142/api/APIAuth/GetByEmail/" + email;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            UserDto userResult = await response.Content.ReadFromJsonAsync<UserDto>();

            return (userResult != null);
        }
    }
}
