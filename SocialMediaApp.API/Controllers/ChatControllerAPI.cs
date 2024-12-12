using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System.Linq;

namespace SocialMediaApp.API.Controllers
{
    public class ChatControllerAPI : Controller
    {
        private readonly IChat _repoChat;

        public ChatControllerAPI(IChat repoChat)
        {
            _repoChat = repoChat;
        }

        [HttpPost]
        [Route("EnviarMensaje")]
        public async Task<IActionResult> EnviarMensaje(int emisorId, int receptorId, string mensajeTexto)
        {
            await _repoChat.EnviarMensajeAsync(emisorId, receptorId, mensajeTexto);
            return Ok(new { message = "Mensaje enviado." });
        }

        [HttpGet]
        [Route("ObtenerMensajesEntreUsuarios")]
        public async Task<IActionResult> ObtenerMensajesEntreUsuarios(int usuario1Id, int usuario2Id)
        {
            var mensajes = await _repoChat.ObtenerMensajesEntreUsuariosAsync(usuario1Id, usuario2Id);
            return Ok(mensajes.Select(m => new
            {
                m.MensajeId,
                m.EmisorId,
                m.ReceptorId,
                m.MensajeTexto,
                m.FechaEnvio,
                m.EsLeido
            }));
        }

        [HttpGet]
        [Route("ObtenerMensajesNoLeidos")]
        public async Task<IActionResult> ObtenerMensajesNoLeidos(int receptorId)
        {
            var mensajesNoLeidos = await _repoChat.ObtenerMensajesNoLeidosAsync(receptorId);
            return Ok(mensajesNoLeidos.Select(m => new
            {
                m.MensajeId,
                m.EmisorId,
                m.ReceptorId,
                m.MensajeTexto,
                m.FechaEnvio,
                m.EsLeido
            }));
        }

        [HttpPut]
        [Route("MarcarMensajesComoLeidos")]
        public async Task<IActionResult> MarcarMensajesComoLeidos(int receptorId, int emisorId)
        {
            await _repoChat.MarcarMensajesComoLeidosAsync(receptorId, emisorId);
            return Ok(new { message = "Mensaje(s) leido(s)." });
        }
    }
}
