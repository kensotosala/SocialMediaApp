using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Dominio.Dto;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialMediaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIAuthController : ControllerBase
    {
        private readonly IAuth _authRep;

        public APIAuthController(IAuth authRep)
        {
            _authRep = authRep;
        }

        // GET: api/<APIAuthController>
        [HttpGet]
        [Route("GetByUsername/{username}")]
        public async Task<ActionResult> GetByUsername(string username)
        {
            var response = await _authRep.getByUsername(username);
            
            UserDto userDto = null;

            if (response != null)
            {
                userDto = new UserDto()
                {
                    NombreUsuario = response.NombreUsuario,
                    Email = response.Email,
                    Contraseña = response.Contraseña
                };
            }

            var jsonResponse = JsonConvert.SerializeObject(userDto);

            return Content(jsonResponse, "application/json");
        }

        // GET: api/<APIAuthController>
        [HttpGet]
        [Route("GetByEmail/{email}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            var response = await _authRep.getByEmail(email);

            UserDto userDto = null;

            if (response != null)
            {
                userDto = new UserDto()
                {
                    NombreUsuario = response.NombreUsuario,
                    Email = response.Email,
                    Contraseña = response.Contraseña
                };
            }

            var jsonResponse = JsonConvert.SerializeObject(userDto);

            return Content(jsonResponse, "application/json");
        }

        // GET api/<APIAuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<APIAuthController>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(int id, [FromBody] Usuario user)
        {
            return Ok(new { resultado = await _authRep.Register(user) });
        }

        // PUT api/<APIAuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<APIAuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
