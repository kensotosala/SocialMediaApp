using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionesControllerAPI : Controller
    {
        private readonly IPublicaciones _repPublicaciones;

        public PublicacionesControllerAPI(IPublicaciones repPublicaciones)
        {
            _repPublicaciones = repPublicaciones;
        }
        [HttpGet]
        [Route("ObtenerTodo")]
        public async Task<IActionResult> ObtenerTodo() 
        {
            var res = await _repPublicaciones.obtenerTodo();
            var dto = res.Select(p => new PublicacionesDTO
            {
                publicacionID = p.PublicacionId,
                usuarioID = (int)p.UsuarioId,
                publicacion = p.Texto,
                imagen = p.Imagen,
                enlace = p.Enlace,
                Fecha = (DateTime)p.FechaPublicacion
            }).ToList();
            var jsonRes = JsonConvert.SerializeObject(dto);
            return Content(jsonRes, "aplication/json");
        }

        [HttpGet]
        [Route("ObtenerPublicacionesxID")]
        public async Task<IActionResult> obtenerPublicacionxID([FromQuery] int publicacionID)
        {
            var res = await _repPublicaciones.obtenerPublicacionxID(publicacionID);
            return Ok(new { res });
        }

        [HttpGet]
        [Route("obtenerComentariosxPublicacionId")]
        public async Task<IActionResult> obtenerComentariosxPublicacionId(int publicacionID)
        {
            var res = await _repPublicaciones.obtenerComentariosxPublicacionId(publicacionID);
            var dto = res.SelectMany(p => p.Comentarios).Select(static c => new ComentariosDTO
            {
                comentarioID = c.ComentarioId,
                publicacionID = (int)c.PublicacionId,
                usuarioID = (int)c.UsuarioId,
                comentario = c.Texto,
                Fecha = (DateTime)c.FechaComentario
            }).ToList();
            var jsonRes = JsonConvert.SerializeObject(dto);
            return Content(jsonRes, "aplication/json"); 
        }

        [HttpPost]
        [Route("insertarPublicacion")]
        public async Task<IActionResult> insertarPublicacion([FromBody] Publicacione publicacion) 
        { 
            var res = await _repPublicaciones.insertarPublicacion(publicacion);
            return Ok(new { res });
        }
        [HttpPut]
        [Route("ModificarPublicacion")]
        public async Task<IActionResult> ModificarPublicacion([FromBody] Publicacione publicacion)
        {
            var res = await _repPublicaciones.ModificarPublicacion(publicacion);
            return Ok(new { res });
        }
        [HttpDelete]
        [Route("eliminarPublicacion")]
        public async Task<IActionResult> eliminarPublicacion([FromBody] Publicacione publicacion)
        {
            var res = await _repPublicaciones.eliminarPublicacion(publicacion);
            return Ok(new { res });
        }
    }
}
