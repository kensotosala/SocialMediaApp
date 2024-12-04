using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;


namespace SocialMediaApp.Presentacion.Controllers
{
    public class Buscar_UsuariosController : Controller
    {
        private readonly IBuscar_Usuario _repBuscarUsuario;
        private readonly HttpClient _httpCliente;

        public Buscar_UsuariosController(IBuscar_Usuario repBuscar_Usuario,
                                          HttpClient httpCliente)
        {
            _repBuscarUsuario = repBuscar_Usuario;
            _httpCliente = httpCliente;
        }
        public async Task<IActionResult> Index()
        {
            //URL para usar la API
            string url = "http://localhost:5142/api/Buscar_UsuarioControllerAPI/ObtenerTodoslosUsuarios";

            //Realizar petición
            HttpResponseMessage res = await _httpCliente.GetAsync(url);

            //validar si la petición es exitosa
            if (res.IsSuccessStatusCode)
            {
                //deserializar los datos de json a objeto Usuario
                List<UsuarioDTO> usuarios = await res.Content.ReadFromJsonAsync<List<UsuarioDTO>>();
                return View(usuarios);
            }
            else
            {
                StatusCode((int)res.StatusCode, "Error al obtener datos del curso");
                return View();
            }
        }

        public async Task<IActionResult> BuscarUsuariosPorNombreAsync(string nombre)
        {
            // URL para usar la API y buscar por nombre
            string url = $"http://localhost:5142/api/Buscar_UsuarioControllerAPI/BuscarUsuariosPorNombre?nombre={nombre}";

            // Verificar si el parámetro 'nombre' está vacío
            if (string.IsNullOrWhiteSpace(nombre))
            {
                ViewBag.ErrorMessage = "Por favor, ingrese un nombre para buscar.";
                return View("NoResults"); // Vista para cuando no hay resultados o falta el parámetro
            }

            // Realizar la petición a la API
            HttpResponseMessage res = await _httpCliente.GetAsync(url);

            // Validar si la petición es exitosa
            if (res.IsSuccessStatusCode)
            {
                // Deserializar los datos de JSON a objeto UsuarioDTO
                List<UsuarioDTO> usuarios = await res.Content.ReadFromJsonAsync<List<UsuarioDTO>>();

                // Validar si hay resultados
                if (usuarios == null || !usuarios.Any())
                {
                    ViewBag.ErrorMessage = $"No se encontraron usuarios con el nombre '{nombre}'.";
                    return View("NoResults"); // Vista para cuando no hay coincidencias
                }

                return View(usuarios); // Mostrar la lista de usuarios encontrados
            }
            else
            {
                // Manejar errores de la API
                ViewBag.ErrorMessage = "Error al obtener datos del servidor.";
                return View("Error");
            }
        }
    }
}