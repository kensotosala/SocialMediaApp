using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class AuthRepository : IAuth
    {
        private readonly SocialMediaDBContext _context;

        public AuthRepository(SocialMediaDBContext context)
        {
            _context = context;
        }

        public int Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Register(Usuario user)
        {
            _context.Add(user);
            var res = _context.SaveChangesAsync();
            return await res;
        }

        public Task<Usuario?> getByUsername(string username)
        {
            return _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == username);
        }

        public Task<Usuario?> getByEmail(string email)
        {
            return _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
