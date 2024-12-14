using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.DTO
{
    public class InvitacionEventoDTO
    {
        public int InvitadoId { get; set; }

        public int? EventoId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Confirmacion { get; set; }
    }
}