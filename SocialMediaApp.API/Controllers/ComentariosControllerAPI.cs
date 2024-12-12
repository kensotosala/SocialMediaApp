using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosControllerAPI : Controller
    {
        private readonly IComentarios _repComentarios;
        public ComentariosControllerAPI(IComentarios repComentarios)
        {
            _repComentarios = repComentarios;
        }
        [HttpGet]
        [Route("ObtenerTodo")]
        public async Task<IActionResult> ObtenerTodo() 
        {
            var res = await _repComentarios.obtenerTodo();
            var dto = res.Select(c => new ComentariosDTO 
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
        [Route("insertarComentario")]
        public async Task<IActionResult> insertarComentario([FromBody] Comentario comentario)
        {
            var res = await _repComentarios.insertarComentario(comentario);
            return Ok(new { res });
        }
        [HttpPut]
        [Route("modificarComentario")]
        public async Task<IActionResult> modificarComentario([FromBody] Comentario comentario)
        {
            var res = await _repComentarios.modificarComentario(comentario);
            return Ok(new { res });
        }
        [HttpDelete]
        [Route("eliminarComentario")]
        public async Task<IActionResult> eliminarComentario([FromBody] Comentario comentario)
        {
            var res = await _repComentarios.eliminarComentario(comentario);
            return Ok(new { res });
        }
    }
}
