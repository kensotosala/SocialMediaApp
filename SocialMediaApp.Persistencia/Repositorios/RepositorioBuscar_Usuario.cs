using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class RepositorioBuscar_Usuario : IBuscar_Usuario
    {
        private readonly SocialMediaDBContext _context;
<<<<<<< HEAD
=======

>>>>>>> pr/1
        public RepositorioBuscar_Usuario(SocialMediaDBContext context)
        {
            _context = context;
        }
<<<<<<< HEAD
        // Obtiene un usuario específico
        public Task<List<Usuario>> ObtenerListaUsuariosAsync()
        {
            return _context.Usuarios.ToListAsync();
        }
        //Buscar por nombre de usuario
=======

        // Obtiene un evento específico
         public Task<List<Usuario>> ObtenerListaUsuariosAsync()
         {
                return _context.Usuarios.ToListAsync();
         }

        //Buscar por nombre completo de curso
>>>>>>> pr/1
        public Task<Usuario?> ObtenerUsuarioXNombreAsync(string NombreUsuario)
        {
            return _context.Usuarios.Include(b => b.BuscarUsuarios).FirstOrDefaultAsync(n => n.NombreUsuario == NombreUsuario);
        }
<<<<<<< HEAD
    }
}



=======


    }
}
>>>>>>> pr/1
