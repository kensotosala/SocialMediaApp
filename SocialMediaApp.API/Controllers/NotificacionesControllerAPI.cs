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

        //// Marca una notificación como leída
        //[HttpPut("{id}/marcar-leida")]
        //public async Task<IActionResult> MarcarNotificacionComoLeida(int id)
        //{
        //    await _repNotificaciones.MarcarNotificacionComoLeidaAsync(id);
        //    return NoContent();
        //}


        // Crear una notificación
        [HttpGet]
        [Route("ObtenerNotificaciones")]
        public async Task<ActionResult> ObtenerNotificaciones()
        {
            return Ok(new { resultado = await _repNotificaciones.obtenerTodo() });
        }

        // Crear una notificación
        [HttpPost]
        [Route("CrearNotificacion")]
        public async Task<ActionResult> CrearNotificacion([FromBody] Notificacione notificacion)
        {
            return Ok(new { resultado = await _repNotificaciones.insertar(notificacion) });
        }

        /*
        // GET: NotificacionesControllerAPI
        public ActionResult Index()
        {
            return View();
        }

        // GET: NotificacionesControllerAPI/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NotificacionesControllerAPI/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotificacionesControllerAPI/Create
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

        // GET: NotificacionesControllerAPI/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotificacionesControllerAPI/Edit/5
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

        // GET: NotificacionesControllerAPI/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NotificacionesControllerAPI/Delete/5
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