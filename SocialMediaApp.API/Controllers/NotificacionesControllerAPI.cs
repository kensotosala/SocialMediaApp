using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesControllerAPI : Controller
    {
        private readonly INotificaciones _repNotificaciones;

        public NotificacionesControllerAPI(INotificaciones repNotificaciones)
        {
            _repNotificaciones = repNotificaciones;
        }

        // Marca una notificación como leída
        [HttpPut("{id}/marcar-leida")]
        public async Task<IActionResult> MarcarNotificacionComoLeida(int id)
        {
            await _repNotificaciones.MarcarNotificacionComoLeidaAsync(id);
            return NoContent();
        }

        // Obtener todas las notificaciones
        [HttpGet("ObtenerNotificaciones")]
        public async Task<ActionResult> ObtenerNotificaciones()
        {
            var resultado = await _repNotificaciones.obtenerTodo();

            var notificacionDTO = resultado.Select(n => new NotificacionDTO
            {
                NotificacionId = n.NotificacionId,
                UsuarioId = n.UsuarioId,
                Tipo = n.Tipo,
                Descripcion = n.Descripcion,
                Fecha = n.Fecha,
                EsLeida = n.EsLeida,
            }).ToList();

            var jsonRes = JsonConvert.SerializeObject(notificacionDTO);
            return Content(jsonRes, "application/json");
        }

        // Crear una notificación
        [HttpPost("CrearNotificacion")]
        public async Task<ActionResult> CrearNotificacion([FromBody] Notificacione notificacion)
        {
            return Ok(new { resultado = await _repNotificaciones.insertar(notificacion) });
        }
    }
}