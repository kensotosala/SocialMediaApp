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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                    TempData["Mensaje"] = $"¡Te damos la bienvenida {userDto.Nombre}!";
                    TempData["TipoMensaje"] = "alert-primary";

                    await RegisterClaims(new UsuarioDto
                    {
                        Nombre = userDto.Nombre,
                        Email = userDto.Email,
                        AutenticacionExternal = false,
                        NombreUsuario = userDto.NombreUsuario
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

        private async Task RegisterClaims(UsuarioDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Nombre),
                new Claim("UserName", user.NombreUsuario)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPwRequest)
        {
            if (ModelState.IsValid)
            {
                string url = "http://localhost:5142/api/APIAuth/ForgotPassword";

                string jsonData = JsonConvert.SerializeObject(forgotPwRequest);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                var response = await _httpClient.PutAsync(url, content);

                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                    if (response.IsSuccessStatusCode)
                    {                       
                        TempData["Mensaje"] = $"Tu nueva contraseña es " + errorResponse.Message;
                        TempData["TipoMensaje"] = "alert-primary";

                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        TempData["Mensaje"] = errorResponse.Message + " " + errorResponse.Details;
                        TempData["TipoMensaje"] = "alert-danger";

                        return RedirectToAction("ForgotPassword", "Auth");
                    }               
            }
            return RedirectToAction("ForgotPassword", "Auth");
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
                string url = "http://localhost:5142/api/APIAuth/Register";

                string jsonData = JsonConvert.SerializeObject(user);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = $"¡Te damos la bienvenida {user.Nombre}!";
                    TempData["TipoMensaje"] = "alert-primary";

                    await RegisterClaims(new UsuarioDto
                    {
                        NombreUsuario = user.NombreUsuario,
                        Nombre = user.Nombre,
                        Apellido = user.Apellido,
                        Email = user.Email,
                        AutenticacionExternal = false
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                var loginRequest = new LoginRequest()
                {
                    Username = User.FindFirst("UserName")?.Value,
                    Password = profile.password
                };

                string url = "http://localhost:5142/api/APIAuth/Login";

                var response = await _httpClient.PostAsJsonAsync(url, loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    url = "http://localhost:5142/api/APIAuth/ChangePassword";

                    loginRequest.Password = profile.newPassword;

                    string jsonData = JsonConvert.SerializeObject(loginRequest);

                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                    response = await _httpClient.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Mensaje"] = $"Cambiaste tu contraseña.";
                        TempData["TipoMensaje"] = "alert-primary";
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                        TempData["Mensaje"] = "No se pudo cambiar la contraseña";
                        TempData["TipoMensaje"] = "alert-danger";
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                    TempData["Mensaje"] = errorResponse.Message + " " + errorResponse.Details;
                    TempData["TipoMensaje"] = "alert-danger";
                }
            }
            return RedirectToAction("Profile", "Auth");
        }

        public async Task<IActionResult> Logout(string username, string password)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
