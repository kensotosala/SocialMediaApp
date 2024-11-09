using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IEvento
    {
        public Task<IEnumerable<Evento>> ObtenerEventosParaUsuarioAsync(int UsuarioId);
        public Task AgregarEventoAsync(Evento evento);
        public Task InvitarUsuarioAsync(int eventoId, int usuarioId);
        public Task ConfirmarAsistenciaAsync(int eventoId, int usuarioId, string confirmacion);
    }
}
