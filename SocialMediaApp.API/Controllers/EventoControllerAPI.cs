using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoControllerAPI : Controller
    {
        private readonly IEvento _repEvento;
        private readonly INotificaciones _repNotificacion;
        private readonly IInvitadosEvento _repInvEvento;

        public EventoControllerAPI(IEvento repEvento, INotificaciones repNotificacion, IInvitadosEvento repInvEvento)
        {
            _repEvento = repEvento;
            _repNotificacion = repNotificacion;
            _repInvEvento = repInvEvento;
        }

        // Obtiene los eventos de un usuario autenticado
        [HttpGet("obtenerTodosLosEventos")]
        public async Task<IActionResult> ObtenerMisEventos()
        {
            var resultado = await _repEvento.ObtenerEventoAsync();

            var eventoDTO = resultado.Select(n => new EventoDTO
            {
                EventoId = n.EventoId,
                UsuarioId = n.UsuarioId,
                Titulo = n.Titulo,
                Descripcion = n.Descripcion,
                FechaEvento = n.FechaEvento,
                Ubicacion = n.Ubicacion,
                FechaCreacion = n.FechaCreacion,
            }).ToList();

            var jsonRes = JsonConvert.SerializeObject(eventoDTO);

            return Content(jsonRes, "application/json");
        }

        /*
        ENDPOINT PARA CREAR UN EVENTO
         */

        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento([FromBody] CrearEventoDTO eventoDTO)
        {
            try
            {
                // More comprehensive validation
                if (eventoDTO == null)
                {
                    return BadRequest(new { errors = "Los datos del evento no pueden ser nulos" });
                }

                // Detailed validation with specific error messages
                var validationErrors = new List<string>();

                if (string.IsNullOrWhiteSpace(eventoDTO.Titulo))
                    validationErrors.Add("El título del evento es obligatorio");

                if (string.IsNullOrWhiteSpace(eventoDTO.Descripcion))
                    validationErrors.Add("La descripción del evento es obligatoria");

                if (eventoDTO.FechaEvento == default)
                    validationErrors.Add("La fecha del evento no es válida");

                if (string.IsNullOrWhiteSpace(eventoDTO.Ubicacion))
                    validationErrors.Add("La ubicación del evento es obligatoria");

                // If there are validation errors, return them
                if (validationErrors.Any())
                {
                    return BadRequest(new { errors = validationErrors });
                }

                // Create the event
                var evento = new Evento
                {
                    UsuarioId = eventoDTO.UsuarioId ?? 1, // Default user ID
                    Titulo = eventoDTO.Titulo,
                    Descripcion = eventoDTO.Descripcion,
                    FechaEvento = eventoDTO.FechaEvento,
                    Ubicacion = eventoDTO.Ubicacion,
                    FechaCreacion = DateTime.Now
                };

                // Save the event
                await _repEvento.AgregarEventoAsync(evento);

                // Create notification for event creator
                var notificacion = new Notificacione
                {
                    UsuarioId = evento.UsuarioId,
                    Tipo = "Evento",
                    Descripcion = $"Has creado un nuevo evento: {evento.Titulo}",
                    Fecha = DateTime.Now,
                    EsLeida = false
                };
                await _repNotificacion.insertar(notificacion);

                int ultimoEventoId = await _repEvento.ObtenerUltimoEventoId();

                // Invite users and create notifications
                if (eventoDTO.UsuarioIds != null && eventoDTO.UsuarioIds.Any())
                {
                    foreach (var usuarioId in eventoDTO.UsuarioIds)
                    {
                        // Invite user to the event
                        await _repInvEvento.InvitarUsuarioAsync(ultimoEventoId, usuarioId);

                        // Create invitation notification for each invited user
                        var invitacionNotificacion = new Notificacione
                        {
                            UsuarioId = usuarioId,
                            Tipo = "Invitación a Evento",
                            Descripcion = $"Has sido invitado al evento: {evento.Titulo}",
                            Fecha = DateTime.Now,
                            EsLeida = false
                        };
                        await _repNotificacion.insertar(invitacionNotificacion);
                    }
                }

                // Return the created event
                return CreatedAtAction(nameof(ObtenerEvento), new { id = evento.EventoId }, evento);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error creating event");

                return StatusCode(500, new
                {
                    errors = "Error interno al crear el evento",
                    details = ex.Message
                });
            }
        }

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

        [HttpPut("ModificarInvEventoXID")]
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
        [HttpDelete("EliminarEventoXID")]
        public async Task<IActionResult> EliminarEvento(int eventoId)
        {
            await _repEvento.EliminarEventoAsync(eventoId);
            return NoContent();
        }
    }
}