using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Entidades;
using SocialMediaApp.Dominio.ViewModels;
using SocialMediaApp.Persistencia.Data;
using System.Security.Claims;
using System.Security.Policy;
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
        public async Task<IActionResult> Profile()
        {

            string url = "http://localhost:5142/api/APIAuth/GetByUsername/" + User.FindFirst("UserName")?.Value;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Usuario userResult = await response.Content.ReadFromJsonAsync<Usuario>();

            var profileViewModel = new ProfileViewModel()
            {
                Nombre = userResult.Nombre,
                Apellido = userResult.Apellido,
                Biografia = userResult.Biografia,
                Ubicacion = userResult.Ubicacion,
                Email = userResult.Email
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel updates)
        {
            var url = "http://localhost:5142/api/APIAuth/ChangeProfile";

            var profileUpdates = new Profile()
            {
                NombreUsuario = User.FindFirst("UserName")?.Value,
                Nombre = updates.Nombre,
                Apellido = updates.Apellido,
                Ubicacion= updates.Ubicacion,
                Biografia= updates.Biografia,
                Email = updates.Email
            };

            string jsonData = JsonConvert.SerializeObject(profileUpdates);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Los cambios guardados correctamente";
                TempData["TipoMensaje"] = "alert-primary";

                await RegisterClaims(new UsuarioDto
                {
                    Nombre = profileUpdates.Nombre,
                    Email = profileUpdates.Email,
                    NombreUsuario = profileUpdates.NombreUsuario,
                    AutenticacionExternal = (User.FindFirst("ExternalLogin")?.Value  == "1")
                });

            }
            else
            {
                TempData["Mensaje"] = "No se pudo actualizar el perfil.";
                TempData["TipoMensaje"] = "alert-danger";
            }

            return RedirectToAction("Profile", "Auth");
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

                var loginRequest = new SocialMediaApp.Persistencia.Data.LoginRequest
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
            int external = 0;

            if (user.AutenticacionExternal)
                external = 1;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Nombre),
                new Claim("UserName", user.NombreUsuario),
                new Claim("ExternalLogin", external.ToString())
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewBag.Preguntas = ObtenerPreguntas();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(SocialMediaApp.Persistencia.Data.ForgotPasswordRequest forgotPwRequest)
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
            ViewBag.Preguntas = ObtenerPreguntas();

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

        public async Task<IActionResult> Logout(string username, string password)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private List<SelectListItem> ObtenerPreguntas()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Seleccione una pregunta de seguridad", Value = "", Disabled = true, Selected = true },
                    new SelectListItem { Text = "¿Cómo se llama tu ciudad natal?", Value = "¿Cómo se llama tu ciudad natal?" },
                    new SelectListItem { Text = "¿Cuál es tu comida favorita?", Value = "¿Cuál es tu comida favorita?" },
                    new SelectListItem { Text = "¿Cómo se llamaba tu primer maestro?", Value = "¿Cómo se llamaba tu primer maestro?" },
                    new SelectListItem { Text = "¿Cuál es el nombre de tu película favorita?", Value = "¿Cuál es el nombre de tu película favorita?" },
                    new SelectListItem { Text = "¿Qué deporte te gusta más??", Value = "¿Qué deporte te gusta más?" }
                };
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeSecurity()
        {
            ViewBag.Preguntas = ObtenerPreguntas();

            return View(new ChangeSecurityViewModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangeSecurityViewModel request)
        {
            var loginRequest = new SocialMediaApp.Persistencia.Data.LoginRequest()
            {
                Username = User.FindFirst("UserName")?.Value,
                Password = request.PasswordChange.password
            };

            string url = "http://localhost:5142/api/APIAuth/Login";

            var response = await _httpClient.PostAsJsonAsync(url, loginRequest);

            if (response.IsSuccessStatusCode)
            {
                url = "http://localhost:5142/api/APIAuth/ChangePassword";

                loginRequest.Password = request.PasswordChange.newPassword;

                string jsonData = JsonConvert.SerializeObject(loginRequest);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Cambiaste tu contraseña.";
                    TempData["TipoMensaje"] = "alert-primary";
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                    TempData["Mensaje"] = "No se pudo cambiar la contraseña.";
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

            return RedirectToAction("Profile", "Auth");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeSecurityQuestion(ChangeSecurityViewModel request)
        {

            var url = "http://localhost:5142/api/APIAuth/ChangeQuestion";

            var changeQuestionRequest = new ChangeQuestionRequest()
            {
                Username = User.FindFirst("UserName")?.Value,
                Pregunta = request.QuestionChange.Pregunta,
                Respuesta = request.QuestionChange.Respuesta,
            };

            string jsonData = JsonConvert.SerializeObject(changeQuestionRequest);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Cambiaste tu pregunta de seguridad.";
                TempData["TipoMensaje"] = "alert-primary";
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                TempData["Mensaje"] = "No se pudo cambiar pregunta de seguridad.";
                TempData["TipoMensaje"] = "alert-danger";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

