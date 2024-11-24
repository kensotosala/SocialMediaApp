using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Presentacion.Controllers
{
    public class Buscar_UsuariosController : Controller
    {
        private readonly SocialMediaDBContext _dbContext;
        public Buscar_UsuariosController(SocialMediaDBContext context)
        {
            _dbContext = context;
        }
    }
        namespace SocialMediaApp.Presentacion.Controllers { 
    
        public class Buscar_UsuariosController : Controller
        {
            private readonly SocialMediaDBContext _dbContext;
            public Buscar_UsuariosController(SocialMediaDBContext context)
            {
                _dbContext = context;
            }

            //Get: Usuarios | Este get es para que el input de busqueda obtenga los datos segun el Nombre que se está buscando |
            public async Task<IActionResult> Index(string nombre)
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return View(new List<Usuario>());
                }

                var usuarios = await _dbContext.Usuarios
                    .Where(u => u.NombreUsuario.Contains(nombre))
                    .ToListAsync();

                if (usuarios.Count == 0)
                {
                    return View("NoResults"); // Redirige a la vista NoResults.cshtml
                }

                return View(usuarios); // Retorna los usuarios encontrados
            }
        }
    }

}



//previo al cambio final
//Get: Usuarios | Este get es para que el input de busqueda obtenga los datos segun el Nombre que se está buscando |
       /* public async Task<IActionResult> Index(string nombre)
        {
            if (nombre == null)
            {
                return NotFound();
            }


            // Busca usuarios cuyo nombre coincida (parcialmente o totalmente)
            var usuarios = await _dbContext.Usuarios
                .Where(u => u.NombreUsuario.Contains(nombre)) // Puedes ajustar esta consulta según tus necesidades
                .ToListAsync();

            if (usuarios == null || usuarios.Count == 0)
            {
                return View("NoResults"); // Si da tiempo, podríamos crear una vista específica para cuando no se encuentren resultados e iría aquí
            }

            return View(usuarios);
        }*/