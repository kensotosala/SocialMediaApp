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
    }
}