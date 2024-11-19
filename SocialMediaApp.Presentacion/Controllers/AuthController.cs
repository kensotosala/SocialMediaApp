using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Persistencia.Data;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
                        TempData["Mensaje"] = $"Bienvenido {user.NombreUsuario}";
                        TempData["TipoMensaje"] = "alert-primary";

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Mensaje"] = "No se pudo crear la cuenta.";
                        TempData["TipoMensaje"] = "alert-danger";

                        return RedirectToAction("Register", "AuthController");
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
