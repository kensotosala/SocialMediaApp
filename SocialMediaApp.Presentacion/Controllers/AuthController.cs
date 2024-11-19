using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                string url = "http://localhost:5142/api/APIAuth/GetByUsername/" + user.NombreUsuario;

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

                if (userResult == null)
                {
                    TempData["Mensaje"] = "El nombre de usuario no está registrado.";
                    TempData["TipoMensaje"] = "alert-danger";
                }
                else
                {
                    if (userResult.Contraseña == user.Contraseña)
                    {
                        TempData["Mensaje"] = $"Bienvenido de nuevo {user.NombreUsuario}.";
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

        private async void RegisterClaims(Usuario user)
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
        public async Task<IActionResult> Register(UserRegisterViewModel user)
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

                        RegisterClaims(new Usuario
                        {
                            NombreUsuario = user.NombreUsuario,
                            Email = user.Email,
                            Contraseña = user.Contraseña,
                            FechaRegistro = DateTime.Now
                        });

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

            Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

            return (userResult != null);
        }

        [HttpGet]
        private async Task<bool> EmailExists(string email)
        {
            string url = "http://localhost:5142/api/APIAuth/GetByEmail/" + email;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

            return (userResult != null);
        }
    }
}
