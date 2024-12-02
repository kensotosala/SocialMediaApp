﻿using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Dominio.Interfaces;
using SocialMediaApp.Persistencia.Data;

namespace SocialMediaApp.Persistencia.Repositorios
{
    public class AuthRepository : IAuth
    {
        private readonly SocialMediaDBContext _context;

        public AuthRepository(SocialMediaDBContext context)
        {
            _context = context;
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