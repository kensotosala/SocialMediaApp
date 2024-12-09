using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoControllerAPI : Controller
    {
        private readonly IEvento _repEvento;

        public EventoControllerAPI(IEvento repEvento)
        {
            _repEvento = repEvento;
        }

        // Obtiene los eventos de un usuario autenticado
        [HttpGet("obtnerTodosLosEventos")]
        public async Task<IActionResult> ObtenerMisEventos()
        {
            var eventos = await _repEvento.ObtenerEventoAsync();
            return Ok(eventos);
        }

        // Crea un nuevo evento
        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento(Evento evento)
        {
            await _repEvento.AgregarEventoAsync(evento);
            return CreatedAtAction(nameof(ObtenerMisEventos), new { id = evento.EventoId }, evento);
        }

        // Obtiene un evento específico
        [HttpGet("ObtenerEventosPorID")]
        public async Task<IActionResult> ObtenerEvento(int eventoId)
        {
            var evento = await _repEvento.ObtenerEventosParaUsuarioAsync(eventoId);
            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }

        // Modifica un evento existente
        [HttpPut("ModificarXID")]
        public async Task<IActionResult> ModificarEvento(int eventoId, [FromBody] Evento evento)
        {
            if (eventoId != evento.EventoId)
            {
                return BadRequest();
            }

            await _repEvento.ModificarEventoAsync(evento);
            return NoContent();
        }

        // Elimina un evento
        [HttpDelete("EliminarXID")]
        public async Task<IActionResult> EliminarEvento(int eventoId)
        {
            await _repEvento.EliminarEventoAsync(eventoId);
            return NoContent();
        }

        [HttpPost("invitar al usuario")]
        public async Task<IActionResult> InvitarUsuario(int eventoId, int usuarioId)
        {
            try
            {
                await _repEvento.InvitarUsuarioAsync(eventoId, usuarioId);
                return Ok(new { message = "Usuario invitado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("ObtenerInvitados/{eventoId}")]
        public async Task<ActionResult> ObtenerInvitados(int eventoId)
        {
            var invitados = await _repEvento.ObtenerInvitadosPorEventoAsync(eventoId);
            return Ok(invitados);
        }
        // Obtiene la información de un invitado por ID de invitación
        [HttpGet("ObtenerInvitacion/{invitacionId}")]
        public async Task<ActionResult> ObtenerInvitacion(int invitacionId)
        {
            var invitacion = await _repEvento.ObtenerInvitacionPorIdAsync(invitacionId);
            if (invitacion == null)
            {
                return NotFound(new { mensaje = "La invitación no fue encontrada." });
            }
            return Ok(invitacion);
        }
        // Elimina una invitación por ID
        [HttpDelete("EliminarInvitacion/{invitacionId}")]
        public async Task<ActionResult> EliminarInvitacion(int invitacionId)
        {
            await _repEvento.EliminarInvitacionAsync(invitacionId);
            return NoContent();
        }

        // Modifica el estado de una invitación
        [HttpPut("ModificarEstadoInvitacion/{invitacionId}")]
        public async Task<ActionResult> ModificarEstadoInvitacion(int invitacionId, [FromBody] string nuevoEstado)
        {
            await _repEvento.ModificarEstadoInvitacionAsync(invitacionId, nuevoEstado);
            return Ok(new { mensaje = "Estado de la invitación actualizado." });
        }

    }
}