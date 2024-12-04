using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Dominio.DTO;
using SocialMediaApp.Dominio.Interfaces;
using Newtonsoft.Json;
using SocialMediaApp.Persistencia.Data;


namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmistadControllerAPI : ControllerBase
    {
        private readonly IAmistad _repAmistad;
        public AmistadControllerAPI(IAmistad repAmistad)
        {
            _repAmistad = repAmistad;
        }

        // Obtiene las Amistades 
        [HttpGet]
        [Route("ObtenerTodaslasAmistades")]
        public async Task<ActionResult> ObtenerListaAmistadesAsync()
        {
            var amistad = await _repAmistad.ObtenerListaAmistadesAsync();
            var amistadDTO = amistad.Select(u => new AmistadDTO
            {
                AmistadId = u.AmistadId,
                UsuarioId = u.UsuarioId,
 
                Estado = u.Estado,
                FechaSolicitud = u.FechaSolicitud,
                FechaAceptacion = u.FechaAceptacion,
                Amigo = u.Amigo,
                Usuario = u.Usuario,

            }).ToList();
            var jsonRes = JsonConvert.SerializeObject(amistadDTO);
            return Content(jsonRes, "application/json");
            //return Ok(new { resultado=res});
        }
        // Endpoint para obtener los nombres de los amigos de un usuario
        [HttpGet]
        [Route("ObtenerNombresAmigos/{usuarioId}")]
        public async Task<ActionResult> ObtenerListaAmistadporNombreAsyn(int usuarioId)
        {
            var amistades = await _repAmistad.ObtenerListaAmistadporNombreAsyn();

            // Filtrar las amistades por el ID del usuario y por el estado "Aceptada"
            var amigos = amistades
            .Where(a =>
            (a.UsuarioId == usuarioId || a.AmigoId == usuarioId) &&
            a.Estado != null &&
            a.Estado.Trim().ToLower() == "aceptado")
            .Select(a =>
            a.UsuarioId == usuarioId ?
            a.Amigo?.NombreUsuario :
            a.Usuario?.NombreUsuario)
            .Distinct() // Evita nombres duplicados
            .ToList();

            if (!amigos.Any())
            {
                return NotFound($"No tienes amigos o no hay amistades aceptadas para el usuario con ID {usuarioId}.");
            }

            return Ok(amigos);
        }
    }
}
