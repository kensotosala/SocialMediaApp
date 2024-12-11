using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitarEventoControllerAPI : Controller
    {
        private readonly IInvitadosEvento _repInvitadoEvento;

        public InvitarEventoControllerAPI(IInvitadosEvento repInvitadoEvento)
        {
            _repInvitadoEvento = repInvitadoEvento;
        }

        // Invitar a un usuario a un evento
        [HttpPost("InvitarUsuario")]
        public async Task<IActionResult> InvitarUsuario([FromBody] InvitarUsuarioDTO invitacionDto)
        {
            try
            {
                if (invitacionDto.EventoId == null || invitacionDto.UsuarioId == null)
                {
                    return BadRequest(new { message = "EventoId y UsuarioId son requeridos." });
                }

                await _repInvitadoEvento.InvitarUsuarioAsync(invitacionDto.EventoId.Value, invitacionDto.UsuarioId.Value);
                return Ok(new { message = "Usuario invitado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Obtener todos los invitados a un evento
        [HttpGet("ObtenerInvitados/{eventoId}")]
        public async Task<ActionResult> ObtenerInvitados(int eventoId)
        {
            var invitados = await _repInvitadoEvento.ObtenerInvitadosPorEventoAsync(eventoId);
            var invitadosDto = invitados.Select(invitado => new InvitarUsuarioDTO
            {
                EventoId = invitado.EventoId,
                UsuarioId = invitado.UsuarioId,
                Confirmacion = invitado.Confirmacion,
                Evento = invitado.Evento,
                Usuario = invitado.Usuario
            }).ToList();

            return Ok(invitadosDto);
        }

        // Obtener una invitación específica
        [HttpGet("ObtenerInvitacion/{invitacionId}")]
        public async Task<ActionResult> ObtenerInvitacion(int invitacionId)
        {
            var invitacion = await _repInvitadoEvento.ObtenerInvitacionPorIdAsync(invitacionId);
            if (invitacion == null)
            {
                return NotFound(new { mensaje = "La invitación no fue encontrada." });
            }

            var invitacionDto = new InvitarUsuarioDTO
            {
                EventoId = invitacion.EventoId,
                UsuarioId = invitacion.UsuarioId,
                Confirmacion = invitacion.Confirmacion,
                Evento = invitacion.Evento,
                Usuario = invitacion.Usuario
            };

            return Ok(invitacionDto);
        }

        // Eliminar una invitación
        [HttpDelete("EliminarInvitacion/{invitacionId}")]
        public async Task<ActionResult> EliminarInvitacion(int invitacionId)
        {
            await _repInvitadoEvento.EliminarInvitacionAsync(invitacionId);
            return NoContent();
        }

        // Modificar el estado de una invitación
        [HttpPut("ModificarEstadoInvitacion/{invitacionId}")]
        public async Task<ActionResult> ModificarEstadoInvitacion(int invitacionId, [FromBody] string nuevoEstado)
        {
            await _repInvitadoEvento.ModificarEstadoInvitacionAsync(invitacionId, nuevoEstado);
            return Ok(new { mensaje = "Estado de la invitación actualizado." });
        }
    }
}
