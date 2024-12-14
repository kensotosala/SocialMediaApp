using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        // Listar Eventos
        [HttpGet("ListarInvitadoEvento")]
        public async Task<ActionResult> ListarInvitadoEvento()
        {
            var resultado = await _repInvitadoEvento.ListarInvitadosAEvento();

            var objDTO = resultado.Select(n => new InvitacionEventoDTO
            {
                InvitadoId = n.InvitadoId,
                EventoId = n.EventoId,
                UsuarioId = n.UsuarioId,
                Confirmacion = n.Confirmacion,
            }).ToList();

            var jsonRes = JsonConvert.SerializeObject(objDTO);

            return Content(jsonRes, "application/json");
        }

        // Invitar a un usuario a un evento
        [HttpPost("InvitarUsuario")]
        public async Task<IActionResult> InvitarUsuario([FromBody] InvitarUsuarioDTO invitarUsuarioDTO)
        {
            try
            {
                // Validar que EventoId y UsuarioId son requeridos
                if (invitarUsuarioDTO.EventoId == null || invitarUsuarioDTO.UsuarioId == null)
                {
                    return BadRequest(new { message = "EventoId y UsuarioId son requeridos." });
                }

                // Llamar al método de lógica de negocio para invitar al usuario
                await _repInvitadoEvento.InvitarUsuarioAsync(invitarUsuarioDTO.EventoId.Value, invitarUsuarioDTO.UsuarioId.Value);

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