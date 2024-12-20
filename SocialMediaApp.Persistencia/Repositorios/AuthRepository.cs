using Azure.Core;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> ChangePassword(Usuario userUpdate)
        {
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == userUpdate.NombreUsuario);

            if (existingUser == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            _context.Entry(existingUser).Property(u => u.Contraseña).CurrentValue = userUpdate.Contraseña;
            _context.Entry(existingUser).Property(u => u.SalContraseña).CurrentValue = userUpdate.SalContraseña;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ChangeQuestion(ChangeQuestionRequest request)
        {
            var existingUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == request.Username);

            if (existingUser == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            _context.Entry(existingUser).Property(u => u.Pregunta).CurrentValue = request.Pregunta;
            _context.Entry(existingUser).Property(u => u.Respuesta).CurrentValue = request.Respuesta;

            return await _context.SaveChangesAsync();

        }

        public async Task<int> ChangeProfile(Profile updates)
        {
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == updates.NombreUsuario);

            if (existingUser == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            _context.Entry(existingUser).Property(u => u.Nombre).CurrentValue = updates.Nombre;
            _context.Entry(existingUser).Property(u => u.Apellido).CurrentValue = updates.Apellido;
            
            if(updates.Biografia != null)
                _context.Entry(existingUser).Property(u => u.Biografia).CurrentValue = updates.Biografia;

            if (updates.Ubicacion != null)
                _context.Entry(existingUser).Property(u => u.Ubicacion).CurrentValue = updates.Ubicacion;
            
            _context.Entry(existingUser).Property(u => u.Email).CurrentValue = updates.Email;

            return await _context.SaveChangesAsync();


        }
    }
}
