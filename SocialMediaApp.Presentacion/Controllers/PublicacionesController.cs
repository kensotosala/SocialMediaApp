using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Dominio.Interfaces;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly IPublicaciones _publicaciones;
        private readonly IComentarios _comentarios;
        private readonly HttpClient _httpClient;

        public PublicacionesController(IPublicaciones publicaciones, IComentarios comentarios, HttpClient httpClient)
        {
            _publicaciones = publicaciones;
            _comentarios = comentarios;
            _httpClient = httpClient;
        }
        // GET: PublicacionesController
        public async Task <ActionResult> Index()
        {
            string  URL= "http://localhost:5142/api/PublicacionesControllerAPI/ObtenerTodo";
            string URL2 = "http://localhost:5142/api/ComentariosControllerAPI/ObtenerTodo";
            HttpResponseMessage Request = await _httpClient.GetAsync(URL);
            HttpResponseMessage Request2 = await _httpClient.GetAsync(URL2);

            if (Request.IsSuccessStatusCode && Request2.IsSuccessStatusCode){
            List<PublicacionesDTO> publicaciones = await Request.Content.ReadFromJsonAsync<List<PublicacionesDTO>>();
            List<ComentariosDTO> comentarios = await Request2.Content.ReadFromJsonAsync<List<ComentariosDTO>>();

                var tupla = new Tuple<List<PublicacionesDTO>, List<ComentariosDTO>>(publicaciones,comentarios);
                return View(tupla);
            }
            else
            {
                return View();
            }

        }

        // GET: PublicacionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PublicacionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicacionesController/Create
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

        // GET: PublicacionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PublicacionesController/Edit/5
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

        // GET: PublicacionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PublicacionesController/Delete/5
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
        }
    }
}
