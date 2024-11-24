using Microsoft.AspNetCore.Mvc;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class Mostrar_AmigosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
