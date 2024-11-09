using Microsoft.AspNetCore.Http;
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
        [HttpGet("mis-eventos")]
        public async Task<IActionResult> ObtenerMisEventos()
        {
            var usuarioId = int.Parse(User.Identity.Name);
            var eventos = await _repEvento.ObtenerEventosParaUsuarioAsync(usuarioId);
            return Ok(eventos);
        }

        // Crea un nuevo evento
        [HttpPost]
        public async Task<IActionResult> CrearEvento(Evento evento)
        {
            await _repEvento.AgregarEventoAsync(evento);
            return CreatedAtAction(nameof(ObtenerMisEventos), new { id = evento.EventoId }, evento);
        }

        // Invita a un usuario a un evento
        [HttpPost("{eventoId}/invitar")]
        public async Task<IActionResult> InvitarUsuario(int eventoId, int usuarioId)
        {
            await _repEvento.InvitarUsuarioAsync(eventoId, usuarioId);
            return Ok();
        }

        // Confirma la asistencia de un usuario al evento
        [HttpPut("{eventoId}/confirmar")]
        public async Task<IActionResult> ConfirmarAsistencia(int eventoId, int usuarioId, [FromBody] string confirmacion)
        {
            await _repEvento.ConfirmarAsistenciaAsync(eventoId, usuarioId, confirmacion);
            return NoContent();
        }






        /*
        // GET: EventoControllerAPI
        public ActionResult Index()
        {
            return View();
        }

        // GET: EventoControllerAPI/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EventoControllerAPI/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventoControllerAPI/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventoControllerAPI/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventoControllerAPI/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventoControllerAPI/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventoControllerAPI/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
