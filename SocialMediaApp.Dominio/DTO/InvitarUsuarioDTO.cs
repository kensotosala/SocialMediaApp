using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.DTO
{
    public class InvitarUsuarioDTO
    {
        public int? EventoId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Confirmacion { get; set; }

        public virtual Evento? Evento { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
