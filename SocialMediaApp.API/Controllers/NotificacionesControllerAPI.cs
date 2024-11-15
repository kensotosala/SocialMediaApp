using Microsoft.AspNetCore.Mvc;
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
        //[HttpPut("{id}/marcar-leida")]
        //public async Task<IActionResult> MarcarNotificacionComoLeida(int id)
        //{
        //    await _repNotificaciones.MarcarNotificacionComoLeidaAsync(id);
        //    return NoContent();
        //}

        // Obtener todas las notificaciones
        [HttpGet("ObtenerNotificaciones")]
        public async Task<ActionResult> ObtenerNotificaciones()
        {
            return Ok(new { resultado = await _repNotificaciones.obtenerTodo() });
        }

        // Crear una notificación
        [HttpPost("CrearNotificacion")]
        public async Task<ActionResult> CrearNotificacion([FromBody] Notificacione notificacion)
        {
            return Ok(new { resultado = await _repNotificaciones.insertar(notificacion) });
        }
    }
}
