using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;
using Newtonsoft.Json;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Buscar_UsuarioControllerAPI : ControllerBase
    {
        private readonly IBuscar_Usuario _repBuscar_Usuario;
        public Buscar_UsuarioControllerAPI(IBuscar_Usuario repBuscar_Usuario)
        {
            _repBuscar_Usuario = repBuscar_Usuario;
        }

        // Obtiene los eventos de un usuario autenticado
        [HttpGet]
        [Route("ObtenerTodoslosUsuarios")]
        public async Task<ActionResult> ObtenerListaUsuariosAsync()
        {
            var usuarios = await _repBuscar_Usuario.ObtenerListaUsuariosAsync();
            var usuariosDTO = usuarios.Select(u => new UsuarioDTO
            {
                UsuarioId = u.UsuarioId,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Contraseña = u.Contraseña,
                FotoPerfil = u.FotoPerfil,
                Biografia = u.Biografia,
                Ubicacion = u.Ubicacion,
                Intereses = u.Intereses,
                EsPremium = u.EsPremium,
                FechaRegistro = u.FechaRegistro,

            }).ToList();
            var jsonRes = JsonConvert.SerializeObject(usuariosDTO);
            return Content(jsonRes, "application/json");
            //return Ok(new { resultado=res});
        }

        // Obtiene un evento específico
        // Busca usuarios por nombre
        [HttpGet]
        [Route("BuscarUsuariosPorNombre")]
        public async Task<ActionResult> BuscarUsuariosPorNombreAsync(string nombre)
        {
            // Filtrar los usuarios en el repositorio
            var usuarios = await _repBuscar_Usuario.ObtenerListaUsuariosAsync();
            var usuariosFiltrados = usuarios
                .Where(u => u.NombreUsuario.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .Select(u => new UsuarioDTO
                {
                    UsuarioId = u.UsuarioId,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Contraseña = u.Contraseña,
                    FotoPerfil = u.FotoPerfil,
                    Biografia = u.Biografia,
                    Ubicacion = u.Ubicacion,
                    Intereses = u.Intereses,
                    EsPremium = u.EsPremium,
                    FechaRegistro = u.FechaRegistro,
                })
                .ToList();

            // Convertir a JSON y devolver la respuesta
            var jsonRes = JsonConvert.SerializeObject(usuariosFiltrados);
            return Content(jsonRes, "application/json");
        }
    }
}
