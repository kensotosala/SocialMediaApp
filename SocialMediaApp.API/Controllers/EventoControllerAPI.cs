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

        public EventoControllerAPI(IEvento repEvento, INotificaciones repNotificacion)
        {
            _repEvento = repEvento;
            _repNotificacion = repNotificacion;
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

        // Endpoint para crear un evento
        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento([FromBody] CrearEventoDTO eventoDTO)
        {
            var evento = new Evento
            {
                UsuarioId = eventoDTO.UsuarioId ?? 1,
                Titulo = eventoDTO.Titulo,
                Descripcion = eventoDTO.Descripcion,
                FechaEvento = eventoDTO.FechaEvento,
                Ubicacion = eventoDTO.Ubicacion,
                FechaCreacion = DateTime.Now
            };

            await _repEvento.AgregarEventoAsync(evento);

            // Crear la notificación para el usuario asociado al evento (por ejemplo, el creador)
            var notificacion = new Notificacione
            {
                UsuarioId = eventoDTO.UsuarioId,  // Notificar al usuario que creó el evento
                Tipo = "Evento",  // Tipo de notificación
                Descripcion = $"Has creado un nuevo evento: {evento.Titulo}",  // Descripción de la notificación
                Fecha = DateTime.Now,  // Fecha actual
                EsLeida = false  // Estado de la notificación (no leída)
            };

            // Guardar la notificación para el usuario
            await _repNotificacion.insertar(notificacion);

            return CreatedAtAction(nameof(ObtenerEvento), new { id = evento.EventoId }, evento);
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

       

    }
}