using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System.Threading.Tasks;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEvento _eventoRepositorio;

        public EventosController(IEvento eventoRepositorio)
        {
            _eventoRepositorio = eventoRepositorio;
        }

        // GET: Eventos/MisEventos
        public async Task<IActionResult> MisEventos()
        {
            int usuarioId = int.Parse(User.Identity.Name);
            var eventos = await _eventoRepositorio.ObtenerEventosParaUsuarioAsync(usuarioId);
            return View(eventos);
        }

        // GET: Eventos/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Eventos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Evento evento)
        {
            if (ModelState.IsValid)
            {
                await _eventoRepositorio.AgregarEventoAsync(evento);
                return RedirectToAction(nameof(MisEventos));
            }
            return View(evento);
        }

        // GET: Eventos/Invitar
        public IActionResult Invitar(int eventoId)
        {
            ViewData["EventoId"] = eventoId;
            return View();
        }

        // POST: Eventos/Invitar
        //[HttpPost]
        //[ValidateAntiForgeryToken]
       /* public async Task<IActionResult> Invitar(int eventoId, int usuarioId)
        {
            await _eventoRepositorio.InvitarUsuarioAsync(eventoId, usuarioId);
            return RedirectToAction(nameof(MisEventos));
        }*/

        // GET: Eventos/ConfirmarAsistencia
        //public IActionResult ConfirmarAsistencia(int eventoId)
        //{
        //    ViewData["EventoId"] = eventoId;
        //    return View();
        //}

        // POST: Eventos/ConfirmarAsistencia
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ConfirmarAsistencia(int eventoId, int usuarioId, string confirmacion)
        //{
        //    await _eventoRepositorio.ConfirmarAsistenciaAsync(eventoId, usuarioId, confirmacion);
        //    return RedirectToAction(nameof(MisEventos));
        //}
    }
}
