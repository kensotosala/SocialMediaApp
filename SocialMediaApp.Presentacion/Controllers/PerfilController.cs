using Microsoft.AspNetCore.Mvc;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
